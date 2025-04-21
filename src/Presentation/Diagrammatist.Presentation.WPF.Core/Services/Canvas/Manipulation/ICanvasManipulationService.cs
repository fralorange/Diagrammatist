using Diagrammatist.Presentation.WPF.Core.Models.Canvas;
using System.Windows;
using System.Windows.Media;

namespace Diagrammatist.Presentation.WPF.Core.Services.Canvas.Manipulation
{
    /// <summary>
    /// An interface for canvas manipulation operations.
    /// </summary>
    public interface ICanvasManipulationService
    {
        /// <summary>
        /// Create new canvas.
        /// </summary>
        /// <param name="settings">Settings.</param>
        /// <returns><see cref="CanvasModel"/>. Canvas model.</returns>
        Task<CanvasModel> CreateCanvasAsync(SettingsModel settings);
        /// <summary>
        /// Edits existing canvas settings by changing its size.
        /// </summary>
        /// <param name="canvas">Target canvas.</param>
        /// <param name="size">New size.</param>
        void UpdateCanvas(CanvasModel canvas, Size size);
        /// <summary>
        /// Edits existing canvas settings by changing its background color.
        /// </summary>
        /// <param name="canvas">Target canvas.</param>
        /// <param name="background">New background color.</param>
        void UpdateCanvas(CanvasModel canvas, Color background);
    }
}
