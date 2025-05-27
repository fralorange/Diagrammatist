using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

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

        public static readonly DependencyProperty HasArrowProperty = DependencyProperty.Register(
            nameof(HasArrow),
            typeof(bool),
            typeof(ObservablePolyline),
            new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsRender, OnHasArrowChanged));

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
        /// Gets or sets has arrow parameter.
        /// </summary>
        public bool HasArrow
        {
            get => (bool)GetValue(HasArrowProperty);
            set => SetValue(HasArrowProperty, value);
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
        /// Static constructor to override metadata for StrokeThickness property.
        /// </summary>
        static ObservablePolyline()
        {
            StrokeThicknessProperty.OverrideMetadata(
                typeof(ObservablePolyline),
                new FrameworkPropertyMetadata(
                    2.0,
                    FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure,
                    OnAppearanceChanged));
        }

        /// <inheritdoc/>
        protected override Geometry DefiningGeometry => _polylineGeometry;

        /// <inheritdoc/>
        protected override Size MeasureOverride(Size constraint)
        {
            if (Points == null || Points.Count == 0)
                return Size.Empty;

            var bounds = GetPolylineBounds();
            return new Size(bounds.Width, bounds.Height);
        }

        /// <summary>
        /// Updates the polyline geometry based on the current points.
        /// </summary>
        protected void UpdatePolyline()
        {
            if (Points == null || Points.Count < 2)
            {
                return;
            }

            var minX = Points.Min(p => p.X);
            var minY = Points.Min(p => p.Y);
            PosX = minX;
            PosY = minY;

            var pts = Points.Select(p => new Point(p.X - minX, p.Y - minY)).ToList();

            var geometry = new StreamGeometry { FillRule = FillRule };
            using (var ctx = geometry.Open())
            {
                ctx.BeginFigure(pts[0], isFilled: false, isClosed: false);
                ctx.PolyLineTo(pts.Skip(1).ToList(), isStroked: true, isSmoothJoin: true);

                if (HasArrow)
                {
                    var from = pts[^2];
                    var to = pts[^1];
                    var dir = to - from; 
                    dir.Normalize();
                    var perp = new Vector(-dir.Y, dir.X);
                    var size = StrokeThickness * 5;

                    var tip = to;
                    var b1 = to - dir * size + perp * (size / 2);
                    var b2 = to - dir * size - perp * (size / 2);

                    ctx.BeginFigure(tip, isFilled: true, isClosed: true);
                    ctx.LineTo(b1, isStroked: true, isSmoothJoin: false);
                    ctx.LineTo(b2, isStroked: true, isSmoothJoin: false);
                }
            }

            geometry.Freeze();
            _polylineGeometry = geometry;

            InvalidateVisual();
        }

        private Rect GetPolylineBounds()
        {
            if (Points == null || Points.Count == 0)
                return Rect.Empty;

            var minX = Points.Min(p => p.X);
            var minY = Points.Min(p => p.Y);
            var maxX = Points.Max(p => p.X);
            var maxY = Points.Max(p => p.Y);

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

        private static void OnHasArrowChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ObservablePolyline)d).UpdatePolyline();
        }

        private static void OnAppearanceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var poly = (ObservablePolyline)d;
            poly.UpdatePolyline();
            poly.InvalidateMeasure();
        }
    }
}
