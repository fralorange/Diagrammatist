using Diagrammatist.Presentation.WPF.Core.Models.Connection;
using Diagrammatist.Presentation.WPF.Simulator.Models.Node;

namespace Diagrammatist.Presentation.WPF.Simulator.Models.Context
{
    /// <summary>
    /// A class that represents simulation context.
    /// </summary>
    public class SimulationContext
    {
        /// <summary>
        /// Gets or sets simulation nodes.
        /// </summary>
        public required IEnumerable<SimulationNode> Nodes { get; set; }
        /// <summary>
        /// Gets or sets figure connections.
        /// </summary>
        public required IEnumerable<ConnectionModel> Connections { get; set; }
    }
}
