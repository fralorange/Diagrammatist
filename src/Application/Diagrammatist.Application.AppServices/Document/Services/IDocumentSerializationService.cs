using DocumentEnt = Diagrammatist.Domain.Document.Document;

namespace Diagrammatist.Application.AppServices.Document.Services
{
    /// <summary>
    /// An interface that provides with top-level serialization and I/O operations.
    /// </summary>
    public interface IDocumentSerializationService
    {
        /// <summary>
        /// Saves <see cref="DocumentEnt"/> to binary file.
        /// </summary>
        /// <param name="document"></param>
        /// <param name="filePath"></param>
        void Save(DocumentEnt document, string filePath);
        /// <summary>
        /// Loads <see cref="DocumentEnt"/> from binary file.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        DocumentEnt? Load(string filePath);
    }
}
