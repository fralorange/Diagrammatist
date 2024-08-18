using DiagramApp.Domain.Figures.Constants;
using System.Drawing;

namespace DiagramApp.Domain.Figures
{
    /// <summary>
    /// Line figure.
    /// </summary>
    public class LineFigure : Figure
    {
        /// <summary>
        /// Collection of points.
        /// </summary>
        public List<Point> Points { get; set; } = [];
        /// <summary>
        /// Line thickness.
        /// </summary>
        public double Thickness { get; set; } = LineFigureManipulationConstants.DefaultThickness;
        /// <summary>
        /// Indicates whether the line is dashed.
        /// </summary>
        public bool IsDashed { get; set; } = LineFigureBoolConstants.DefaultDashedParameter;
        /// <summary>
        /// Indicates whether the line has arrow on last point.
        /// </summary>
        public bool HasArrow { get; set; } = LineFigureBoolConstants.DefaultArrowParameter;
    }
}
