using DiagramApp.Contracts.Canvas;
using DiagramApp.Domain.Settings;

namespace DiagramApp.Application.AppServices.Contexts.Canvas.Services
{
    /// <summary>
    /// Canvas manipulation service.
    /// </summary>
    public interface ICanvasManipulationService
    {
        /// <summary>
        /// Create new canvas.
        /// </summary>
        /// <param name="settings">Settings.</param>
        /// <returns><see cref="CanvasDto"/>. Canvas model.</returns>
        CanvasDto CreateCanvas(DiagramSettings settings);
    }
}
