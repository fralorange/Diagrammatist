using Diagrammatist.Contracts.Canvas;
using Diagrammatist.Contracts.Settings;

namespace Diagrammatist.Application.AppServices.Contexts.Canvas.Services
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
        /// <returns><see cref="CanvasDto"/>. Canvas model.</returns>
        Task<CanvasDto> CreateCanvasAsync(DiagramSettingsDto settings);
        /// <summary>
        /// Edits existing canvas.
        /// </summary>
        /// <param name="canvas">Target canvas.</param>
        /// <param name="settings">New settings.</param>
        void UpdateCanvas(CanvasDto canvas, DiagramSettingsDto settings);
    }
}
