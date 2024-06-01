using DiagramApp.Contracts.Canvas;

namespace DiagramApp.Application.AppServices.Services.File
{
    public interface IFileService
    {
        byte[] Save(CanvasDto canvasDto);
        CanvasDto? Load(string filePath);
    }
}
