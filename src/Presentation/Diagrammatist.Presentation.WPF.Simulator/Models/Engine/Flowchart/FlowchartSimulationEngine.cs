using Diagrammatist.Presentation.WPF.Core.Models.Canvas;
using Diagrammatist.Presentation.WPF.Core.Models.Figures.Special.Flowchart;
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
        private Lua _lua = new();

        private readonly Dictionary<FlowchartSimulationNode, List<FlowchartSimulationNode>> _graph = [];
        private FlowchartSimulationNode? _currentNode;

        private Stack<FlowchartSimulationNode> _history = [];

        private Timer? _simulationTimer;

        public TimeSpan SimulationTime { get; set; }

        /// <summary>
        /// Initializes flowchart simulation engine.
        /// </summary>
        public FlowchartSimulationEngine(CanvasModel canvas)
        {
            BuildGraph(canvas);
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
            if (_currentNode == null || _currentNode.Figure is not FlowchartFigureModel figure)
                return;

            _history.Push(_currentNode);

            switch (figure.Subtype)
            {
                case FlowchartSubtypeModel.Process:
                    _lua.DoString(_currentNode.LuaScript);
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
            //if (_history.Count > 0)
            //{
            //    _currentNode = _history.Pop();
            //}
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void Reset()
        {
            Stop();

            ResetNode();

            _lua.Dispose();
            _lua = new Lua();
            _history.Clear();
        }

        private void ResetNode()
        {
            _currentNode = _graph.Keys
                .FirstOrDefault(n => n.Figure is FlowchartFigureModel figure && figure.Subtype == FlowchartSubtypeModel.StartEnd);
        }

        private void MoveToNext()
        {
            if (_currentNode is null || !_graph.TryGetValue(_currentNode, out List<FlowchartSimulationNode>? value))
                return;

            _currentNode = value.FirstOrDefault();
        }

        private void MoveByDecision()
        {
            if (_currentNode is null || !_graph.ContainsKey(_currentNode))
                return;

            var result = _lua.DoString(_currentNode.LuaScript).FirstOrDefault();
            var cond = result is bool b && b;

            var nextNodes = _graph[_currentNode];
            _currentNode = cond ? nextNodes.ElementAtOrDefault(1) : nextNodes.ElementAtOrDefault(0);
        }

        private void HandleInputOutput()
        {
            throw new NotImplementedException();
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

        private void BuildGraph(CanvasModel canvas)
        {
            var nodes = canvas.Figures
                .OfType<FlowchartFigureModel>()
                .Select(f => new FlowchartSimulationNode { Figure = f })
                .ToDictionary(n => n.Figure);

            foreach (var connection in canvas.Connections)
            {
                if (connection.SourceMagneticPoint?.Owner is FlowchartFigureModel sourceFig &&
                    connection.DestinationMagneticPoint?.Owner is FlowchartFigureModel destFig &&
                    nodes.TryGetValue(sourceFig, out var sourceNode) &&
                    nodes.TryGetValue(destFig, out var destNode))
                {
                    if (!_graph.ContainsKey(sourceNode))
                    {
                        _graph[sourceNode] = [];
                    }

                    _graph[sourceNode].Add(destNode);
                }
            }

            foreach (var node in nodes.Values.ToList())
            {
                if (!_graph.ContainsKey(node) && !_graph.Values.Any(list => list.Contains(node)))
                {
                    nodes.Remove(node.Figure!);
                }
            }
        }
    }
}
