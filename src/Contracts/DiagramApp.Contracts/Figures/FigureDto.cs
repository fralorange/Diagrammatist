namespace DiagramApp.Contracts.Figures
{
    public abstract class FigureDto
    {
        public required string Name { get; set; }
        public required double TranslationX { get; set; }
        public required double TranslationY { get; set; }
        public required double Rotation { get; set; }
        public required double ZIndex { get; set; }
    }
}
