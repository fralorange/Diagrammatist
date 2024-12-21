using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Diagrammatist.Presentation.WPF.Core.Controls
{
    /// <summary>
    /// A class that derives from <see cref="ScrollViewer"/> and represents extended scroll viewer control.
    /// </summary>
    /// <remarks>
    /// Features:<br/>-Customizable zooming;<br/>-Customizable panning.
    /// </remarks>
    public class ExtendedScrollViewer : ScrollViewer
    {
        private Point initialMousePos;
        private bool isDragging;

        public static readonly DependencyProperty ZoomProperty =
            DependencyProperty.Register(nameof(Zoom), typeof(float), typeof(ExtendedScrollViewer), new PropertyMetadata(1f, OnZoomChanged));

        public static readonly DependencyProperty MinZoomProperty =
            DependencyProperty.Register(nameof(MinZoom), typeof(float), typeof(ExtendedScrollViewer), new PropertyMetadata(0.1f));

        public static readonly DependencyProperty MaxZoomProperty =
            DependencyProperty.Register(nameof(MaxZoom), typeof(float), typeof(ExtendedScrollViewer), new PropertyMetadata(10f));

        public static readonly DependencyProperty ZoomModifierProperty =
            DependencyProperty.Register(nameof(ZoomModifier), typeof(ModifierKeys), typeof(ExtendedScrollViewer), new PropertyMetadata(ModifierKeys.Control));

        public static readonly DependencyProperty ZoomFactorProperty =
            DependencyProperty.Register(nameof(ZoomFactor), typeof(float), typeof(ExtendedScrollViewer), new PropertyMetadata(1.1f));

        public static readonly DependencyProperty IsPanEnabledProperty =
            DependencyProperty.Register(nameof(IsPanEnabled), typeof(bool), typeof(ExtendedScrollViewer), new PropertyMetadata(true));

        /// <summary>
        /// Gets or sets the current zoom.
        /// </summary>
        public float Zoom
        {
            get { return (float)GetValue(ZoomProperty); }
            set { SetValue(ZoomProperty, value); }
        }

        /// <summary>
        /// Gets or sets the minimal zoom.
        /// </summary>
        public float MinZoom
        {
            get { return (float)GetValue(MinZoomProperty); }
            set { SetValue(MinZoomProperty, value); }
        }

        /// <summary>
        /// Gets or sets the maxium zoom.
        /// </summary>
        public float MaxZoom
        {
            get { return (float)GetValue(MaxZoomProperty); }
            set { SetValue(MaxZoomProperty, value); }
        }

        /// <summary>
        /// Gets or sets the zoom modifier key.
        /// </summary>
        public ModifierKeys ZoomModifier
        {
            get { return (ModifierKeys)GetValue(ZoomModifierProperty); }
            set { SetValue(ZoomModifierProperty, value); }
        }

        /// <summary>
        /// Gets or sets the zoom factor.
        /// </summary>
        public float ZoomFactor
        {
            get { return (float)GetValue(ZoomFactorProperty); }
            set { SetValue(ZoomFactorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the pan enabled parameter.
        /// </summary>
        public bool IsPanEnabled
        {
            get { return (bool)GetValue(IsPanEnabledProperty); }
            set { SetValue(IsPanEnabledProperty, value); }
        }

        public ExtendedScrollViewer()
        {
            PreviewMouseLeftButtonDown += ExtendedScrollViewer_PreviewMouseLeftButtonDown;
            MouseMove += ExtendedScrollViewer_MouseMove;
            MouseLeftButtonUp += ExtendedScrollViewer_MouseLeftButtonUp;
            PreviewMouseWheel += ExtendedScrollViewer_MouseWheel;

            LayoutTransform = new ScaleTransform();
        }

        /// <summary>
        /// Zooms content in.
        /// </summary>
        /// <remarks>
        /// This method used to zoom in current content relatively to center without mouse position.
        /// </remarks>
        public void ZoomIn()
        {
            CenterContentRelatively(ZoomFactor);

            Zoom = CalculateZoom(ZoomFactor);
        }

        /// <summary>
        /// Zooms content out.
        /// </summary>
        /// <remarks>
        /// This method used to zoom out current content relatively to center without mouse position.
        /// </remarks>
        public void ZoomOut()
        {
            CenterContentRelatively(1f / ZoomFactor);

            Zoom = CalculateZoom(1f / ZoomFactor);
        }

        /// <summary>
        /// Resets current content scale.
        /// </summary>
        /// <remarks>
        /// This method used to reset and center current content.
        /// </remarks>
        public void ZoomReset()
        {
            Zoom = 1f;

            Dispatcher.BeginInvoke(CenterContent, System.Windows.Threading.DispatcherPriority.Render);
        }

        private void ExtendedScrollViewer_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (IsPanEnabled && e.OriginalSource is not Rectangle { TemplatedParent: Thumb })
            {
                initialMousePos = e.GetPosition(this);
                isDragging = true;

                CaptureMouse();
            }
        }

        private void ExtendedScrollViewer_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                var currentMousePos = e.GetPosition(this);

                var deltaX = currentMousePos.X - initialMousePos.X;
                var deltaY = currentMousePos.Y - initialMousePos.Y;

                ScrollToHorizontalOffset(HorizontalOffset - deltaX);
                ScrollToVerticalOffset(VerticalOffset - deltaY);

                initialMousePos = currentMousePos;
            }
        }

        private void ExtendedScrollViewer_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (isDragging)
            {
                isDragging = false;

                ReleaseMouseCapture();
            }
        }

        private void ExtendedScrollViewer_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (IsPanEnabled && Keyboard.Modifiers == ZoomModifier)
            {
                e.Handled = true;

                var mousePos = Mouse.GetPosition(this);

                var scaleFactor = e.Delta > 0 ? ZoomFactor : 1f / ZoomFactor;
                var newOffset = CalculateOffset(mousePos, scaleFactor);

                ScrollToHorizontalOffset(newOffset.X);
                ScrollToVerticalOffset(newOffset.Y);

                Zoom = CalculateZoom(scaleFactor);
            }
        }

        private Point CalculateOffset(Point pos, float scaleFactor)
        {
            return new Point(
                pos.X + HorizontalOffset - pos.X / scaleFactor,
                pos.Y + VerticalOffset - pos.Y / scaleFactor);
        }

        private void CenterContent()
        {
            ScrollToHorizontalOffset(ScrollableWidth / 2);
            ScrollToVerticalOffset(ScrollableHeight / 2);
        }

        private void CenterContentRelatively(float factor)
        {
            var viewportCenter = new Point(ViewportWidth / 2, ViewportHeight / 2);

            var newOffset = CalculateOffset(viewportCenter, factor);

            ScrollToHorizontalOffset(newOffset.X);
            ScrollToVerticalOffset(newOffset.Y);
        }

        private float CalculateZoom(float factor)
        {
            var newZoom = Zoom * factor;
            return newZoom < MinZoom ? MinZoom : newZoom > MaxZoom ? MaxZoom : newZoom;
        }

        private void ApplyZoom(float zoom)
        {
            var scaleTransform = LayoutTransform as ScaleTransform;
            scaleTransform!.ScaleX = zoom;
            scaleTransform!.ScaleY = zoom;
        }

        private static void OnZoomChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var extendedScrollViewer = (ExtendedScrollViewer)d;
            extendedScrollViewer.ApplyZoom((float)e.NewValue);
        }
    }
}
