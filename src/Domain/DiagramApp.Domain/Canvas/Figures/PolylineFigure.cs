using System.Drawing;

namespace DiagramApp.Domain.Canvas.Figures
{
    public class PolylineFigure : Figure
    {
        public List<Point> Points { get; set; } = [];
    }
}
