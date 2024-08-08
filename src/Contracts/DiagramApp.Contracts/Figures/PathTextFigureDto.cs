namespace DiagramApp.Contracts.Figures
{
    public class PathTextFigureDto : PathFigureDto
    {
        public required string Text { get; set; }
        public required double FontSize { get; set; }
    }
}
