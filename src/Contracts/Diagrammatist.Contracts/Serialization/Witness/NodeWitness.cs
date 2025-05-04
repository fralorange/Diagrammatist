using Diagrammatist.Domain.Simulator;
using Diagrammatist.Domain.Simulator.Flowchart;
using PolyType;

namespace Diagrammatist.Application.AppServices.Simulator.Serializer.Witness
{
    /// <summary>
    /// A node witness partial class.
    /// </summary>
    [GenerateShape<SimulationNodeData>]
    [GenerateShape<FlowchartSimulationNodeData>]
    public partial class NodeWitness;
}
