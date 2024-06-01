using DiagramApp.Contracts.Canvas;

namespace DiagramApp.Application.AppServices.Services.File
{
    public interface IFileService
    {
        void Save(CanvasDto canvasDto, string filePath);
        CanvasDto? Load(string filePath);
    }
}
