namespace Diagrammatist.Domain.Simulator.Flowchart
{
    /// <summary>
    /// A class that derives from <see cref="SimulationNodeData"/>. 
    /// This class represents flowchart simulation node data.
    /// </summary>
    public class FlowchartSimulationNodeData : SimulationNodeData
    {
        /// <inheritdoc/>
        public override string LuaScript { get; set; } = string.Empty;
    }
}
