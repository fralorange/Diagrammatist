using Diagrammatist.Presentation.WPF.Core.Contracts.Document.Providers;
using Diagrammatist.Presentation.WPF.Core.Mappers.Canvas;
using Diagrammatist.Presentation.WPF.Core.Models.Document;
using DocumentEnt = Diagrammatist.Domain.Document.Document;

namespace Diagrammatist.Presentation.WPF.Core.Mappers.Document
{
    /// <summary>
    /// Document mapper extension.
    /// </summary>
    public static class DocumentMapperExtension
    {
        private static readonly List<IDocumentPayloadMapper> _payloadMappers = new();

        /// <summary>
        /// Registers payload mapper.
        /// </summary>
        /// <param name="mapper"></param>
        public static void RegisterPayloadMapper(IDocumentPayloadMapper mapper)
        {
            _payloadMappers.Add(mapper);
        }

        /// <summary>
        /// Maps document domain to document model.
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public static DocumentModel ToModel(this DocumentEnt document)
        {
            var model = new DocumentModel 
            {
                Canvas = document.Canvas.ToModel(),
            };

            foreach (var mapper in _payloadMappers)
                mapper.MapToModel(document, model);

            return model;
        }

        /// <summary>
        /// Maps document model to document domain.
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public static DocumentEnt ToDomain(this DocumentModel document)
        {
            var domain = new DocumentEnt
            {
                Canvas = document.Canvas.ToDomain(),
            };

            foreach (var mapper in _payloadMappers)
                mapper.MapToDomain(document, domain);

            return domain;
        }
    }
}
