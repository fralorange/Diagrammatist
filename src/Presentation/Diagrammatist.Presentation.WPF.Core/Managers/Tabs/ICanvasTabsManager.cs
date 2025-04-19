using Diagrammatist.Presentation.WPF.Core.Models.Canvas;
using System.Collections.ObjectModel;

namespace Diagrammatist.Presentation.WPF.Core.Managers.Tabs
{
    /// <summary>
    /// An interface that is responsible for canvas tabs management.
    /// </summary>
    public interface ICanvasTabsManager
    {
        /// <summary>
        /// Gets observable canvases.
        /// </summary>
        ObservableCollection<CanvasModel> Canvases { get; }
        /// <summary>
        /// Adds new canvas.
        /// </summary>
        /// <param name="canvas">New canvas.</param>
        /// <param name="filePath">Optional file path.</param>
        void Add(CanvasModel canvas, string filePath = "");
        /// <summary>
        /// Removes existing canvas.
        /// </summary>
        /// <param name="canvas">Target canvas.</param>
        void Remove(CanvasModel canvas);
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
