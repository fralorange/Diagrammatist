using DiagramApp.Contracts.Canvas;
using DiagramApp.Domain.Canvas.Constants;

namespace DiagramApp.Application.AppServices.Services.Canvas
{
    public class CanvasZoomService : ICanvasZoomService
    {
        public void ZoomIn(CanvasDto canvas, double zoomFactor, int? mouseX = null, int? mouseY = null)
        {
            var offsetX = canvas.Offset.X;
            var offsetY = canvas.Offset.Y;

            double newPositionX = offsetX;
            double newPositionY = offsetY;

            if (mouseX.HasValue && mouseY.HasValue)
            {
                newPositionX = offsetX - (int)((mouseX.Value - offsetX) * zoomFactor);
                newPositionY = offsetY - (int)((mouseY.Value - offsetY) * zoomFactor);
            }

            canvas.Offset.X = newPositionX;
            canvas.Offset.Y = newPositionY;
            canvas.Zoom = Math.Min(CanvasZoomConstants.MaxZoom, canvas.Zoom + zoomFactor);

            UpdateImaginaryBorders(canvas);
            EnsureCanvasWithinBorders(canvas);
        }

        public void ZoomOut(CanvasDto canvas, double zoomFactor, int? mouseX = null, int? mouseY = null)
        {
            var offsetX = canvas.Offset.X;
            var offsetY = canvas.Offset.Y;

            double newPositionX = offsetX;
            double newPositionY = offsetY;

            if (mouseX.HasValue && mouseY.HasValue)
            {
                newPositionX = offsetX + (int)((mouseX.Value - offsetX) * zoomFactor);
                newPositionY = offsetY + (int)((mouseY.Value - offsetY) * zoomFactor);
            }

            canvas.Offset.X = newPositionX;
            canvas.Offset.Y = newPositionY;
            canvas.Zoom = Math.Max(CanvasZoomConstants.MinZoom, canvas.Zoom - zoomFactor);

            UpdateImaginaryBorders(canvas);
            EnsureCanvasWithinBorders(canvas);
        }

        private void UpdateImaginaryBorders(CanvasDto canvas)
        {
            var settings = canvas.Settings;
            var zoom = canvas.Zoom;

            int borderWidth = (int)(settings.Width * 4 * zoom);
            int borderHeight = (int)(settings.Height * 4 * zoom);

            int borderCenterX = 0;
            int borderCenterY = 0;

            canvas.ImaginaryWidth = Math.Abs(borderCenterX - borderWidth / 2) + Math.Abs(borderCenterX + borderWidth / 2);
            canvas.ImaginaryHeight = Math.Abs(borderCenterY - borderHeight / 2) + Math.Abs(borderCenterY + borderHeight / 2);
        }

        private void EnsureCanvasWithinBorders(CanvasDto canvas)
        {
            var offsetX = canvas.Offset.X;
            var offsetY = canvas.Offset.Y;

            if (offsetX < 0 || offsetX > canvas.ImaginaryWidth)
            {
                canvas.Offset.X = Math.Max(0, Math.Min(offsetX, canvas.ImaginaryWidth));
            }

            if (offsetY < 0 || offsetY > canvas.ImaginaryHeight)
            {
                canvas.Offset.Y = Math.Max(0, Math.Min(offsetY, canvas.ImaginaryHeight));
            }
        }

        public void ZoomReset(CanvasDto canvas)
        {
            canvas.Zoom = CanvasZoomConstants.DefaultZoom;
        }
    }
}
