using Diagrammatist.Application.AppServices.Simulator.Serializer.Witness;
using Diagrammatist.Domain.Document;
using Diagrammatist.Domain.Simulator;
using Diagrammatist.Domain.Simulator.Flowchart;
using Nerdbank.MessagePack;

namespace Diagrammatist.Application.AppServices.Simulator.Serializer.Configuration
{
    /// <summary>
    /// A class that configures serialization for simulation domains.
    /// </summary>
    public class SimulationSerializationConfigurator
    {
        /// <summary>
        /// Gets required node and context mappings.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DerivedTypeMapping> GetMappings()
        {
            var nodeMapping = new DerivedShapeMapping<SimulationNodeData>();
            nodeMapping.Add<FlowchartSimulationNodeData, NodeWitness>(1);

            var contextMapping = new DerivedShapeMapping<IPayloadData>();
            contextMapping.Add<SimulationContextData, ContextWitness>(1);

            return [nodeMapping, contextMapping];
        }
    }
}
