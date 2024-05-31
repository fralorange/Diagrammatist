namespace DiagramApp.Contracts.Canvas.Figures
{
    public class PathFigureDto : FigureDto
    {
        public required string PathData { get; set; }
        public required double Width { get; set; }
        public required double Height { get; set; }
        public required bool Aspect { get; set; }
    }
}
