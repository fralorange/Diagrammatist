using DiagramApp.Contracts.Canvas;

namespace DiagramApp.Application.AppServices.Contexts.Canvas.Helpers
{
    /// <summary>
    /// Helps to update or set up canvas bounds.
    /// </summary>
    public static class CanvasBoundaryHelper
    {
        private const int BorderMultiplier = 4;

        /// <summary>
        /// Updates canvas bounds.
        /// </summary>
        /// <param name="canvas">Target canvas.</param>
        public static void UpdateCanvasBounds(CanvasDto canvas)
        {
            var settings = canvas.Settings;
            var zoom = canvas.Zoom;

            int borderWidth = (int)(settings.Width * BorderMultiplier * zoom);
            int borderHeight = (int)(settings.Height * BorderMultiplier * zoom);

            canvas.ImaginaryWidth = Math.Abs(0 - borderWidth / 2) + Math.Abs(0 + borderWidth / 2);
            canvas.ImaginaryHeight = Math.Abs(0 - borderHeight / 2) + Math.Abs(0 + borderHeight / 2);
        }
    }
}
