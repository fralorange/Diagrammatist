using System.Drawing;

namespace DiagramApp.Domain.Figures
{
    public class PolylineFigure : Figure
    {
        public List<Point> Points { get; set; } = [];
    }
}
