using Diagrammatist.Presentation.WPF.Core.Models.Connection;
using Diagrammatist.Presentation.WPF.Core.Models.Figures;
using Diagrammatist.Presentation.WPF.Simulator.Models.Engine;
using Diagrammatist.Presentation.WPF.Simulator.Models.Node;

namespace Diagrammatist.Presentation.WPF.Simulator.Interfaces
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
        /// <param name="existingNodes"></param>
        /// <returns></returns>
        IEnumerable<SimulationNode> CreateNodes(IEnumerable<FigureModel> figures, IEnumerable<SimulationNode>? existingNodes = null);
        /// <summary>
        /// Creates simulation engine.
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="connections"></param>
        /// <param name="io"></param>
        /// <param name="contextProvider"></param>
        /// <returns></returns>
        ISimulationEngine CreateEngine(IEnumerable<SimulationNode> nodes,
                                       IEnumerable<ConnectionModel> connections,
                                       ISimulationIO io,
                                       ISimulationContextProvider contextProvider);
    }
}
