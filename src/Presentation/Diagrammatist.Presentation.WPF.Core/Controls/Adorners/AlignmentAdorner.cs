using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Diagrammatist.Presentation.WPF.Core.Controls.Adorners
{
    /// <summary>
    /// A class that derives from <see cref="Adorner"/>.
    /// This class is used to draw alignment guides on the adorned element.
    /// </summary>
    public class AlignmentAdorner : Adorner
    {
        /// <summary>
        /// A collection of lines that represent the alignment guides.
        /// </summary>
        public List<Line> GuideLines { get; } = [];

        private readonly Pen _pen = new(new SolidColorBrush(Colors.HotPink), 1.5) { DashStyle = DashStyles.Dash };

        /// <summary>
        /// Initializes a new instance of the <see cref="AlignmentAdorner"/> class.
        /// </summary>
        /// <param name="adornedElement"> The element to be adorned.</param>
        public AlignmentAdorner(UIElement adornedElement) : base(adornedElement)
        {
            IsHitTestVisible = false;
        }

        /// <summary>
        /// Sets guides to be drawn on the adorned element.
        /// </summary>
        /// <param name="lines"></param>
        public void SetGuides(List<Line> lines)
        {
            GuideLines.Clear();
            GuideLines.AddRange(lines);
            InvalidateVisual();
        }

        /// <summary>
        /// Clears the guides from the adorned element.
        /// </summary>
        public void ClearGuides()
        {
            GuideLines.Clear();
            InvalidateVisual();
        }

        /// <inheritdoc/>
        protected override void OnRender(DrawingContext drawingContext)
        {
            foreach (var line in GuideLines)
            {
                drawingContext.DrawLine(_pen, new(line.X1, line.Y1), new(line.X2, line.Y2));
            }
        }
    }
}
