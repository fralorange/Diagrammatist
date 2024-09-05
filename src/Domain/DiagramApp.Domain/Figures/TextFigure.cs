using DiagramApp.Domain.Figures.Constants;
using System.Drawing;

namespace DiagramApp.Domain.Figures
{
    /// <summary>
    /// A text figure class. Derived class from <see cref="Figure"/>.
    /// </summary>
    public class TextFigure : Figure
    {
        /// <summary>
        /// Gets or sets figure Text.
        /// </summary>
        /// <remarks>
        /// This property used to display text in figure UI.
        /// </remarks>
        public string Text { get; set; } = FigureTextConstants.DefaultText;
        /// <summary>
        /// Gets or sets figure text color.
        /// </summary>
        /// <remarks>
        /// This property used to store figure text color.
        /// </remarks>
        public Color TextColor { get; set; } = Color.Black;
        /// <summary>
        /// Gets or sets font size.
        /// </summary>
        /// <remarks>
        /// This property used to configure font size.
        /// </remarks>
        public double FontSize { get; set; } = TextFigureManipulationConstants.DefaultFontSize;
        /// <summary>
        /// Gets or sets figure outline condition.
        /// </summary>
        /// <remarks>
        /// This property indicates whether figure has outline.
        /// </remarks>
        public bool HasOutline { get; set; } = TextFigureBoolConstants.DefaultOutlineParameter;
        /// <summary>
        /// Gets or sets figure background condition,
        /// </summary>
        /// <remarks>
        /// This property indicates whether figure has background.
        /// </remarks>
        public bool HasBackground { get; set; } = TextFigureBoolConstants.DefaultBackgroundParameter;
    }
}
