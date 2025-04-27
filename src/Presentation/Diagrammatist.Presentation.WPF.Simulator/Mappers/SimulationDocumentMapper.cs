using Diagrammatist.Domain.Document;
using Diagrammatist.Domain.Simulator;
using Diagrammatist.Presentation.WPF.Core.Contracts.Document.Providers;
using Diagrammatist.Presentation.WPF.Core.Models.Document;
using Diagrammatist.Presentation.WPF.Simulator.Models.Context;

namespace Diagrammatist.Presentation.WPF.Simulator.Mappers
{
    /// <summary>
    /// A class that implements <see cref="IDocumentPayloadMapper"/>.
    /// Provides with simulation document mapping operations.
    /// </summary>
    public class SimulationDocumentMapper : IDocumentPayloadMapper
    {
        private readonly string _key = SimulationContextData.PayloadKey;

        /// <inheritdoc/>
        public void MapToModel(Document source, DocumentModel target)
        {
            if (source.Payloads.TryGetValue(_key, out var payload) && payload is SimulationContextData simContext)
            {
                target.SetPayload(_key, simContext.ToModel(target.Canvas.Figures));
            }
        }

        /// <inheritdoc/>
        public void MapToDomain(DocumentModel source, Document target)
        {
            if (source.GetPayloadData<SimulationContext>(_key) is SimulationContext simContext)
            {
                target.Payloads[_key] = simContext.ToDomain(target.Canvas.Figures);
            }
        }
    }
}
