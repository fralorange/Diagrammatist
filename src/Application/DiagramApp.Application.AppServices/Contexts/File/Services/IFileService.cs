using DiagramApp.Contracts.Canvas;

namespace DiagramApp.Application.AppServices.Contexts.File.Services
{
    public interface IFileService
    {
        byte[] Save(CanvasDto canvasDto);
        CanvasDto? Load(string filePath);
    }
}
