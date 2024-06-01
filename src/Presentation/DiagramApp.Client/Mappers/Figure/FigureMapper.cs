using DiagramApp.Client.ViewModels.Wrappers;
using DiagramApp.Contracts.Canvas.Figures;
using DiagramApp.Domain.Canvas.Figures;
using Microsoft.Maui.Controls.Shapes;

namespace DiagramApp.Client.Mappers.Figure
{
    public class FigureMapper : IFigureMapper
    {
        public FigureDto ToDto(ObservableFigure figure)
        {
            return figure switch
            {
                ObservablePathFigure pathFigure => new PathFigureDto
                {
                    Name = pathFigure.Name,
                    TranslationX = pathFigure.TranslationX,
                    TranslationY = pathFigure.TranslationY,
                    ZIndex = pathFigure.ZIndex,
                    PathData = pathFigure.PathData,
                    Width = pathFigure.Width,
                    Height = pathFigure.Height,
                    Aspect = pathFigure.Aspect,
                },
                ObservablePolylineFigure polylineFigure => new PolylineFigureDto
                {
                    Name = polylineFigure.Name,
                    TranslationX = polylineFigure.TranslationX,
                    TranslationY = polylineFigure.TranslationY,
                    ZIndex = polylineFigure.ZIndex,
                    Dashed = polylineFigure.Dashed,
                    LineJoin = polylineFigure.LineJoin.ToString(),
                    Thickness = polylineFigure.Thickness,
                    Points = polylineFigure.Points.ToList(),
                },
                ObservableTextFigure textFigure => new TextFigureDto
                {
                    Name = textFigure.Name,
                    TranslationX = textFigure.TranslationX,
                    TranslationY = textFigure.TranslationY,
                    ZIndex = textFigure.ZIndex,
                    Text = textFigure.Text,
                    FontSize = textFigure.FontSize,
                    HasBackground = textFigure.HasBackground,
                    HasOutline = textFigure.HasOutline,
                },
                _ => throw new InvalidOperationException("Unknown figure type"),
            };
        }

        public ObservableFigure FromDto(FigureDto dto)
        {
            return dto switch
            {
                PathFigureDto pathFigure => new ObservablePathFigure(new Domain.Canvas.Figures.PathFigure { Name = pathFigure.Name, PathData = pathFigure.PathData, })
                {
                    TranslationX = pathFigure.TranslationX,
                    TranslationY = pathFigure.TranslationY,
                    ZIndex = pathFigure.ZIndex,
                    Width = pathFigure.Width,
                    Height = pathFigure.Height,
                    Aspect = pathFigure.Aspect,
                },
                PolylineFigureDto polylineFigure => new ObservablePolylineFigure(new PolylineFigure { Name = polylineFigure.Name, Points = polylineFigure.Points.ToList()})
                {
                    TranslationX = polylineFigure.TranslationX,
                    TranslationY = polylineFigure.TranslationY,
                    ZIndex = polylineFigure.ZIndex,
                    Dashed = polylineFigure.Dashed,
                    LineJoin = (PenLineJoin)Enum.Parse(typeof(PenLineJoin), polylineFigure.LineJoin),
                    Thickness = polylineFigure.Thickness,
                },
                TextFigureDto textFigure => new ObservableTextFigure(new TextFigure { Name = textFigure.Name, Text = textFigure.Text })
                {
                    TranslationX = textFigure.TranslationX,
                    TranslationY = textFigure.TranslationY,
                    ZIndex = textFigure.ZIndex,
                    Text = textFigure.Text,
                    FontSize = textFigure.FontSize,
                    HasBackground = textFigure.HasBackground,
                    HasOutline = textFigure.HasOutline,
                },
                _ => throw new InvalidOperationException("Unknown figure dto type"),
            };
        }
    }
}
