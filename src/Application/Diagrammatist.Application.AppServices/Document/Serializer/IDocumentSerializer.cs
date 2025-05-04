using DocumentEnt = Diagrammatist.Domain.Document.Document;

namespace Diagrammatist.Application.AppServices.Document.Serializer
{
    /// <summary>
    /// An interface that provides with serialization operations.
    /// </summary>
    public interface IDocumentSerializer
    {
        /// <summary>
        /// Serializes <see cref="DocumentEnt"/> to array of bytes.
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        byte[] Serialize(DocumentEnt document);
        /// <summary>
        /// Deserializes array of bytes into <see cref="DocumentEnt"/>.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        DocumentEnt? Deserialize(byte[] data);
    }
}
