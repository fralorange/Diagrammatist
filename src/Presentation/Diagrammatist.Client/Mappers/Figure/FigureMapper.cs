//using DiagramApp.Client.ViewModels.Wrappers;
//using DiagramApp.Contracts.Canvas.Figures;
//using DiagramApp.Domain.Canvas.Figures;

//namespace DiagramApp.Client.Mappers.Figure
//{
//    public class FigureMapper : IFigureMapper
//    {
//        public FigureDto ToDto(ObservableFigure figure)
//        {
//            return figure switch
//            {
//                ObservablePathTextFigure pathTextFigure => new PathTextFigureDto
//                {
//                    Name = pathTextFigure.Name,
//                    TranslationX = pathTextFigure.TranslationX,
//                    TranslationY = pathTextFigure.TranslationY,
//                    Rotation = pathTextFigure.Rotation,
//                    ZIndex = pathTextFigure.ZIndex,
//                    PathData = pathTextFigure.PathData,
//                    Width = pathTextFigure.Width,
//                    Height = pathTextFigure.Height,
//                    Aspect = pathTextFigure.Aspect,
//                    Text = pathTextFigure.Text,
//                    FontSize = pathTextFigure.FontSize,
//                },
//                ObservablePathFigure pathFigure => new PathFigureDto
//                {
//                    Name = pathFigure.Name,
//                    TranslationX = pathFigure.TranslationX,
//                    TranslationY = pathFigure.TranslationY,
//                    Rotation = pathFigure.Rotation,
//                    ZIndex = pathFigure.ZIndex,
//                    PathData = pathFigure.PathData,
//                    Width = pathFigure.Width,
//                    Height = pathFigure.Height,
//                    Aspect = pathFigure.Aspect,
//                },
//                ObservablePolylineFigure polylineFigure => new PolylineFigureDto
//                {
//                    Name = polylineFigure.Name,
//                    TranslationX = polylineFigure.TranslationX,
//                    TranslationY = polylineFigure.TranslationY,
//                    Rotation = polylineFigure.Rotation,
//                    ZIndex = polylineFigure.ZIndex,
//                    Dashed = polylineFigure.Dashed,
//                    Arrow = polylineFigure.Arrow,
//                    LineJoin = polylineFigure.LineJoin.ToString(),
//                    Thickness = polylineFigure.Thickness,
//                    Points = polylineFigure.Points.ToList(),
//                },
//                ObservableTextFigure textFigure => new TextFigureDto
//                {
//                    Name = textFigure.Name,
//                    TranslationX = textFigure.TranslationX,
//                    TranslationY = textFigure.TranslationY,
//                    Rotation = textFigure.Rotation,
//                    ZIndex = textFigure.ZIndex,
//                    Text = textFigure.Text,
//                    FontSize = textFigure.FontSize,
//                    HasBackground = textFigure.HasBackground,
//                    HasOutline = textFigure.HasOutline,
//                },
//                _ => throw new InvalidOperationException("Unknown figure type"),
//            };
//        }

//        public ObservableFigure FromDto(FigureDto dto)
//        {
//            return dto switch
//            {
//                PathTextFigureDto pathTextFigure => new ObservablePathTextFigure(new PathFigure { Name = pathTextFigure.Name, PathData = pathTextFigure.PathData, })
//                {
//                    TranslationX = pathTextFigure.TranslationX,
//                    TranslationY = pathTextFigure.TranslationY,
//                    Rotation = pathTextFigure.Rotation,
//                    ZIndex = pathTextFigure.ZIndex,
//                    Width = pathTextFigure.Width,
//                    Height = pathTextFigure.Height,
//                    Aspect = pathTextFigure.Aspect,
//                    Text = pathTextFigure.Text,
//                    FontSize = pathTextFigure.FontSize,
//                },
//                PathFigureDto pathFigure => new ObservablePathFigure(new PathFigure { Name = pathFigure.Name, PathData = pathFigure.PathData, })
//                {
//                    TranslationX = pathFigure.TranslationX,
//                    TranslationY = pathFigure.TranslationY,
//                    Rotation = pathFigure.Rotation,
//                    ZIndex = pathFigure.ZIndex,
//                    Width = pathFigure.Width,
//                    Height = pathFigure.Height,
//                    Aspect = pathFigure.Aspect,
//                },
//                PolylineFigureDto polylineFigure => new ObservablePolylineFigure(new PolylineFigure { Name = polylineFigure.Name, Points = polylineFigure.Points.ToList() })
//                {
//                    TranslationX = polylineFigure.TranslationX,
//                    TranslationY = polylineFigure.TranslationY,
//                    Rotation = polylineFigure.Rotation,
//                    ZIndex = polylineFigure.ZIndex,
//                    Dashed = polylineFigure.Dashed,
//                    Arrow = polylineFigure.Arrow,
//                    LineJoin = (Microsoft.Maui.Controls.Shapes.PenLineJoin)Enum.Parse(typeof(Microsoft.Maui.Controls.Shapes.PenLineJoin), polylineFigure.LineJoin),
//                    Thickness = polylineFigure.Thickness,
//                },
//                TextFigureDto textFigure => new ObservableTextFigure(new TextFigure { Name = textFigure.Name, Text = textFigure.Text })
//                {
//                    TranslationX = textFigure.TranslationX,
//                    TranslationY = textFigure.TranslationY,
//                    Rotation = textFigure.Rotation,
//                    ZIndex = textFigure.ZIndex,
//                    Text = textFigure.Text,
//                    FontSize = textFigure.FontSize,
//                    HasBackground = textFigure.HasBackground,
//                    HasOutline = textFigure.HasOutline,
//                },
//                _ => throw new InvalidOperationException("Unknown figure dto type"),
//            };
//        }
//    }
//}
