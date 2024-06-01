using DiagramApp.Client.ViewModels.Wrappers;
using DiagramApp.Contracts.Canvas.Figures;

namespace DiagramApp.Client.Mappers.Figure
{
    public interface IFigureMapper
    {
        FigureDto ToDto(ObservableFigure figure);
        ObservableFigure FromDto(FigureDto dto);
    }
}
