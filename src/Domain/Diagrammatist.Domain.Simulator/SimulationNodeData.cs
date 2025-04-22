namespace Diagrammatist.Domain.Simulator
{
    /// <summary>
    /// A class that defines simulation node data.
    /// </summary>
    public abstract class SimulationNodeData
    {
        /// <summary>
        /// Gets or sets decorated figure id.
        /// </summary>
        public required Guid FigureId { get; set; }
        /// <summary>
        /// Gets or sets lua script.
        /// </summary>
        public abstract string LuaScript { get; set; }
    }
}
