using Diagrammatist.Application.AppServices.Document.Serializer;
using DocumentEnt = Diagrammatist.Domain.Document.Document;

namespace Diagrammatist.Application.AppServices.Document.Services
{
    /// <summary>
    /// A class that implements <see cref="IDocumentSerializationService"/>. Provides with I/O operations.
    /// </summary>
    public class DocumentSerializationService : IDocumentSerializationService
    {
        private readonly IDocumentSerializer _documentSerializer;

        /// <summary>
        /// Initializes document serialization service.
        /// </summary>
        /// <param name="documentSerializer"></param>
        public DocumentSerializationService(IDocumentSerializer documentSerializer)
        {
            _documentSerializer = documentSerializer;
        }
        
        /// <inheritdoc/>
        public void Save(DocumentEnt document, string filePath)
        {
            var data = _documentSerializer.Serialize(document);
            File.WriteAllBytes(filePath, data);
        }

        /// <inheritdoc/>
        public DocumentEnt? Load(string filePath)
        {
            var data = File.ReadAllBytes(filePath);
            return _documentSerializer.Deserialize(data);
        }
    }
}
