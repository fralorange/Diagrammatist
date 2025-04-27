using Diagrammatist.Application.AppServices.Document.Serializer.Witness;
using Nerdbank.MessagePack;
using DocumentEnt = Diagrammatist.Domain.Document.Document;

namespace Diagrammatist.Application.AppServices.Document.Serializer
{
    /// <summary>
    /// A class that implements <see cref="IDocumentSerializer"/>. Provides with serialization operations for document objects.
    /// </summary>
    public class DocumentSerializer : IDocumentSerializer
    {
        private readonly MessagePackSerializer _serializer;

        /// <summary>
        /// Initializes document serializer.
        /// </summary>
        /// <param name="serializer">Message pack serializer.</param>
        public DocumentSerializer(MessagePackSerializer serializer)
        {
            _serializer = serializer;
        }

        /// <inheritdoc/>
        public byte[] Serialize(DocumentEnt document)
        {
            return _serializer.Serialize<DocumentEnt, DocumentWitness>(document);
        }

        /// <inheritdoc/>
        public DocumentEnt? Deserialize(byte[] data)
        {
            return _serializer.Deserialize<DocumentEnt, DocumentWitness>(data);
        }
    }
}
