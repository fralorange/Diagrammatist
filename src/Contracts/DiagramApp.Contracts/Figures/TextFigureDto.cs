namespace DiagramApp.Contracts.Figures
{
    /// <summary>
    /// Text figure.
    /// </summary>
    public class TextFigureDto : FigureDto
    {
        /// <summary>
        /// Figure Text.
        /// </summary>
        public required string Text { get; set; }
        /// <summary>
        /// Font size.
        /// </summary>
        public required double FontSize { get; set; }
        /// <summary>
        /// Indicates whether figure has outline.
        /// </summary>
        public required bool HasOutline { get; set; }
        /// <summary>
        /// Indicates whether figure has background.
        /// </summary>
        public required bool HasBackground { get; set; }
    }
}
