using Diagrammatist.Domain.Document;
using Diagrammatist.Domain.Simulator;
using PolyType;

namespace Diagrammatist.Application.AppServices.Simulator.Serializer.Witness
{
    /// <summary>
    /// A simulation context data witness partial class.
    /// </summary>
    [GenerateShape<IPayloadData>]
    [GenerateShape<SimulationContextData>]
    public partial class ContextWitness;
}
