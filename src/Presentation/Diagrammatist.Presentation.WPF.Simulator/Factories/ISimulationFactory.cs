using Diagrammatist.Presentation.WPF.Core.Models.Connection;
using Diagrammatist.Presentation.WPF.Core.Models.Figures;
using Diagrammatist.Presentation.WPF.Simulator.Models.Engine;
using Diagrammatist.Presentation.WPF.Simulator.Models.Node;

namespace Diagrammatist.Presentation.WPF.Simulator.Factories
{
    /// <summary>
    /// An interface that provides node and engine creation operations.
    /// </summary>
    public interface ISimulationFactory
    {
        /// <summary>
        /// Creates simulation nodes.
        /// </summary>
        /// <param name="figures"></param>
        /// <returns></returns>
        IEnumerable<SimulationNodeBase> CreateNodes(IEnumerable<FigureModel> figures);
        /// <summary>
        /// Creates simulation engine.
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="connections"></param>
        /// <returns></returns>
        ISimulationEngine CreateEngine(IEnumerable<SimulationNodeBase> nodes, IEnumerable<ConnectionModel> connections);
    }
}
