using Diagrammatist.Presentation.WPF.Core.Models.Canvas;
using Diagrammatist.Presentation.WPF.Core.Models.Figures.Special.Flowchart;
using Diagrammatist.Presentation.WPF.Simulator.Models.Node.Flowchart;

namespace Diagrammatist.Presentation.WPF.Simulator.Models.Engine.Flowchart
{
    /// <summary>
    /// A class that implements <see cref="ISimulationEngine"/>.
    /// </summary>
    public class FlowchartSimulationEngine : ISimulationEngine
    {
        private readonly Dictionary<FlowchartSimulationNode, List<FlowchartSimulationNode>> _graph = [];

        public TimeSpan SimulationTime { get; set; }

        /// <summary>
        /// Initializes flowchart simulation engine.
        /// </summary>
        public FlowchartSimulationEngine(CanvasModel canvas)
        {
            BuildGraph(canvas);
        }

        /// <inheritdoc/>
        public void Start()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void Stop()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void StepForward()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void StepBackward()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void Reset()
        {
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
