using System.Drawing;

namespace DiagramApp.Contracts.Figures
{
    /// <summary>
    /// Line figure dto.
    /// </summary>
    public class LineFigureDto : FigureDto
    {
        /// <summary>
        /// Collection of points.
        /// </summary>
        public required List<Point> Points { get; set; } = [];
        /// <summary>
        /// Line thickness.
        /// </summary>
        public required double Thickness { get; set; }
        /// <summary>
        /// Indicates whether the line is dashed.
        /// </summary>
        public required bool IsDashed { get; set; }
        /// <summary>
        /// Indicates whether the line has arrow on last point.
        /// </summary>
        public required bool HasArrow { get; set; }
    }
}
