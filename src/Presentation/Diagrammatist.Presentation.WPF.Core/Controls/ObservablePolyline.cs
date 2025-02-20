using System.Collections.Specialized;
using System.Windows.Media;
using System.Windows;
using System.Windows.Shapes;
using System.Collections.ObjectModel;

namespace Diagrammatist.Presentation.WPF.Core.Controls
{
    /// <summary>
    /// A class that derives from <see cref="Shape"/> and defines custom <see cref="Polyline"/> that can process dynamic operations.
    /// </summary>
    public class ObservablePolyline : Shape
    {
        private Geometry _polylineGeometry = Geometry.Empty;

        public static readonly DependencyProperty PointsProperty = DependencyProperty.Register(
            nameof(Points),
            typeof(ObservableCollection<Point>),
            typeof(ObservablePolyline),
            new FrameworkPropertyMetadata(null, PointsChanged));

        public static readonly DependencyProperty FillRuleProperty = DependencyProperty.Register(
            nameof(FillRule),
            typeof(FillRule),
            typeof(ObservablePolyline),
            new FrameworkPropertyMetadata(FillRule.EvenOdd));

        private static readonly DependencyProperty PosXProperty = DependencyProperty.Register(
            nameof(PosX),
            typeof(double),
            typeof(ObservablePolyline),
            new FrameworkPropertyMetadata(0.0));

        private static readonly DependencyProperty PosYProperty = DependencyProperty.Register(
            nameof(PosY),
            typeof(double),
            typeof(ObservablePolyline),
            new FrameworkPropertyMetadata(0.0));

        /// <summary>
        /// Gets or sets the collection of points.
        /// </summary>
        public ObservableCollection<Point> Points
        {
            get => (ObservableCollection<Point>)GetValue(PointsProperty);
            set => SetValue(PointsProperty, value);
        }

        /// <summary>
        /// Gets or sets fill rule.
        /// </summary>
        public FillRule FillRule
        {
            get => (FillRule)GetValue(FillRuleProperty);
            set => SetValue(FillRuleProperty, value);
        }

        /// <summary>
        /// Gets position by x-axis.
        /// </summary>
        public double PosX
        {
            get => (double)GetValue(PosXProperty);
            set => SetValue(PosXProperty, value);
        }

        /// <summary>
        /// Gets position by y-axis.
        /// </summary>
        public double PosY
        {
            get => (double)GetValue(PosYProperty);
            set => SetValue(PosYProperty, value);
        }

        /// <summary>
        /// Gets defining geometry.
        /// </summary>
        protected override Geometry DefiningGeometry => _polylineGeometry;

        protected override Size MeasureOverride(Size constraint)
        {
            if (Points == null || Points.Count == 0)
                return Size.Empty;

            var bounds = GetPolylineBounds();
            return new Size(bounds.Width, bounds.Height);
        }

        /// <summary>
        /// Updates polyline.
        /// </summary>
        protected void UpdatePolyline()
        {
            if (Points == null || Points.Count < 2)
            {
                _polylineGeometry = Geometry.Empty;
                PosX = 0;
                PosY = 0;
            }
            else
            {
                double minX = Points.Min(p => p.X);
                double minY = Points.Min(p => p.Y);

                PosX = minX;
                PosY = minY;

                var relativePoints = Points
                    .Select(p => new Point(p.X - minX, p.Y - minY))
                    .ToList();

                var geometry = new PathGeometry { FillRule = FillRule };
                for (int i = 1; i < relativePoints.Count; i++)
                {
                    var figure = new PathFigure
                    {
                        StartPoint = relativePoints[i - 1],
                        IsClosed = false
                    };
                    figure.Segments.Add(new LineSegment(relativePoints[i], true));
                    geometry.Figures.Add(figure);
                }
                _polylineGeometry = geometry;
            }

            InvalidateVisual();
        }

        private Rect GetPolylineBounds()
        {
            if (Points == null || Points.Count == 0)
                return Rect.Empty;

            double minX = Points.Min(p => p.X);
            double minY = Points.Min(p => p.Y);
            double maxX = Points.Max(p => p.X);
            double maxY = Points.Max(p => p.Y);

            return new Rect(new Point(minX, minY), new Point(maxX, maxY));
        }

        private static void PointsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var polyline = (ObservablePolyline)d;
            if (e.OldValue is ObservableCollection<Point> oldPoints)
                oldPoints.CollectionChanged -= polyline.OnPointsChanged!;
            if (e.NewValue is ObservableCollection<Point> newPoints)
                newPoints.CollectionChanged += polyline.OnPointsChanged!;

            polyline.UpdatePolyline();
        }

        private void OnPointsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdatePolyline();
            InvalidateMeasure();
        }
    }
}
