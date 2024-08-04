using DiagramApp.Contracts.Canvas.Figures;

namespace DiagramApp.Application.AppServices.Services.Clipboard
{
    public interface IClipboardService
    {
        string ToClipboardString(FigureDto figure);
        FigureDto? ToObjectFromClipboard(string clipboard);
    }
}
