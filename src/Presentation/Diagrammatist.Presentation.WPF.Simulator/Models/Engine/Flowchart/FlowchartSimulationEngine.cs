using Diagrammatist.Presentation.WPF.Core.Models.Connection;
using Diagrammatist.Presentation.WPF.Core.Models.Figures;
using Diagrammatist.Presentation.WPF.Core.Models.Figures.Special.Flowchart;
using Diagrammatist.Presentation.WPF.Simulator.Interfaces;
using Diagrammatist.Presentation.WPF.Simulator.Managers;
using Diagrammatist.Presentation.WPF.Simulator.Models.Node;
using Diagrammatist.Presentation.WPF.Simulator.Models.Node.Flowchart;
using System.Text.RegularExpressions;

namespace Diagrammatist.Presentation.WPF.Simulator.Models.Engine.Flowchart
{
    /// <summary>
    /// A class that implements <see cref="ISimulationEngine"/>.
    /// </summary>
    public partial class FlowchartSimulationEngine : ISimulationEngine
    {
        /// <inheritdoc/>
        public event EventHandler<SimulationNode?> CurrentNodeChanged;
        // Simulation parameters.
        /// <inheritdoc/>
        public bool IsCompleted => CurrentNode == _endNode;

        private readonly ISimulationIO _io;
        private readonly ISimulationContextProvider _contextProvider;

        private FlowchartSimulationEngine? _subEngine;
        private string[] _simulateParameters;
        // Lua
        private readonly LuaStateManager _luaStateManager;
        // Nodes.
        private readonly Dictionary<FlowchartSimulationNode, List<FlowchartSimulationNode>> _graph = [];
        private readonly Stack<FlowchartSimulationNode> _history = [];

        private FlowchartSimulationNode? _currentNode;
        private FlowchartSimulationNode? CurrentNode
        {
            get => _currentNode;
            set
            {
                if (_currentNode != value)
                {
                    _currentNode = value;
                    CurrentNodeChanged?.Invoke(this, value);
                }
            }
        }

        private FlowchartSimulationNode _endNode;

        /// <summary>
        /// Initializes flowchart simulation engine.
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="connections"></param>
        /// <param name="io"></param>
        /// <param name="contextProvider"></param>
#pragma warning disable CS8618
        public FlowchartSimulationEngine(IEnumerable<FlowchartSimulationNode> nodes,
                                         IEnumerable<ConnectionModel> connections,
                                         ISimulationIO io,
                                         ISimulationContextProvider contextProvider,
                                         LuaStateManager? luaStateManager = null)
#pragma warning restore CS8618 
        {
            BuildGraph(nodes, connections);

            _io = io;
            _contextProvider = contextProvider;
            _luaStateManager = (luaStateManager is not null) ? new LuaStateManager(luaStateManager) : new LuaStateManager();
        }

        /// <inheritdoc/>
        public void Initialize()
        {
            InitializeLua();
            ResetNode();
        }

        /// <inheritdoc/>
        public object[]? Simulate(params object[] args)
        {
            for (int i = 0; i < _simulateParameters.Length && i < args.Length; i++)
            {
                _luaStateManager.SetValue(_simulateParameters[i], args[i]);
            }

            // Iteration.
            while (!IsCompleted)
            {
                StepForward();
            }

            if (_history.Count >= 2)
            {
                var lastBeforeEnd = _history.ElementAt(1);

                if (lastBeforeEnd?.Figure is FlowchartFigureModel figure &&
                    figure.Subtype == FlowchartSubtypeModel.InputOutput)
                {
                    var script = lastBeforeEnd.LuaScript;
                    if (!string.IsNullOrWhiteSpace(script) && script.TrimStart().StartsWith("return"))
                    {
                        return _luaStateManager.Execute(script);
                    }
                }
            }

            return [];
        }

        /// <inheritdoc/>
        public void StepForward()
        {
            // Validation.
            if (IsCompleted)
                return;

            // Main engine.
            if (CurrentNode is null || CurrentNode.Figure is not FlowchartFigureModel figure)
                return;

            _history.Push(CurrentNode);

            switch (figure.Subtype)
            {
                case FlowchartSubtypeModel.StartEnd:
                case FlowchartSubtypeModel.Connector:
                    MoveToNext();
                    break;
                case FlowchartSubtypeModel.Process:
                    _luaStateManager.ExecuteWithSnapshot(CurrentNode.LuaScript);
                    MoveToNext();
                    break;
                case FlowchartSubtypeModel.InputOutput:
                    HandleInputOutput();
                    MoveToNext();
                    break;
                case FlowchartSubtypeModel.Decision:
                    MoveByDecision();
                    break;
                case FlowchartSubtypeModel.Preparation:
                    HandleLoop();
                    break;
                case FlowchartSubtypeModel.PredefinedProcess:
                    HandlePredefinedProcess();
                    MoveToNext();
                    break;
            }
        }

        /// <inheritdoc/>
        public void StepBackward()
        {
            if (_history.Count == 0)
                return;

            CurrentNode = _history.Pop();
            _luaStateManager.Undo();
        }

        /// <inheritdoc/>
        public void Reset()
        {
            ResetNode();

            _luaStateManager.Reset();
            _history.Clear();

            InitializeLua();
        }

        #region Lua handlers

        /// <summary>
        /// Gets input from dialog window.
        /// </summary>
        /// <param name="args">Input args.</param>
        public void GetInput(params object[] args)
        {
            var variableNames = args.Select(arg => arg.ToString()).Where(str => !string.IsNullOrEmpty(str)).ToList();
            if (variableNames is null || variableNames.Count == 0)
                return;

            var values = _io.GetInput(variableNames!);

            if (values is null)
                return;

            var luaCode = string.Join("\n", values.Select(kv => $"{kv.Key} = {kv.Value}"));
            _luaStateManager.Execute(luaCode);
        }

        /// <summary>
        /// Shows output in dialog window.
        /// </summary>
        /// <param name="args">Output args.</param>
        public void ShowOutput(params object[] args)
        {
            var message = string.Join(" ", args.Select(arg => arg?.ToString()));

            _io.ShowOutput(message);
        }

        /// <summary>
        /// Loops over with parameter.
        /// </summary>
        /// <param name="args">Loop args, which requires three arguments: iterator name, start-end, and step (optional).</param>
        /// <returns>A <see cref="bool"/> value. True if still in the loop, otherwise false.</returns>
        public bool Loop(params object[] args)
        {
            if (args.Length < 3 || args.Length > 4) return false;

            var varName = args[0]?.ToString();
            if (string.IsNullOrEmpty(varName)) return false;

            if (!double.TryParse(args[1]?.ToString(), out var from)) return false;
            if (!double.TryParse(args[2]?.ToString(), out var to)) return false;
            var step = (args.Length >= 4 && double.TryParse(args[3]?.ToString(), out var s)) ? s : 1;

            var val = _luaStateManager.GetValue(varName) as double?;

            if (val is null)
            {
                _luaStateManager.SetValue(varName, from);
                return true;
            }

            var next = val.Value + step;

            if ((step > 0 && next > to) || (step < 0 && next < to))
            {
                _luaStateManager.SetValue(varName, null);
                return false;
            }

            _luaStateManager.SetValue(varName, next);
            return true;
        }

        #endregion

        private void InitializeLua()
        {
            _luaStateManager.Initialize();

            _luaStateManager.RegisterFunctions(this,
                nameof(ShowOutput),
                nameof(GetInput),
                nameof(Loop));
        }

        #region Node Handlers

        private void ResetNode()
        {
            CurrentNode = _graph.Keys
                .FirstOrDefault(n => n.Figure is FlowchartFigureModel figure && figure.Subtype == FlowchartSubtypeModel.StartEnd);
        }

        private void MoveToNext()
        {
            if (CurrentNode is null || !_graph.TryGetValue(CurrentNode, out List<FlowchartSimulationNode>? value))
                return;

            CurrentNode = value.FirstOrDefault();

            if (_subEngine is not null && _subEngine.IsCompleted)
            {
                _subEngine = null;
            }
        }

        private void MoveByDecision()
        {
            if (CurrentNode is null || !_graph.TryGetValue(CurrentNode, out List<FlowchartSimulationNode>? nextNodes))
                return;

            var result = _luaStateManager.Execute(CurrentNode.LuaScript).FirstOrDefault();
            var cond = result is bool b && b;
            CurrentNode = cond ? nextNodes.ElementAtOrDefault(0) : nextNodes.ElementAtOrDefault(1);
        }

        private void HandleInputOutput()
        {
            if (CurrentNode == null || string.IsNullOrWhiteSpace(CurrentNode.LuaScript))
                return;

            var script = CurrentNode.LuaScript;

            bool hasIO = script.Contains("read") ||
                         script.Contains("print") ||
                         script.Contains("return");

            if (!hasIO)
                return;

            _luaStateManager.ExecuteWithSnapshot(script);
        }

        private void HandleLoop()
        {
            if (CurrentNode == null || string.IsNullOrWhiteSpace(CurrentNode.LuaScript))
                return;

            var result = _luaStateManager.ExecuteWithSnapshot(CurrentNode.LuaScript);
            var res = result.FirstOrDefault();

            if (res is bool shouldContinue && shouldContinue)
            {
                MoveToNext();
            }
            else
            {
                if (_graph.TryGetValue(CurrentNode, out var nextNodes))
                {
                    CurrentNode = nextNodes.ElementAtOrDefault(1);
                }
            }
        }

        private void HandlePredefinedProcess()
        {
            if (CurrentNode is null)
                return;

            var filePath = CurrentNode.ExternalFilePath;
            if (filePath?.EndsWith(".dgmf", StringComparison.OrdinalIgnoreCase) == true && _contextProvider.Load(filePath) is { } context)
            {
                _subEngine = new FlowchartSimulationEngine(context.Nodes.OfType<FlowchartSimulationNode>(),
                                                           context.Connections,
                                                           _io,
                                                           _contextProvider,
                                                           _luaStateManager);
                _subEngine.Initialize();

                var startNode = FindStartNode(context.Nodes.OfType<FlowchartSimulationNode>(), context.Connections);

                if (startNode is { Figure: FlowchartFigureModel { Text: { } signature } }
                    && FunctionSignatureRegex().Match(signature) is { Success: true } match)
                {
                    var functionName = match.Groups[1].Value;
                    var parameterNames = match.Groups[2].Value
                        .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries); ;

                    _luaStateManager.RegisterFunction(_subEngine, functionName, nameof(Simulate));

                    _subEngine._simulateParameters = parameterNames;
                }
            }
            else if (filePath?.EndsWith(".lua", StringComparison.OrdinalIgnoreCase) == true)
            {
                _luaStateManager.ExecuteFile(filePath);
            }
            _luaStateManager.ExecuteWithSnapshot(CurrentNode.LuaScript);
        }

        #endregion

        [GeneratedRegex(@"(\w+)\(([^)]*)\)")]
        private static partial Regex FunctionSignatureRegex();

        #region Graph Builders

        /// <summary>
        /// Builds outgoing dictionary.
        /// </summary>
        /// <param name="figureToNode">Dictionary of nodes.</param>
        /// <param name="connections">Logical connections between figures.</param>
        /// <returns>Outgoing <see cref="Dictionary{TKey, TValue}"/>.</returns>
        private Dictionary<FlowchartSimulationNode, List<FlowchartSimulationNode>> BuildOutgoingConnections(
            Dictionary<FigureModel, FlowchartSimulationNode> figureToNode,
            IEnumerable<ConnectionModel> connections)
        {
            var outgoing = new Dictionary<FlowchartSimulationNode, List<FlowchartSimulationNode>>();

            foreach (var connection in connections)
            {
                if (connection.SourceMagneticPoint?.Owner is FlowchartFigureModel sourceFig &&
                    connection.DestinationMagneticPoint?.Owner is FlowchartFigureModel destFig &&
                    figureToNode.TryGetValue(sourceFig, out var sourceNode) &&
                    figureToNode.TryGetValue(destFig, out var destNode))
                {
                    AddOutgoingEdge(outgoing, sourceNode, destNode);
                }
            }

            var labeledConnectors = figureToNode
                .Where(kvp => kvp.Key is FlowchartFigureModel { Subtype: FlowchartSubtypeModel.Connector, Text: not "" or not null } connector &&
                    connector.Text.Trim().StartsWith('#'))
                .GroupBy(kvp => ((FlowchartFigureModel)kvp.Key).Text!.Trim());

            foreach (var group in labeledConnectors)
            {
                var connectorNodes = group.Select(g => g.Value).ToList();
                if (connectorNodes.Count < 2)
                    continue;

                var source = connectorNodes.First();
                foreach (var target in connectorNodes.Skip(1))
                {
                    AddOutgoingEdge(outgoing, source, target);
                }
            }

            return outgoing;
        }

        /// <summary>
        /// Adds a relation between two nodes if it doesn't already exist.
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        private void AddOutgoingEdge(Dictionary<FlowchartSimulationNode, List<FlowchartSimulationNode>> dict,
                                    FlowchartSimulationNode from,
                                    FlowchartSimulationNode to)
        {
            if (!dict.ContainsKey(from))
                dict[from] = [];

            if (!dict[from].Contains(to))
                dict[from].Add(to);
        }

        /// <summary>
        /// Finds start node.
        /// </summary>
        /// <param name="nodes">Collection of nodes.</param>
        /// <param name="connections">Logical connections between figures.</param>
        /// <returns>Start node with subtype <see cref="FlowchartSubtypeModel.StartEnd"/>.</returns>
        private FlowchartSimulationNode? FindStartNode(
            IEnumerable<FlowchartSimulationNode> nodes,
            IEnumerable<ConnectionModel> connections)
        {
            var figureToNode = nodes.ToDictionary(n => n.Figure);
            var nodesWithIncoming = new HashSet<FlowchartSimulationNode>();

            foreach (var connection in connections)
            {
                if (connection.DestinationMagneticPoint?.Owner is FlowchartFigureModel destFig &&
                    figureToNode.TryGetValue(destFig, out var destNode))
                {
                    nodesWithIncoming.Add(destNode);
                }
            }

            return nodes
                .FirstOrDefault(n => !nodesWithIncoming.Contains(n) && n.Figure is FlowchartFigureModel { Subtype: FlowchartSubtypeModel.StartEnd });
        }

        /// <summary>
        /// Traverses from start node to the end node.
        /// </summary>
        /// <param name="startNode">Start node.</param>
        /// <param name="outgoing">Dictionary of nodes with outgoing connections.</param>
        private void TraverseFromStartNode(
            FlowchartSimulationNode startNode,
            Dictionary<FlowchartSimulationNode, List<FlowchartSimulationNode>> outgoing)
        {
            var visited = new HashSet<FlowchartSimulationNode>();
            var queue = new Queue<FlowchartSimulationNode>();
            queue.Enqueue(startNode);
            visited.Add(startNode);

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();

                if (!outgoing.TryGetValue(current, out var nextNodes))
                    continue;

                foreach (var next in nextNodes)
                {
                    if (!_graph.ContainsKey(current))
                        _graph[current] = [];

                    _graph[current].Add(next);

                    if (visited.Add(next))
                        queue.Enqueue(next);
                }
            }
        }

        /// <summary>
        /// Calculates end node.
        /// </summary>
        private void CalculateEndNode()
        {
            _endNode = _graph.Values
                .SelectMany(v => v)
                .FirstOrDefault(fig => fig.Figure is FlowchartFigureModel { Subtype: FlowchartSubtypeModel.StartEnd })
                ?? throw new InvalidOperationException("The ending node of the flowchart could not be determined.");
        }

        /// <summary>
        /// Builds graph.
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="connections"></param>
        private void BuildGraph(IEnumerable<FlowchartSimulationNode> nodes, IEnumerable<ConnectionModel> connections)
        {
            var figureToNode = nodes.ToDictionary(n => n.Figure);

            var outgoing = BuildOutgoingConnections(figureToNode, connections);
            var startNode = FindStartNode(nodes, connections)
                ?? throw new InvalidOperationException("The starting node of the flowchart could not be determined.");
            _graph.Clear();

            TraverseFromStartNode(startNode, outgoing);
            CalculateEndNode();
        }

        #endregion
    }
}
