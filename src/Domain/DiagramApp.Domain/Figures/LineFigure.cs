using DiagramApp.Domain.Figures.Constants;
using System.Drawing;

namespace DiagramApp.Domain.Figures
{
    /// <summary>
    /// A line figure class. Derived class from <see cref="Figure"/>.
    /// </summary>
    public class LineFigure : Figure
    {
        /// <summary>
        /// Gets or sets collection of points.
        /// </summary>
        /// <remarks>
        /// This property used to draw line by points.
        /// </remarks>
        public List<Point> Points { get; set; } = [];
        /// <summary>
        /// Gets or sets line thickness.
        /// </summary>
        /// <remarks>
        /// This property used to configure line thickness.
        /// </remarks>
        public double Thickness { get; set; } = LineFigureManipulationConstants.DefaultThickness;
        /// <summary>
        /// Gets or sets line dash condition.
        /// </summary>
        /// <remarks>
        /// This property indicates whether the line is dashed.
        /// </remarks>
        public bool IsDashed { get; set; } = LineFigureBoolConstants.DefaultDashedParameter;
        /// <summary>
        /// Gets or sets line arrow condition.
        /// </summary>
        /// <remarks>
        /// This property indicates whether the line has arrow on last point.
        /// </remarks>
        public bool HasArrow { get; set; } = LineFigureBoolConstants.DefaultArrowParameter;
    }
}
