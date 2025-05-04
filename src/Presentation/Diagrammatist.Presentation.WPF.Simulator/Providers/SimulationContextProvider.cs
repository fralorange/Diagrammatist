using Diagrammatist.Application.AppServices.Document.Services;
using Diagrammatist.Presentation.WPF.Core.Mappers.Document;
using Diagrammatist.Presentation.WPF.Core.Models.Document;
using Diagrammatist.Presentation.WPF.Simulator.Interfaces;
using Diagrammatist.Presentation.WPF.Simulator.Models.Context;

namespace Diagrammatist.Presentation.WPF.Simulator.Providers
{
    /// <summary>
    /// A class that provides required simulation context.
    /// </summary>
    public class SimulationContextProvider : ISimulationContextProvider
    {
        private const string Key = "Simulation";
        
        private readonly IDocumentSerializationService _serializationService;

        /// <summary>
        /// Initializes simulation context provider.
        /// </summary>
        /// <param name="serializationService"></param>
        public SimulationContextProvider(IDocumentSerializationService serializationService)
        {
            _serializationService = serializationService;
        }

        /// <inheritdoc/>
        public SimulationContext? Load(string path)
        {
            var doc = _serializationService.Load(path)?.ToModel();

            return doc?.GetPayloadData<SimulationContext>(Key);
        }
    }
}
