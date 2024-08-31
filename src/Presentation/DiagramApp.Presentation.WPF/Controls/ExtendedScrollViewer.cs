using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace DiagramApp.Presentation.WPF.Controls
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

        public static readonly DependencyProperty MinZoomProperty =
        DependencyProperty.Register(nameof(MinZoom), typeof(float), typeof(ExtendedScrollViewer), new PropertyMetadata(0.1f));

        public static readonly DependencyProperty MaxZoomProperty =
            DependencyProperty.Register(nameof(MaxZoom), typeof(float), typeof(ExtendedScrollViewer), new PropertyMetadata(10f));

        public static readonly DependencyProperty ZoomModifierProperty =
            DependencyProperty.Register(nameof(ZoomModifier), typeof(ModifierKeys), typeof(ExtendedCanvas), new PropertyMetadata(ModifierKeys.Control));

        public static readonly DependencyProperty ZoomFactorProperty =
            DependencyProperty.Register(nameof(ZoomFactor), typeof(float), typeof(ExtendedCanvas), new PropertyMetadata(1.1f));

        public static readonly DependencyProperty IsPanEnabledProperty =
            DependencyProperty.Register(nameof(IsPanEnabled), typeof(bool), typeof(ExtendedCanvas), new PropertyMetadata(true));

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
            if(IsPanEnabled && Keyboard.Modifiers == ZoomModifier)
            {
                e.Handled = true;
                var scaleTransform = LayoutTransform as ScaleTransform;
                var mousePos = Mouse.GetPosition(this);

                var scaleFactor = e.Delta > 0 ? ZoomFactor : 1f / ZoomFactor;

                var newOffset = CalculateOffset(mousePos, scaleFactor);

                ScrollToHorizontalOffset(newOffset.X);
                ScrollToVerticalOffset(newOffset.Y);

                var newScale = scaleTransform!.ScaleX * scaleFactor;
                if (newScale < MinZoom || newScale > MaxZoom)
                    return;

                scaleTransform!.ScaleX *= scaleFactor;
                scaleTransform!.ScaleY *= scaleFactor;
            }
        }

        private Point CalculateOffset(Point mousePos, float scaleFactor)
        {
            return new Point(
                mousePos.X + HorizontalOffset - mousePos.X / scaleFactor,
                mousePos.Y + VerticalOffset - mousePos.Y / scaleFactor);
        }
    }
}
