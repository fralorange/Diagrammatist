using DiagramApp.Contracts.Canvas;

namespace DiagramApp.Application.AppServices.Services.File
{
    public interface IFileService
    {
        void SaveAsync(CanvasDto canvasDto, string filePath);
        CanvasDto? LoadAsync(string filePath);
    }
}
