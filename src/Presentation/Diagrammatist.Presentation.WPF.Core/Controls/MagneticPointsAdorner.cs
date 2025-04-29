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
        /// <summary>
        /// A fill brush dependency property.
        /// </summary>
        public static readonly DependencyProperty FillBrushProperty =
            DependencyProperty.Register(
                nameof(FillBrush),
                typeof(Brush),
                typeof(MagneticPointsAdorner),
                new FrameworkPropertyMetadata(Brushes.Red, FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>
        /// Gets or sets fill brush.
        /// </summary>
        public Brush FillBrush
        {
            get => (Brush)GetValue(FillBrushProperty);
            set => SetValue(FillBrushProperty, value);
        }

        private IEnumerable<Point> _points;

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

            SetResourceReference(FillBrushProperty, "PointBrush");
        }

        /// <summary>
        /// Updates points.
        /// </summary>
        /// <param name="newPoints"></param>
        public void UpdatePoints(IEnumerable<Point> newPoints)
        {
            _points = newPoints;
            InvalidateVisual(); 
        }

        /// <inheritdoc/>
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            if (!IsVisible || _points is null)
                return; 

            double maxSide = Math.Max(AdornedElement.RenderSize.Width, AdornedElement.RenderSize.Height);
            double radius = maxSide / 25.0;

            var pen = new Pen(FillBrush, 1);

            foreach (var point in _points)
            {
                drawingContext.DrawEllipse(FillBrush, pen, point, radius, radius);
            }
        }
    }
}
