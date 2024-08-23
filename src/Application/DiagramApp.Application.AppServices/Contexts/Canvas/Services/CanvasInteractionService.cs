using DiagramApp.Application.AppServices.Contexts.Canvas.Helpers;
using DiagramApp.Contracts.Canvas;
using DiagramApp.Domain.Canvas.Constants;

namespace DiagramApp.Application.AppServices.Contexts.Canvas.Services
{
    /// <inheritdoc cref="ICanvasInteractionService"/>
    public class CanvasInteractionService : ICanvasInteractionService
    {
        /// <inheritdoc/>
        public void Pan(CanvasDto canvas, double newX, double newY)
        {
            canvas.ScreenOffset.X = newX;
            canvas.ScreenOffset.Y = newY;

            EnsureCanvasWithinBorders(canvas);
        }

        /// <inheritdoc/>
        public void Zoom(CanvasDto canvas, bool isZoomIn = true, int? mouseX = null, int? mouseY = null)
        {
            var oldZoom = canvas.Zoom;

            canvas.Zoom = (isZoomIn)
                ? Math.Min(CanvasZoomConstants.MaxZoom, canvas.Zoom * CanvasZoomConstants.ZoomInFactor)
                : Math.Max(CanvasZoomConstants.MinZoom, canvas.Zoom * CanvasZoomConstants.ZoomOutFactor);

            UpdateCanvasOffset(canvas, isZoomIn, oldZoom, mouseX, mouseY);
            CanvasBoundaryHelper.UpdateCanvasBounds(canvas);
            EnsureCanvasWithinBorders(canvas);
        }

        private void UpdateCanvasOffset(CanvasDto canvas, bool isZoomIn, double oldZoom, int? mouseX, int? mouseY)
        {
            var newOffsetX = canvas.ScreenOffset.X;
            var newOffsetY = canvas.ScreenOffset.Y;

            if (mouseX.HasValue && mouseY.HasValue)
            {
                var factor = (isZoomIn) ? CanvasZoomConstants.ZoomInFactor : CanvasZoomConstants.ZoomOutFactor;

                newOffsetX = (int)Math.Abs((mouseX.Value * (factor - oldZoom) + factor * canvas.ScreenOffset.X));
                newOffsetY = (int)Math.Abs((mouseY.Value * (factor - oldZoom) + factor * canvas.ScreenOffset.Y));
            }

            canvas.ScreenOffset.X = newOffsetX;
            canvas.ScreenOffset.Y = newOffsetY;
        }

        private void EnsureCanvasWithinBorders(CanvasDto canvas)
        {
            canvas.ScreenOffset.X = Math.Max(0, Math.Min(canvas.ScreenOffset.X, canvas.ImaginaryWidth));
            canvas.ScreenOffset.Y = Math.Max(0, Math.Min(canvas.ScreenOffset.Y, canvas.ImaginaryHeight));
        }
    }
}
