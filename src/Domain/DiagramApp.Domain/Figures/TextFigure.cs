using DiagramApp.Domain.Figures.Constants;

namespace DiagramApp.Domain.Figures
{
    /// <summary>
    /// Text figure.
    /// </summary>
    public class TextFigure : Figure
    {
        /// <summary>
        /// Figure Text.
        /// </summary>
        public string Text { get; set; } = FigureTextConstants.DefaultText;
        /// <summary>
        /// Font size.
        /// </summary>
        public double FontSize { get; set; } = TextFigureManipulationConstants.DefaultFontSize;
        /// <summary>
        /// Indicates whether figure has outline.
        /// </summary>
        public bool HasOutline { get; set; } = TextFigureBoolConstants.DefaultOutlineParameter;
        /// <summary>
        /// Indicates whether figure has background.
        /// </summary>
        public bool HasBackground { get; set; } = TextFigureBoolConstants.DefaultBackgroundParameter;
    }
}
