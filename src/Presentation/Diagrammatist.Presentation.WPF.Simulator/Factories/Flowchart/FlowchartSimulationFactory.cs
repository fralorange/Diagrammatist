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
        public IEnumerable<SimulationNodeBase> CreateNodes(IEnumerable<FigureModel> figures)
        {
            return figures
                .OfType<FlowchartFigureModel>()
                .Select(f => new FlowchartSimulationNode { Figure = f });
        }

        /// <inheritdoc/>
        public ISimulationEngine CreateEngine(IEnumerable<SimulationNodeBase> nodes, IEnumerable<ConnectionModel> connections, ISimulationIO io)
        {
            return new FlowchartSimulationEngine(
                nodes.OfType<FlowchartSimulationNode>(),
                connections,
                io);
        }
    }
}
