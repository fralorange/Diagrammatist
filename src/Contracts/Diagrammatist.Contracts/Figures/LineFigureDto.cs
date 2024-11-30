using System.Drawing;

namespace DiagramApp.Contracts.Figures
{
    /// <summary>
    /// A line figure dto. Derived class from <see cref="FigureDto"/>.
    /// </summary>
    public class LineFigureDto : FigureDto
    {
        /// <summary>
        /// Gets or sets collection of points.
        /// </summary>
        /// <remarks>
        /// This property used to draw line by points.
        /// </remarks>
        public required List<Point> Points { get; set; } = [];
        /// <summary>
        /// Gets or sets line thickness.
        /// </summary>
        /// <remarks>
        /// This property used to configure line thickness.
        /// </remarks>
        public required double Thickness { get; set; }
        /// <summary>
        /// Gets or sets line dash condition.
        /// </summary>
        /// <remarks>
        /// This property indicates whether the line is dashed.
        /// </remarks>
        public required bool IsDashed { get; set; }
        /// <summary>
        /// Gets or sets line arrow condition.
        /// </summary>
        /// <remarks>
        /// This property indicates whether the line has arrow on last point.
        /// </remarks>
        public required bool HasArrow { get; set; }
    }
}
