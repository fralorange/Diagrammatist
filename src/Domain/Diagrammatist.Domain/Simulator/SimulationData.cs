using Diagrammatist.Domain.Connection;

namespace Diagrammatist.Domain.Simulator
{
    /// <summary>
    /// A class that represents simulation data.
    /// </summary>
    public class SimulationData
    {
        /// <summary>
        /// Gets or sets data of simulation nodes.
        /// </summary>
        public required List<SimulationNodeData> Nodes { get; set; }
        /// <summary>
        /// Gets or sets data of connections.
        /// </summary>
        public required List<ConnectionData> Connections { get; set; }
    }
}
