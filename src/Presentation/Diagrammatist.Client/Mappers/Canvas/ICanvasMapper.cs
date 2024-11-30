using DiagramApp.Client.ViewModels.Wrappers;
using DiagramApp.Contracts.Canvas;

namespace DiagramApp.Client.Mappers.Canvas
{
    public interface ICanvasMapper
    {
        CanvasDto ToDto(ObservableCanvas canvas);
        ObservableCanvas FromDto(CanvasDto canvas);
    }
}
