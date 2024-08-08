using DiagramApp.Contracts.Canvas;

namespace DiagramApp.Application.AppServices.Services.Canvas
{
    public interface ICanvasMoveService
    {
        void MoveCanvas(CanvasDto canvas, double newX, double newY);
    }
}
