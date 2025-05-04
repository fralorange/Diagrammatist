using Diagrammatist.Domain.Connection;
using Diagrammatist.Domain.Document;

namespace Diagrammatist.Domain.Simulator
{
    /// <summary>
    /// A class that represents simulation context data.
    /// </summary>
    public class SimulationContextData : IPayloadData
    {
        public const string PayloadKey = "Simulation";
        /// <inheritdoc/>
        public string Key => PayloadKey;
        /// <summary>
        /// Gets or sets data of simulation nodes.
        /// </summary>
        public required IEnumerable<SimulationNodeData> Nodes { get; set; }
        /// <summary>
        /// Gets or sets data of connections.
        /// </summary>
        public required IEnumerable<ConnectionData> Connections { get; set; }
    }
}
