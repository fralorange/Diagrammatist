using Diagrammatist.Domain.Canvas;
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
        /// Edits existing canvas settings.
        /// </summary>
        /// <param name="canvas">Target canvas.</param>
        /// <param name="settings">New settings.</param>
        void UpdateCanvasSettings(CanvasEntity canvas, Settings settings);
    }
}
