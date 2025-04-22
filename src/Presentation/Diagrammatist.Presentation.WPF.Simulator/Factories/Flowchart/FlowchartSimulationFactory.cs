using Diagrammatist.Presentation.WPF.Core.Models.Connection;
using Diagrammatist.Presentation.WPF.Core.Models.Figures;
using Diagrammatist.Presentation.WPF.Core.Models.Figures.Special.Flowchart;
using Diagrammatist.Presentation.WPF.Simulator.Interfaces;
using Diagrammatist.Presentation.WPF.Simulator.Models.Engine;
using Diagrammatist.Presentation.WPF.Simulator.Models.Engine.Flowchart;
using Diagrammatist.Presentation.WPF.Simulator.Models.Node;
using Diagrammatist.Presentation.WPF.Simulator.Models.Node.Flowchart;

namespace Diagrammatist.Presentation.WPF.Simulator.Factories.Flowchart
{
    /// <summary>
    /// A class that implements <see cref="ISimulationFactory"/>. Provides flowchart node and engine creation operations.
    /// </summary>
    public class FlowchartSimulationFactory : ISimulationFactory
    {
        /// <inheritdoc/>
        public IEnumerable<SimulationNode> CreateNodes(IEnumerable<FigureModel> figures, IEnumerable<SimulationNode>? existingNodes = null)
        {
            var existingDict = existingNodes?
                .OfType<FlowchartSimulationNode>()
                .ToDictionary(n => n.Figure.Id) ?? [];

            return figures
                .OfType<FlowchartFigureModel>()
                .Select(f =>
                {
                    if (existingDict.TryGetValue(f.Id, out var existingNode))
                    {
                        existingNode.Figure = f;
                        return existingNode;
                    }
                    return new FlowchartSimulationNode { Figure = f };
                });
        }

        /// <inheritdoc/>
        public IEnumerable<ConnectionModel> CreateConnections(IEnumerable<ConnectionModel> currentConnections, 
            IEnumerable<ConnectionModel>? existingConnections = null)
        {
            if (existingConnections is null)
                return currentConnections;

            var existingDict = existingConnections?
                .ToDictionary(c => c.Line.Id) ?? [];

            return currentConnections.Select(c =>
            {
                if (existingDict.TryGetValue(c.Line.Id, out var existing))
                {
                    existing.SourceMagneticPoint = c.SourceMagneticPoint;
                    existing.DestinationMagneticPoint = c.DestinationMagneticPoint;
                    existing.Line = c.Line;
                    return existing;
                }
                return c; 
            });
        }

        /// <inheritdoc/>
        public ISimulationEngine CreateEngine(IEnumerable<SimulationNode> nodes, IEnumerable<ConnectionModel> connections, ISimulationIO io)
        {
            return new FlowchartSimulationEngine(
                nodes.OfType<FlowchartSimulationNode>(),
                connections,
                io);
        }
    }
}
