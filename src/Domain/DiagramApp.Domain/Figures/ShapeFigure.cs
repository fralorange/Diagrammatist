using DiagramApp.Domain.Figures.Constants;

namespace DiagramApp.Domain.Figures
{
    /// <summary>
    /// Shape figure.
    /// </summary>
    public class ShapeFigure : Figure
    {
        /// <summary>
        /// Width.
        /// </summary>
        public double Width { get; set; } = FigureManipulationConstants.DefaultWidth;
        /// <summary>
        /// Height.
        /// </summary>
        public double Height { get; set; } = FigureManipulationConstants.DefaultHeight;
    }
}
