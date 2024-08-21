namespace DiagramApp.Contracts.Figures
{
    /// <summary>
    /// Shape figure.
    /// </summary>
    public class ShapeFigureDto : FigureDto
    {
        /// <summary>
        /// Width.
        /// </summary>
        public required double Width { get; set; }
        /// <summary>
        /// Height.
        /// </summary>
        public required double Height { get; set; }
    }
}
