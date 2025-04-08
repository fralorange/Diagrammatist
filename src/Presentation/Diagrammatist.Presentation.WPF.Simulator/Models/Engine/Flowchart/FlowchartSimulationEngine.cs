using Diagrammatist.Presentation.WPF.Core.Models.Connection;
using Diagrammatist.Presentation.WPF.Core.Models.Figures;
using Diagrammatist.Presentation.WPF.Core.Models.Figures.Special.Flowchart;
using Diagrammatist.Presentation.WPF.Simulator.Interfaces;
using Diagrammatist.Presentation.WPF.Simulator.Models.Node;
using Diagrammatist.Presentation.WPF.Simulator.Models.Node.Flowchart;
using NLua;

namespace Diagrammatist.Presentation.WPF.Simulator.Models.Engine.Flowchart
{
    /// <summary>
    /// A class that implements <see cref="ISimulationEngine"/>.
    /// </summary>
    public partial class FlowchartSimulationEngine : ISimulationEngine
    {
        /// <inheritdoc/>
        public event EventHandler<SimulationNodeBase?> CurrentNodeChanged;

        private readonly ISimulationIO _io;
        private Lua _lua;
        // Nodes.
        private readonly Dictionary<FlowchartSimulationNode, List<FlowchartSimulationNode>> _graph = [];

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

        private Stack<FlowchartSimulationNode> _history = [];

        /// <summary>
        /// Initializes flowchart simulation engine.
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="connections"></param>
#pragma warning disable CS8618
        public FlowchartSimulationEngine(IEnumerable<FlowchartSimulationNode> nodes, IEnumerable<ConnectionModel> connections, ISimulationIO io)
#pragma warning restore CS8618 
        {
            BuildGraph(nodes, connections);

            _io = io;
        }

        /// <inheritdoc/>
        public void Initialize()
        {
            InitializeLua();
            ResetNode();
        }

        /// <inheritdoc/>
        public void StepForward()
        {
            if (CurrentNode == null || CurrentNode.Figure is not FlowchartFigureModel figure)
                return;

            _history.Push(CurrentNode);

            switch (figure.Subtype)
            {
                case FlowchartSubtypeModel.StartEnd:
                case FlowchartSubtypeModel.Connector:
                    MoveToNext();
                    break;
                case FlowchartSubtypeModel.Process:
                    _lua.DoString(CurrentNode.LuaScript);
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
                    //HandlePredefinedProcess();
                    MoveToNext();
                    break;
                case FlowchartSubtypeModel.Database:
                    //HandleDatabase();
                    MoveToNext();
                    break;
            }
        }

        /// <inheritdoc/>
        public void StepBackward()
        {
            if (_history.Count > 0)
            {
                CurrentNode = _history.Pop();
            }
        }

        /// <inheritdoc/>
        public void Reset()
        {
            ResetNode();

            _lua.Dispose();
            InitializeLua();
            _history.Clear();
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
            _lua.DoString(luaCode);
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

            var val = _lua[varName] as double?;

            if (val is null)
            {
                _lua[varName] = from;
                return true;
            }

            var next = val.Value + step;

            if ((step > 0 && next > to) || (step < 0 && next < to))
            {
                _lua[varName] = null;
                return false;
            }

            _lua[varName] = next;
            return true;
        }

        #endregion

        private void InitializeLua()
        {
            _lua = new Lua();

            _lua.RegisterFunction("print", this, GetType().GetMethod(nameof(ShowOutput)));
            _lua.RegisterFunction("read", this, GetType().GetMethod(nameof(GetInput)));
            _lua.RegisterFunction("loop", this, GetType().GetMethod(nameof(Loop)));
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
        }

        private void MoveByDecision()
        {
            if (CurrentNode is null || !_graph.TryGetValue(CurrentNode, out List<FlowchartSimulationNode>? nextNodes))
                return;

            var result = _lua.DoString(CurrentNode.LuaScript).FirstOrDefault();
            var cond = result is bool b && b;
            CurrentNode = cond ? nextNodes.ElementAtOrDefault(0) : nextNodes.ElementAtOrDefault(1);
        }

        private void HandleInputOutput()
        {
            if (CurrentNode == null || string.IsNullOrWhiteSpace(CurrentNode.LuaScript))
                return;

            var script = CurrentNode.LuaScript;

            bool hasIO = script.Contains("read") ||
                         script.Contains("print");

            if (!hasIO)
                return;

            _lua.DoString(script);
        }

        private void HandleLoop()
        {
            if (CurrentNode == null || string.IsNullOrWhiteSpace(CurrentNode.LuaScript))
                return;

            var result = _lua.DoString(CurrentNode.LuaScript);
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
            // Implement .lua file with method? or smth like that
            throw new NotImplementedException();
        }

        private void HandleDatabase()
        {
            // implement sqlite or text file database operations?
            throw new NotImplementedException();
        }

        #endregion

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
        }

        #endregion
    }
}
