using DiagramApp.Contracts.Canvas;

namespace DiagramApp.Application.AppServices.Services.Canvas
{
    public interface ICanvasZoomService
    {
        void ZoomIn(CanvasDto canvas, double zoomFactor, int? mouseX = null, int? mouseY = null);
        void ZoomOut(CanvasDto canvas, double zoomFactor, int? mouseX = null, int? mouseY = null);
    }
}
