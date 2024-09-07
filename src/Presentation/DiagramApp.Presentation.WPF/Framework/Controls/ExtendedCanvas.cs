using DiagramApp.Presentation.WPF.Framework.Extensions.DependencyObject;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace DiagramApp.Presentation.WPF.Framework.Controls
{
    /// <summary>
    /// A class that derives from <see cref="Canvas"/> and represents extended canvas control.
    /// </summary>
    /// <remarks>
    /// Features:<br/>-Visible grid;<br/>-Element panning.
    /// <para>
    /// Works only when extended canvas used as items panel in items control.
    /// </para>
    /// </remarks>
    public class ExtendedCanvas : Canvas
    {
        private List<Line> _gridLines = [];

        private UIElement? _selectedElement;
        private Point _startPoint;
        private Cursor? _previousCursor;

        public static readonly DependencyProperty IsGridVisibleProperty =
            DependencyProperty.Register(nameof(IsGridVisible), typeof(bool), typeof(ExtendedCanvas), new PropertyMetadata { DefaultValue = true });

        public static readonly DependencyProperty GridStepProperty =
            DependencyProperty.Register(nameof(GridStep), typeof(int), typeof(ExtendedCanvas), new PropertyMetadata { DefaultValue = 10 });

        public static readonly DependencyProperty GridLineColorProperty =
            DependencyProperty.Register(nameof(GridLineColor), typeof(Color), typeof(ExtendedCanvas));

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
            PreviewMouseLeftButtonDown += ExtendedCanvas_MouseLeftButtonDown;
            MouseMove += ExtendedCanvas_MouseMove;
            MouseLeftButtonUp += ExtendedCanvas_MouseLeftButtonUp;
        }

        private void ExtendedCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (IsElementPanEnabled && (e.Source as DependencyObject)?.GetVisualAncestor<ListBoxItem>() is ListBoxItem item)
            {
                _startPoint = e.GetPosition(this);

                _selectedElement = item;

                item.IsSelected = true;
                item.Focus();

                _previousCursor = Cursor;
                Cursor = Cursors.None;

                CaptureMouse();
            }
            else if (e.OriginalSource is ExtendedCanvas && Keyboard.FocusedElement is ListBoxItem focusedItem)
            {
                focusedItem.IsSelected = false;
                Keyboard.ClearFocus();
            }
        }

        private void ExtendedCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsElementPanEnabled && IsMouseCaptured)
            {
                var endPoint = e.GetPosition(this);
                Vector delta = endPoint - _startPoint;

                double newX = GetLeft(_selectedElement) + delta.X;
                double newY = GetTop(_selectedElement) + delta.Y;

                newX = Math.Max(0, Math.Min(newX, ActualWidth - (_selectedElement as FrameworkElement)!.ActualWidth));
                newY = Math.Max(0, Math.Min(newY, ActualHeight - (_selectedElement as FrameworkElement)!.ActualHeight));

                SetLeft(_selectedElement, newX);
                SetTop(_selectedElement, newY);

                _startPoint = endPoint;
            }
        }

        private void ExtendedCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_selectedElement is not null)
            {
                _selectedElement = null;
                Cursor = _previousCursor;

                ReleaseMouseCapture();
            }
        }

        private void DrawGridLines()
        {
            //for (int x = -4000; x <= 4000; x += 100)
            //{
            //    Line verticalLine = new Line
            //    {
            //        Stroke = new SolidColorBrush(Colors.AliceBlue),
            //        X1 = x,
            //        Y1 = -4000,
            //        X2 = x,
            //        Y2 = 4000
            //    };

            //    if (x % 1000 == 0)
            //    {
            //        verticalLine.StrokeThickness = 6;
            //    }
            //    else
            //    {
            //        verticalLine.StrokeThickness = 2;
            //    }

            //    Children.Add(verticalLine);
            //    _gridLines.Add(verticalLine);
            //}

            //for (int y = -4000; y <= 4000; y += 100)
            //{
            //    Line horizontalLine = new Line
            //    {
            //        Stroke = new SolidColorBrush(Colors.AliceBlue),
            //        X1 = -4000,
            //        Y1 = y,
            //        X2 = 4000,
            //        Y2 = y
            //    };

            //    if (y % 1000 == 0)
            //    {
            //        horizontalLine.StrokeThickness = 6;
            //    }
            //    else
            //    {
            //        horizontalLine.StrokeThickness = 2;
            //    }

            //    Children.Add(horizontalLine);
            //    _gridLines.Add(horizontalLine);
            //}
        }
    }
}
