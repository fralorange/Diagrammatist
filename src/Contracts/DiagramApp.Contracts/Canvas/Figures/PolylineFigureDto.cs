using System.Drawing;

namespace DiagramApp.Contracts.Canvas.Figures
{
    public class PolylineFigureDto : FigureDto
    {
        public required List<Point> Points { get; set; } = [];
        public required double Thickness { get; set; }
        public required bool Dashed { get; set; }
        public required string LineJoin { get; set; }
    }
}
