using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Diagrammatist.Presentation.WPF.Core.Controls
{
    /// <summary>
    /// A custom scroll viewer control that supports zooming and panning functionality.
    /// </summary>
    public partial class ExtendedScrollViewer : UserControl
    {
        /// <summary>
        /// Dependency property for the zoom level.
        /// </summary>
        public static readonly DependencyProperty ZoomProperty =
            DependencyProperty.Register(nameof(Zoom), typeof(float), typeof(ExtendedScrollViewer),
                new PropertyMetadata(1f, OnZoomChanged));
        /// <summary>
        /// Dependency property for the minimum zoom level.
        /// </summary>
        public static readonly DependencyProperty MinZoomProperty =
            DependencyProperty.Register(nameof(MinZoom), typeof(float), typeof(ExtendedScrollViewer),
                new PropertyMetadata(0.1f));
        /// <summary>
        /// Dependency property for the maximum zoom level.
        /// </summary>
        public static readonly DependencyProperty MaxZoomProperty =
            DependencyProperty.Register(nameof(MaxZoom), typeof(float), typeof(ExtendedScrollViewer),
                new PropertyMetadata(10f));
        /// <summary>
        /// Dependency property for the zoom modifier key.
        /// </summary>
        public static readonly DependencyProperty ZoomModifierProperty =
            DependencyProperty.Register(nameof(ZoomModifier), typeof(ModifierKeys), typeof(ExtendedScrollViewer),
                new PropertyMetadata(ModifierKeys.Control));
        /// <summary>
        /// Dependency property for the zoom factor.
        /// </summary>
        public static readonly DependencyProperty ZoomFactorProperty =
            DependencyProperty.Register(nameof(ZoomFactor), typeof(float), typeof(ExtendedScrollViewer),
                new PropertyMetadata(1.1f));
        /// <summary>
        /// Dependency property for enabling/disabling panning.
        /// </summary>
        public static readonly DependencyProperty IsPanEnabledProperty =
            DependencyProperty.Register(nameof(IsPanEnabled), typeof(bool), typeof(ExtendedScrollViewer),
                new PropertyMetadata(true));
        /// <summary>
        /// Dependency property for the horizontal offset.
        /// </summary>
        public static readonly DependencyProperty HorizontalOffsetProperty =
            DependencyProperty.Register(nameof(HorizontalOffset), typeof(double), typeof(ExtendedScrollViewer),
                new PropertyMetadata(0.0, OnHorizontalOffsetChanged));
        /// <summary>
        /// Dependency property for the vertical offset.
        /// </summary>
        public static readonly DependencyProperty VerticalOffsetProperty =
            DependencyProperty.Register(nameof(VerticalOffset), typeof(double), typeof(ExtendedScrollViewer),
                new PropertyMetadata(0.0, OnVerticalOffsetChanged));

        /// <summary>
        /// Dependency property for the horizontal scroll bar visibility.
        /// </summary>
        public static readonly DependencyProperty HorizontalScrollBarVisibilityProperty =
            DependencyProperty.Register(nameof(HorizontalScrollBarVisibility), typeof(ScrollBarVisibility), typeof(ExtendedScrollViewer),
                new PropertyMetadata(ScrollBarVisibility.Visible));

        /// <summary>
        /// Dependency property for the vertical scroll bar visibility.
        /// </summary>
        public static readonly DependencyProperty VerticalScrollBarVisibilityProperty =
            DependencyProperty.Register(nameof(VerticalScrollBarVisibility), typeof(ScrollBarVisibility), typeof(ExtendedScrollViewer),
                new PropertyMetadata(ScrollBarVisibility.Visible));

        /// <summary>
        /// Dependency property for the content background brush.
        /// </summary>
        public static readonly DependencyProperty ContentBackgroundProperty =
            DependencyProperty.Register(nameof(ContentBackground), typeof(Brush), typeof(ExtendedScrollViewer), 
                new PropertyMetadata(Brushes.Transparent));

        /// <summary>
        /// Gets or sets the zoom level of the scroll viewer.
        /// </summary>
        public float Zoom
        {
            get => (float)GetValue(ZoomProperty);
            set => SetValue(ZoomProperty, value);
        }

        /// <summary>
        /// Gets or sets the minimum zoom level of the scroll viewer.
        /// </summary>
        public float MinZoom
        {
            get => (float)GetValue(MinZoomProperty);
            set => SetValue(MinZoomProperty, value);
        }

        /// <summary>
        /// Gets or sets the maximum zoom level of the scroll viewer.
        /// </summary>
        public float MaxZoom
        {
            get => (float)GetValue(MaxZoomProperty);
            set => SetValue(MaxZoomProperty, value);
        }

        /// <summary>
        /// Gets or sets the modifier key used for zooming.
        /// </summary>
        public ModifierKeys ZoomModifier
        {
            get => (ModifierKeys)GetValue(ZoomModifierProperty);
            set => SetValue(ZoomModifierProperty, value);
        }

        /// <summary>
        /// Gets or sets the zoom factor for zooming in and out.
        /// </summary>
        public float ZoomFactor
        {
            get => (float)GetValue(ZoomFactorProperty);
            set => SetValue(ZoomFactorProperty, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether panning is enabled.
        /// </summary>
        public bool IsPanEnabled
        {
            get => (bool)GetValue(IsPanEnabledProperty);
            set => SetValue(IsPanEnabledProperty, value);
        }

        /// <summary>
        /// Gets or sets the horizontal offset of the scroll viewer.
        /// </summary>
        public double HorizontalOffset
        {
            get => (double)GetValue(HorizontalOffsetProperty);
            set => SetValue(HorizontalOffsetProperty, value);
        }

        /// <summary>
        /// Gets or sets the vertical offset of the scroll viewer.
        /// </summary>
        public double VerticalOffset
        {
            get => (double)GetValue(VerticalOffsetProperty);
            set => SetValue(VerticalOffsetProperty, value);
        }

        /// <summary>
        /// Gets or sets the visibility of the horizontal scroll bar.
        /// </summary>
        public ScrollBarVisibility HorizontalScrollBarVisibility
        {
            get => (ScrollBarVisibility)GetValue(HorizontalScrollBarVisibilityProperty);
            set => SetValue(HorizontalScrollBarVisibilityProperty, value);
        }

        /// <summary>
        /// Gets or sets the visibility of the vertical scroll bar.
        /// </summary>
        public ScrollBarVisibility VerticalScrollBarVisibility
        {
            get => (ScrollBarVisibility)GetValue(VerticalScrollBarVisibilityProperty);
            set => SetValue(VerticalScrollBarVisibilityProperty, value);
        }

        /// <summary>
        /// Gets or sets the background brush of the content area.
        /// </summary>
        public Brush ContentBackground
        {
            get => (Brush)GetValue(ContentBackgroundProperty);
            set => SetValue(ContentBackgroundProperty, value);
        }

        private ScrollViewer _sv;
        private Grid _grid;
        private ScaleTransform _scale;
        private Point _initialMousePos;
        private bool _isDragging;
        private Point? _lastMousePosOnTarget;
        private Point? _lastCenterPosOnTarget;

        // Flags to avoid feedback loops
        private bool _settingOffsetFromDP;
        private bool _settingDPFromScroll;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtendedScrollViewer"/> class.
        /// </summary>
#pragma warning disable CS8618
        public ExtendedScrollViewer() { InitializeComponent(); }
#pragma warning restore CS8618

        /// <summary>
        /// Zooms in the content of the scroll viewer.
        /// </summary>
        public void ZoomIn()  
        {
            ChangeZoom(ZoomFactor);
        }
        /// <summary>
        /// Zooms out the content of the scroll viewer.
        /// </summary>
        public void ZoomOut() 
        {
            ChangeZoom(1f / ZoomFactor);
        }
        /// <summary>
        /// Resets the zoom level to the default value (1.0).
        /// </summary>
        public void ZoomReset()
        {
            Zoom = 1f;
            Dispatcher.BeginInvoke(CenterContent, System.Windows.Threading.DispatcherPriority.Render);
        }

        private void OnScrollViewerLoaded(object sender, RoutedEventArgs e)
        {
            _sv    = (ScrollViewer)Template.FindName("scrollViewer", this);
            _grid  = (Grid)Template.FindName("grid", this);
            _scale = (ScaleTransform)Template.FindName("scaleTransform", this);

            _sv.PreviewMouseLeftButtonDown += OnPreviewMouseLeftButtonDown;
            _sv.MouseMove                  += OnMouseMove;
            _sv.MouseLeftButtonUp          += OnMouseLeftButtonUp;
            _sv.PreviewMouseLeftButtonUp   += OnMouseLeftButtonUp;
            _sv.PreviewMouseWheel          += OnMouseWheel;
            _sv.ScrollChanged              += OnScrollChanged;
            _sv.SizeChanged                += OnSizeChanged;

            _sv.ScrollChanged += (s, ev) =>
            {
                if (_settingOffsetFromDP) return;
                _settingDPFromScroll = true;
                HorizontalOffset = _sv.HorizontalOffset;
                VerticalOffset   = _sv.VerticalOffset;
                _settingDPFromScroll = false;
            };
        }

        private void OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!IsPanEnabled)
                return;

            _initialMousePos = e.GetPosition(_sv);
            _isDragging = true;
            _sv.CaptureMouse();
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (!_isDragging) return;
            var current = e.GetPosition(_sv);
            var dx = current.X - _initialMousePos.X;
            var dy = current.Y - _initialMousePos.Y;
            _sv.ScrollToHorizontalOffset(_sv.HorizontalOffset - dx);
            _sv.ScrollToVerticalOffset(_sv.VerticalOffset - dy);
            _initialMousePos = current;
        }

        private void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_isDragging)
            {
                _isDragging = false;
                _sv.ReleaseMouseCapture();
            }
        }

        private void OnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (!IsPanEnabled || Keyboard.Modifiers != ZoomModifier) return;
            e.Handled = true;

            _lastMousePosOnTarget = e.GetPosition(_grid);
            var factor = e.Delta > 0 ? ZoomFactor : 1f / ZoomFactor;
            ChangeZoom(factor);
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.PreviousSize == Size.Empty ||
                e.PreviousSize.Width <= 0 ||
                e.PreviousSize.Height <= 0 ||
                Keyboard.Modifiers == ZoomModifier)
                return;

            double maxScrollX = _sv.ExtentWidth - _sv.ViewportWidth;
            double maxScrollY = _sv.ExtentHeight - _sv.ViewportHeight;

            double horizRatio = maxScrollX > 0
                ? _sv.HorizontalOffset / maxScrollX
                : 0.0;
            double vertRatio = maxScrollY > 0
                ? _sv.VerticalOffset / maxScrollY
                : 0.0;

            _sv.UpdateLayout();

            maxScrollX = _sv.ExtentWidth - _sv.ViewportWidth;
            maxScrollY = _sv.ExtentHeight - _sv.ViewportHeight;

            double newH = maxScrollX > 0
                ? horizRatio * maxScrollX
                : 0.0;
            double newV = maxScrollY > 0
                ? vertRatio * maxScrollY
                : 0.0;

            _sv.ScrollToHorizontalOffset(newH);
            _sv.ScrollToVerticalOffset(newV);
        }

        private void OnScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (e.ExtentWidthChange == 0 && e.ExtentHeightChange == 0) return;

            Point? before = null, now = null;
            if (!_lastMousePosOnTarget.HasValue)
            {
                if (_lastCenterPosOnTarget.HasValue)
                {
                    var centerVp = new Point(_sv.ViewportWidth/2, _sv.ViewportHeight/2);
                    now = _sv.TranslatePoint(centerVp, _grid);
                    before = _lastCenterPosOnTarget;
                }
            }
            else
            {
                before = _lastMousePosOnTarget;
                now = Mouse.GetPosition(_grid);
                _lastMousePosOnTarget = null;
            }

            if (!before.HasValue || !now.HasValue) return;

            var dx = now.Value.X - before.Value.X;
            var dy = now.Value.Y - before.Value.Y;
            var mulX = e.ExtentWidth  / _grid.ActualWidth;
            var mulY = e.ExtentHeight / _grid.ActualHeight;

            _sv.ScrollToHorizontalOffset(_sv.HorizontalOffset - dx * mulX);
            _sv.ScrollToVerticalOffset(_sv.VerticalOffset   - dy * mulY);
        }

        private void ChangeZoom(float factor)
        {
            var centerVp = new Point(_sv.ViewportWidth/2, _sv.ViewportHeight/2);
            _lastCenterPosOnTarget = _sv.TranslatePoint(centerVp, _grid);

            Zoom = CalculateZoom(factor);
        }

        private void CenterContent()
        {
            _sv.ScrollToHorizontalOffset(_sv.ScrollableWidth/2);
            _sv.ScrollToVerticalOffset(_sv.ScrollableHeight/2);
        }

        private float CalculateZoom(float factor)
        {
            var newZ = Zoom * factor;
            return newZ < MinZoom ? MinZoom : (newZ > MaxZoom ? MaxZoom : newZ);
        }

        private static void OnZoomChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctl = (ExtendedScrollViewer)d;
            if (ctl._scale != null)
            {
                var z = (float)e.NewValue;
                ctl._scale.ScaleX = z;
                ctl._scale.ScaleY = z;
            }
        }

        private static void OnHorizontalOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctl = (ExtendedScrollViewer)d;
            if (ctl._sv == null || ctl._settingDPFromScroll) return;
            ctl._settingOffsetFromDP = true;
            ctl._sv.ScrollToHorizontalOffset((double)e.NewValue);
            ctl._settingOffsetFromDP = false;
        }

        private static void OnVerticalOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctl = (ExtendedScrollViewer)d;
            if (ctl._sv == null || ctl._settingDPFromScroll) return;
            ctl._settingOffsetFromDP = true;
            ctl._sv.ScrollToVerticalOffset((double)e.NewValue);
            ctl._settingOffsetFromDP = false;
        }
    }
}
