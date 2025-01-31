using Diagrammatist.Domain.Canvas;
using System.Drawing;
using CanvasEntity = Diagrammatist.Domain.Canvas.Canvas;

namespace Diagrammatist.Application.AppServices.Canvas.Services
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
        /// <returns><see cref="CanvasEntity"/>. Canvas model.</returns>
        Task<CanvasEntity> CreateCanvasAsync(Settings settings);
        /// <summary>
        /// Edits existing canvas settings by changing its size.
        /// </summary>
        /// <param name="canvas">Target canvas.</param>
        /// <param name="size">New size.</param>
        void UpdateCanvas(CanvasEntity canvas, Size size);
        /// <summary>
        /// Edits existing canvas settings by changing its background color.
        /// </summary>
        /// <param name="canvas">Target canvas.</param>
        /// <param name="background">New background color.</param>
        void UpdateCanvas(CanvasEntity canvas, Color background);
    }
}
