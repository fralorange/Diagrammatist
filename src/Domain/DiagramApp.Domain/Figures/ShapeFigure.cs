using DiagramApp.Domain.Figures.Constants;

namespace DiagramApp.Domain.Figures
{
    /// <summary>
    /// A shape figure class. Derived class from <see cref="Figure"/>.
    /// </summary>
    public class ShapeFigure : Figure
    {
        /// <summary>
        /// Gets or sets width.
        /// </summary>
        /// <remarks>
        /// This property used to store figure width.
        /// </remarks>
        public double Width { get; set; } = FigureManipulationConstants.DefaultWidth;
        /// <summary>
        /// Gets or sets height.
        /// </summary>
        /// <remarks>
        /// This property used to store figure height.
        /// </remarks>
        public double Height { get; set; } = FigureManipulationConstants.DefaultHeight;
    }
}
