using Diagrammatist.Presentation.WPF.Core.Models.Connection;
using Diagrammatist.Presentation.WPF.Core.Models.Figures.Special.Flowchart;
using Diagrammatist.Presentation.WPF.Simulator.Interfaces;
using Diagrammatist.Presentation.WPF.Simulator.Models.Node;
using Diagrammatist.Presentation.WPF.Simulator.Models.Node.Flowchart;
using NLua;
using Timer = System.Timers.Timer;

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
        // Simulation settings.
        private Timer? _simulationTimer;

        public TimeSpan SimulationTime { get; set; }

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
        public void Start(int milliseconds)
        {
            if (_simulationTimer != null) return;

            _simulationTimer = new Timer(milliseconds);
            _simulationTimer.Elapsed += (sender, e) =>
            {
                StepForward();
                SimulationTime.Add(TimeSpan.FromMilliseconds(milliseconds));
            };
            _simulationTimer.AutoReset = true;
            _simulationTimer.Start();
        }

        /// <inheritdoc/>
        public void Stop()
        {
            _simulationTimer?.Stop();
            _simulationTimer = null;
        }

        /// <inheritdoc/>
        public void StepForward()
        {
            if (CurrentNode == null || CurrentNode.Figure is not FlowchartFigureModel figure)
                return;

            _history.Push(CurrentNode);

            switch (figure.Subtype)
            {
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
                case FlowchartSubtypeModel.Connector:
                    HandleConnector();
                    break;
                case FlowchartSubtypeModel.Preparation:
                    HandleLoop();
                    break;
                case FlowchartSubtypeModel.StartEnd:
                    MoveToNext();
                    break;
                case FlowchartSubtypeModel.PredefinedProcess:
                    HandlePredefinedProcess();
                    break;
                case FlowchartSubtypeModel.Database:
                    HandleDatabase();
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
            Stop();

            ResetNode();

            _lua.Dispose();
            InitializeLua();
            _history.Clear();
        }

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

        private void ResetNode()
        {
            CurrentNode = _graph.Keys
                .FirstOrDefault(n => n.Figure is FlowchartFigureModel figure && figure.Subtype == FlowchartSubtypeModel.StartEnd);
        }

        private void InitializeLua()
        {
            _lua = new Lua();

            _lua.RegisterFunction("print", this, GetType().GetMethod(nameof(ShowOutput)));
            _lua.RegisterFunction("read", this, GetType().GetMethod(nameof(GetInput)));
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
            throw new NotImplementedException();
        }

        private void HandleConnector()
        {
            throw new NotImplementedException();
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

        private void BuildGraph(IEnumerable<FlowchartSimulationNode> nodes, IEnumerable<ConnectionModel> connections)
        {
            var figureToNode = nodes.ToDictionary(n => n.Figure);

            foreach (var connection in connections)
            {
                if (connection.SourceMagneticPoint?.Owner is FlowchartFigureModel sourceFig &&
                    connection.DestinationMagneticPoint?.Owner is FlowchartFigureModel destFig &&
                    figureToNode.TryGetValue(sourceFig, out var sourceNode) &&
                    figureToNode.TryGetValue(destFig, out var destNode))
                {
                    if (!_graph.ContainsKey(sourceNode))
                        _graph[sourceNode] = [];

                    _graph[sourceNode].Add(destNode);
                }
            }
        }
    }
}
