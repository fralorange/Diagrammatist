using DiagramApp.Contracts.Canvas;

namespace DiagramApp.Application.AppServices.Contexts.Canvas.Services
{
    /// <summary>
    /// Canvas interaction service.
    /// </summary>
    public interface ICanvasInteractionService
    {
        /// <summary>
        /// Pan canvas.
        /// </summary>
        /// <param name="canvas">Canvas model.</param>
        /// <param name="newX">New position by X-axis.</param>
        /// <param name="newY">New position by Y-axis.</param>
        void Pan(CanvasDto canvas, double newX, double newY);
        /// <summary>
        /// Zoom canvas.
        /// </summary>
        /// <param name="canvas">Canvas model.</param>
        /// <param name="zoomFactor">Zoom factor.</param>
        /// <param name="mouseX">Mouse position by X-axis (optional).</param>
        /// <param name="mouseY">Mouse position by Y-axis (optional).</param>
        void Zoom(CanvasDto canvas, double zoomFactor, int? mouseX = null, int? mouseY = null);
    }
}
