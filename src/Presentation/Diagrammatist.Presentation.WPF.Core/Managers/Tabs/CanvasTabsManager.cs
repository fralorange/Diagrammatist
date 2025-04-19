using Diagrammatist.Presentation.WPF.Core.Models.Canvas;
using System.Collections.ObjectModel;

namespace Diagrammatist.Presentation.WPF.Core.Managers.Tabs
{
    /// <summary>
    /// A class that implements <see cref="ICanvasTabsManager"/>.
    /// </summary>
    public class CanvasTabsManager : ICanvasTabsManager
    {
        private readonly ObservableCollection<CanvasModel> _canvases = [];
        private readonly Dictionary<CanvasModel, string> _filePaths = [];

        /// <inheritdoc/>
        public ObservableCollection<CanvasModel> Canvases => _canvases;

        /// <inheritdoc/>
        public void Add(CanvasModel canvas, string filePath = "")
        {
            if (!_canvases.Contains(canvas))
            {
                _canvases.Add(canvas);
                _filePaths[canvas] = filePath;
            }
        }

        /// <inheritdoc/>
        public void Remove(CanvasModel canvas)
        {
            _canvases.Remove(canvas);
                _filePaths.Remove(canvas);
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
