using Diagrammatist.Presentation.WPF.Core.Models.Canvas;
using Diagrammatist.Presentation.WPF.Core.Models.Document;
using System.Collections.ObjectModel;

namespace Diagrammatist.Presentation.WPF.Core.Managers.Tabs
{
    /// <summary>
    /// A class that implements <see cref="IDocumentTabsManager"/>.
    /// </summary>
    public class DocumentTabsManager : IDocumentTabsManager
    {
        private readonly ObservableCollection<CanvasModel> _canvases = [];
        private readonly Dictionary<CanvasModel, string> _filePaths = [];
        private readonly Collection<DocumentModel> _documents = [];

        /// <inheritdoc/>
        public ObservableCollection<CanvasModel> Canvases => _canvases;

        /// <inheritdoc/>
        public DocumentModel? Get(CanvasModel? model)
        {
            return _documents.FirstOrDefault(doc => doc.Canvas == model);
        }

        /// <inheritdoc/>
        public void Add(DocumentModel document, string filePath = "")
        {
            if (!_canvases.Contains(document.Canvas) && document.Canvas is { } canvas)
            {
                _canvases.Add(canvas);
                _filePaths[canvas] = filePath;
                _documents.Add(document);
            }
        }

        /// <inheritdoc/>
        public void Remove(DocumentModel document)
        {
            if (document.Canvas is { } canvas)
            {
                _canvases.Remove(canvas);
                _filePaths.Remove(canvas);
                _documents.Remove(document);
            }
        }

        /// <inheritdoc/>
        public bool ContainsFilePath(string filePath)
        {
            return _filePaths.ContainsValue(filePath);
        }

        /// <inheritdoc/>
        public string? GetFilePath(CanvasModel canvas)
        {
            return _filePaths.TryGetValue(canvas, out var path) ? path : null;
        }

        /// <inheritdoc/>
        public void UpdateFilePath(CanvasModel canvas, string filePath)
        {
            if (_canvases.Contains(canvas))
            {
                _filePaths[canvas] = filePath;
            }
        }
    }
}
