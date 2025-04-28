using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace Diagrammatist.Presentation.WPF.Core.Controls
{
    /// <summary>
    /// A class that derives from <see cref="Adorner"/>. 
    /// This class represents magnetic point adorner element.
    /// </summary>
    public class MagneticPointsAdorner : Adorner
    {
        private IEnumerable<Point> _points;
        private Brush _fillBrush;

        private bool _isVisible = true;

        /// <summary>
        /// Gets or sets visible parameter.
        /// </summary>
        public new bool IsVisible
        {
            get => _isVisible;
            set
            {
                if (_isVisible != value)
                {
                    _isVisible = value;
                    InvalidateVisual();
                }
            }
        }

        /// <summary>
        /// Initializes magnetic points adorner.
        /// </summary>
        /// <param name="adornedElement"></param>
        /// <param name="points"></param>
        public MagneticPointsAdorner(UIElement adornedElement, IEnumerable<Point> points)
            : base(adornedElement)
        {
            _points = points;
            IsHitTestVisible = false;

            if (TryFindResource("PointBrush") is Brush brush)
                _fillBrush = brush;
            else
                _fillBrush = Brushes.Red;
        }

        /// <inheritdoc/>
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            if (!IsVisible)
                return; 

            if (_points == null)
                return;

            double maxSide = Math.Max(AdornedElement.RenderSize.Width, AdornedElement.RenderSize.Height);
            double radius = maxSide / 25.0;

            var pen = new Pen(_fillBrush, 1);

            foreach (var point in _points)
            {
                drawingContext.DrawEllipse(_fillBrush, pen, point, radius, radius);
            }
        }
    }
}
