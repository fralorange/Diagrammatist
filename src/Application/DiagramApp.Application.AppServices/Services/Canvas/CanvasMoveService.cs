using DiagramApp.Contracts.Canvas;

namespace DiagramApp.Application.AppServices.Services.Canvas
{
    public class CanvasMoveService : ICanvasMoveService
    {
        public void MoveCanvas(CanvasDto canvas, double newX, double newY)
        {
            canvas.Offset.X = newX; 
            canvas.Offset.Y = newY;
        }
    }
}
