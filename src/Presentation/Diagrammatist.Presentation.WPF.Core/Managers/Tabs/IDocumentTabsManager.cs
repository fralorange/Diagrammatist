using Diagrammatist.Presentation.WPF.Core.Models.Canvas;
using Diagrammatist.Presentation.WPF.Core.Models.Document;
using System.Collections.ObjectModel;

namespace Diagrammatist.Presentation.WPF.Core.Managers.Tabs
{
    /// <summary>
    /// An interface that is responsible for document tabs management.
    /// </summary>
    public interface IDocumentTabsManager
    {
        /// <summary>
        /// Gets observable canvases.
        /// </summary>
        ObservableCollection<CanvasModel> Canvases { get; }
        /// <summary>
        /// Gets document model by canvas model.
        /// </summary>
        /// <param name="canvas">A canvas model that acts as a key.</param>
        /// <returns><see cref="DocumentModel"/>.</returns>
        DocumentModel? Get(CanvasModel? canvas);
        /// <summary>
        /// Adds new document.
        /// </summary>
        /// <param name="document">New document.</param>
        /// <param name="filePath">Optional file path.</param>
        void Add(DocumentModel document, string filePath = "");
        /// <summary>
        /// Removes existing document.
        /// </summary>
        /// <param name="document">Target document.</param>
        void Remove(DocumentModel document);
        /// <summary>
        /// Determines whether manager contains file path.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns><c>true</c> if manager does contain target file path, otherwise <c>false</c>.</returns>
        bool ContainsFilePath(string filePath);
        /// <summary>
        /// Gets target canvas file path.
        /// </summary>
        /// <param name="canvas">Target canvas.</param>
        /// <returns><see cref="string"/> file path of the canvas, if it exists.</returns>
        string? GetFilePath(CanvasModel canvas);
        /// <summary>
        /// Updates target canvas file path.
        /// </summary>
        /// <param name="canvas">Target canvas.</param>
        /// <param name="filePath">New file path.</param>
        void UpdateFilePath(CanvasModel canvas, string filePath);
    }
}
