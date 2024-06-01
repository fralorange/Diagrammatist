namespace DiagramApp.Contracts.Canvas.Figures
{
    public class TextFigureDto : FigureDto
    {
        public required string Text { get; set; }
        public required double FontSize { get; set; }
        public required bool HasOutline { get; set; }
        public required bool HasBackground { get; set; }
    }
}
