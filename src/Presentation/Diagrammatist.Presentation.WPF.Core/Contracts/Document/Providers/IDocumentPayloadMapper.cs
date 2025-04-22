using Diagrammatist.Presentation.WPF.Core.Models.Document;
using DocumentEnt = Diagrammatist.Domain.Document.Document;

namespace Diagrammatist.Presentation.WPF.Core.Contracts.Document.Providers
{
    /// <summary>
    /// An interface that represents document payload mapper.
    /// </summary>
    public interface IDocumentPayloadMapper
    {
        /// <summary>
        /// Maps <see cref="DocumentEnt"/> instance to <see cref="DocumentModel"/>.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        void MapToModel(DocumentEnt source, DocumentModel target);
        /// <summary>
        /// Maps <see cref="DocumentModel"/> instance to <see cref="DocumentEnt"/>.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        void MapToDomain(DocumentModel source, DocumentEnt target);
    }
}
