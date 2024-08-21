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
        public void Zoom(CanvasDto canvas, double zoomFactor, int? mouseX = null, int? mouseY = null)
        {
            var isZoomIn = canvas.Zoom < zoomFactor;

            UpdateCanvasOffset(canvas, zoomFactor, mouseX, mouseY, isZoomIn);

            canvas.Zoom = (isZoomIn)
                ? Math.Min(CanvasZoomConstants.MaxZoom, canvas.Zoom + zoomFactor)
                : Math.Max(CanvasZoomConstants.MinZoom, canvas.Zoom - zoomFactor);

            CanvasBoundaryHelper.UpdateCanvasBounds(canvas);
            EnsureCanvasWithinBorders(canvas);
        }

        private void UpdateCanvasOffset(CanvasDto canvas, double zoomFactor, int? mouseX, int? mouseY, bool isZoomIn)
        {
            var newOffsetX = canvas.ScreenOffset.X;
            var newOffsetY = canvas.ScreenOffset.Y;

            if (mouseX.HasValue && mouseY.HasValue)
            {
                var factor = (isZoomIn) ? zoomFactor : -zoomFactor;
                newOffsetX -= (int)((mouseX.Value - newOffsetX) * factor);
                newOffsetY -= (int)((mouseY.Value - newOffsetY) * factor);
            }

            canvas.ScreenOffset.X = newOffsetX;
            canvas.ScreenOffset.Y = newOffsetY;
        }

        private void EnsureCanvasWithinBorders(CanvasDto canvas)
        {
            var offsetX = canvas.ScreenOffset.X;
            var offsetY = canvas.ScreenOffset.Y;

            if (offsetX < 0 || offsetX > canvas.ImaginaryWidth)
            {
                canvas.ScreenOffset.X = Math.Max(0, Math.Min(offsetX, canvas.ImaginaryWidth));
            }

            if (offsetY < 0 || offsetY > canvas.ImaginaryHeight)
            {
                canvas.ScreenOffset.Y = Math.Max(0, Math.Min(offsetY, canvas.ImaginaryHeight));
            }
        }
    }
}
