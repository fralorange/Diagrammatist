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
        /// <summary>
        /// Maps document domain to document model.
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public static DocumentModel ToModel(this DocumentEnt document)
        {
            return new DocumentModel
            {
                Canvas = document.Canvas.ToModel(),
            };
        }

        /// <summary>
        /// Maps document model to document domain.
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public static DocumentEnt ToDomain(this DocumentModel document)
        {
            return new DocumentEnt
            {
                Canvas = document.Canvas.ToDomain(),
            };
        }
    }
}
