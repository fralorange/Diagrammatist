using DiagramApp.Presentation.WPF.Framework.Extensions.DependencyObject;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace DiagramApp.Presentation.WPF.Framework.Controls
{
    /// <summary>
    /// A class that derives from <see cref="Canvas"/> and represents extended canvas control.
    /// </summary>
    /// <remarks>
    /// Features:<br/>-Visible grid;<br/>-Element panning.
    /// <para>
    /// Works only when extended canvas used as items panel in selector.
    /// </para>
    /// </remarks>
    public class ExtendedCanvas : Canvas
    {
        private FrameworkElement? _selectedElement;
        private Cursor? _previousCursor;

        public static readonly DependencyProperty IsGridVisibleProperty =
            DependencyProperty.Register(nameof(IsGridVisible), typeof(bool), typeof(ExtendedCanvas), new PropertyMetadata(true, OnGridChange));

        public static readonly DependencyProperty GridStepProperty =
            DependencyProperty.Register(nameof(GridStep), typeof(int), typeof(ExtendedCanvas), new PropertyMetadata(10, OnGridChange));

        public static readonly DependencyProperty GridLineColorProperty =
            DependencyProperty.Register(nameof(GridLineColor), typeof(Color), typeof(ExtendedCanvas), new PropertyMetadata(Colors.MediumPurple, OnGridChange));

        public static readonly DependencyProperty IsElementPanEnabledProperty =
            DependencyProperty.Register(nameof(IsElementPanEnabled), typeof(bool), typeof(ExtendedCanvas), new PropertyMetadata(true));

        /// <summary>
        /// Gets or sets the grid visible parameter.
        /// </summary>
        public bool IsGridVisible
        {
            get { return (bool)GetValue(IsGridVisibleProperty); }
            set { SetValue(IsGridVisibleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the grid step.
        /// </summary>
        public int GridStep
        {
            get { return (int)GetValue(GridStepProperty); }
            set { SetValue(GridStepProperty, value); }
        }

        /// <summary>
        /// Gets or sets grid line color.
        /// </summary>
        public Color GridLineColor
        {
            get { return (Color)GetValue(GridLineColorProperty); }
            set { SetValue(GridLineColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the element pan enabled parameter.
        /// </summary>
        public bool IsElementPanEnabled
        {
            get { return (bool)GetValue(IsElementPanEnabledProperty); }
            set { SetValue(IsElementPanEnabledProperty, value); }
        }

        public ExtendedCanvas()
        {
            PreviewMouseLeftButtonDown += OnMouseLeftButtonDown;
            MouseMove += OnMouseMove;
            MouseLeftButtonUp += OnMouseLeftButtonUp;
        }

        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (IsElementPanEnabled && (e.Source as DependencyObject)?.GetVisualAncestor<ListBoxItem>() is ListBoxItem item)
            {
                _selectedElement = item;

                // Select element
                item.IsSelected = true;
                item.Focus();

                // Disable cursor
                _previousCursor = Cursor;
                Cursor = Cursors.None;

                CaptureMouse();
            }
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (IsElementPanEnabled && IsMouseCaptured)
            {
                double newX = e.GetPosition(this).X;
                double newY = e.GetPosition(this).Y;

                var selBorder = GetSelectedElementSelectionBorder();

                Dispatcher.Invoke(() =>
                {
                    CenterSelectedElement(selBorder, ref newX, ref newY);
                    SnapCoordinatesToGrid(selBorder, ref newX, ref newY);
                    EnsureElementInsideCanvas(selBorder, ref newX, ref newY);
                });

                SetLeft(_selectedElement, newX);
                SetTop(_selectedElement, newY);
            }
        }

        private Thickness GetSelectedElementSelectionBorder()
        {
            if (_selectedElement is Control control)
            {
                var pad = control.Padding;
                var border = control.BorderThickness;
                return new Thickness(pad.Left + border.Left, pad.Top + border.Top, pad.Right + border.Top, pad.Bottom + border.Bottom);
            }

            return new Thickness();
        }

        private void CenterSelectedElement(Thickness selBorder, ref double x, ref double y)
        {
            x -= (_selectedElement!.ActualWidth - selBorder.Left - selBorder.Right) / 2;
            y -= (_selectedElement!.ActualHeight - selBorder.Top - selBorder.Bottom) / 2;
        }

        private void SnapCoordinatesToGrid(Thickness selBorder, ref double x, ref double y)
        {
            x = Math.Round(x / GridStep) * GridStep - selBorder.Left;
            y = Math.Round(y / GridStep) * GridStep - selBorder.Top;
        }

        private void EnsureElementInsideCanvas(Thickness selBorder, ref double x, ref double y)
        {
            x = Math.Max(0 - selBorder.Left, Math.Min(x, ActualWidth - _selectedElement!.ActualWidth + selBorder.Right));
            y = Math.Max(0 - selBorder.Top, Math.Min(y, ActualHeight - _selectedElement!.ActualHeight + selBorder.Bottom));
        }

        private void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_selectedElement is not null)
            {
                _selectedElement = null;
                Cursor = _previousCursor;

                ReleaseMouseCapture();
            }
        }

        private static void OnGridChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ExtendedCanvas canvas)
            {
                canvas.InvalidateVisual();
            }
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);

            if (IsGridVisible)
            {
                var brush = new SolidColorBrush(GridLineColor);

                var penLight = new Pen(brush, 0.5);
                var penBold = new Pen(brush, 1);

                for (int x = 0, cellCount = 0; x < ActualWidth; x += GridStep, cellCount++)
                {
                    dc.DrawLine((cellCount % GridStep == 0) ? penBold : penLight, new Point(x, 0), new Point(x, ActualHeight));
                }

                for (int y = 0, cellCount = 0; y < ActualHeight; y += GridStep, cellCount++)
                {
                    dc.DrawLine((cellCount % GridStep == 0) ? penBold : penLight, new Point(0, y), new Point(ActualWidth, y));
                }
            }
        }
    }
}
