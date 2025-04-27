using Diagrammatist.Presentation.WPF.Core.Controls.Args;
using Diagrammatist.Presentation.WPF.Core.Foundation.Extensions;
using Diagrammatist.Presentation.WPF.Core.Helpers;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Diagrammatist.Presentation.WPF.Core.Controls
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
        private Point? _initialElementPos;
        private Cursor? _previousCursor;

        /// <summary>
        /// Occurs when any element position changes.
        /// </summary>
        public event EventHandler<PositionChangedEventArgs>? ItemPositionChanged;

        public static readonly DependencyProperty IsGridVisibleProperty =
            DependencyProperty.Register(nameof(IsGridVisible), typeof(bool), typeof(ExtendedCanvas), new PropertyMetadata(true, OnGridChange));

        public static readonly DependencyProperty GridStepProperty =
            DependencyProperty.Register(nameof(GridStep), typeof(int), typeof(ExtendedCanvas), new PropertyMetadata(10, OnGridChange));

        public static readonly DependencyProperty GridLineColorProperty =
            DependencyProperty.Register(nameof(GridLineColor), typeof(Brush), typeof(ExtendedCanvas), new PropertyMetadata(Brushes.MediumPurple, OnGridChange));

        public static readonly DependencyProperty IsElementPanEnabledProperty =
            DependencyProperty.Register(nameof(IsElementPanEnabled), typeof(bool), typeof(ExtendedCanvas), new PropertyMetadata(true));
        public static readonly DependencyProperty IsBorderVisibleProperty =
            DependencyProperty.Register(nameof(IsBorderVisible), typeof(bool), typeof(ExtendedCanvas), new PropertyMetadata(true, OnBorderChange));


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
        public Brush GridLineColor
        {
            get { return (Brush)GetValue(GridLineColorProperty); }
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

        /// <summary>
        /// Gets or sets the border visible parameter.
        /// </summary>
        public bool IsBorderVisible
        {
            get => (bool)GetValue(IsBorderVisibleProperty);
            set => SetValue(IsBorderVisibleProperty, value);
        }

        public ExtendedCanvas()
        {
            PreviewMouseLeftButtonDown += OnMouseLeftButtonDown;
            MouseMove += OnMouseMove;
            MouseLeftButtonUp += OnMouseLeftButtonUp;
        }

        /// <summary>
        /// Exports current canvas without grid visible to file path.
        /// </summary>
        /// <param name="filePath">Resultng file path.</param>
        public void Export(string filePath)
        {
            var wasGridVisible = IsGridVisible;
            var wasBorderVisible = IsBorderVisible;

            // Hide grid and border
            SetCurrentValue(IsGridVisibleProperty, false);
            SetCurrentValue(IsBorderVisibleProperty, false);

            // Update layout
            UpdateLayout();

            try
            {
                // Move to render point
                VisualOffset = new(0, 0);

                // Create bitmap
                RenderTargetBitmap renderBitmap = new((int)ActualWidth, (int)ActualHeight, 96, 96, PixelFormats.Pbgra32);

                // Create drawing context
                DrawingVisual dv = new();
                using (DrawingContext ctx = dv.RenderOpen())
                {
                    VisualBrush vb = new(this);
                    ctx.DrawRectangle(vb, null, new Rect(new(ActualWidth, ActualHeight)));
                }

                // Render
                renderBitmap.Render(dv);

                // Encode
                PngBitmapEncoder encoder = new();
                encoder.Frames.Add(BitmapFrame.Create(renderBitmap));

                // Save
                using (FileStream fileStream = new(filePath, FileMode.Create))
                {
                    encoder.Save(fileStream);
                }
            }
            finally
            {
                // Return previous grid and border values
                SetCurrentValue(IsGridVisibleProperty, wasGridVisible);
                SetCurrentValue(IsBorderVisibleProperty, wasBorderVisible);
            }
        }
        #region Event invokers
        /// <summary>
        /// Triggers <see cref="ItemPositionChanged"/> event when and object position changes.
        /// </summary>
        /// <param name="oldX">The previous X-coordinate of the object.</param>
        /// <param name="oldY">The previous Y-coordinate of the object.</param>
        /// <param name="newX">The new X-coordinate of the object.</param>
        /// <param name="newY">The new Y-coordinate of the object.</param>
        protected virtual void OnItemPositionChanged(FrameworkElement item, double oldX, double oldY, double newX, double newY)
        {
            ItemPositionChanged?.Invoke(null, new PositionChangedEventArgs(item.DataContext, oldX, oldY, newX, newY));
        }
        #endregion
        #region Mouse event handlers
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

                // Save item initial position
                _initialElementPos = new(GetLeft(item), GetTop(item));

                CaptureMouse();
            }
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (IsElementPanEnabled && IsMouseCaptured)
            {
                double newX = e.GetPosition(this).X;
                double newY = e.GetPosition(this).Y;

                CenterSelectedElement(ref newX, ref newY);

                TrySnapWithDynamicSpacing(_selectedElement!, ref newX, ref newY);

                GridHelper.SnapCoordinatesToGrid(ref newX, ref newY, GridStep);

                ValidateAndSetElementPosition(_selectedElement!, newX, newY);
            }
        }

        private void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_selectedElement is not null && _initialElementPos is not null)
            {
                OnItemPositionChanged(_selectedElement,
                                      _initialElementPos.Value.X,
                                      _initialElementPos.Value.Y,
                                      GetLeft(_selectedElement),
                                      GetTop(_selectedElement));

                _selectedElement = null;
                Cursor = _previousCursor;
                _initialElementPos = null;

                ReleaseMouseCapture();
            }
        }
        #endregion
        #region Element position handlers
        private void ValidateAndSetElementPosition(FrameworkElement element, double? x = null, double? y = null)
        {
            double left = x ?? GetLeft(element);
            double top = y ?? GetTop(element);

            EnsureElementInsideCanvas(ref left, ref top, element);

            SetLeft(element, left);
            SetTop(element, top);
        }

        private void TrySnapWithDynamicSpacing(FrameworkElement movingElement, ref double newX, ref double newY)
        {
            const double snapThreshold = 10; 

            var elements = Children.OfType<FrameworkElement>().Where(e => e != movingElement).ToList();

            foreach (var reference in elements)
            {
                double refLeft = GetLeft(reference);
                double refTop = GetTop(reference);
                double refRight = refLeft + reference.ActualWidth;
                double refBottom = refTop + reference.ActualHeight;

                double movingCenterX = newX + movingElement.ActualWidth / 2;
                double movingCenterY = newY + movingElement.ActualHeight / 2;

                double refCenterX = refLeft + reference.ActualWidth / 2;
                double refCenterY = refTop + reference.ActualHeight / 2;

                // Center by X.
                if (Math.Abs(movingCenterX - refCenterX) < snapThreshold)
                {
                    newX = refLeft + (reference.ActualWidth - movingElement.ActualWidth) / 2;
                }

                // Center by Y.
                if (Math.Abs(movingCenterY - refCenterY) < snapThreshold)
                {
                    newY = refTop + (reference.ActualHeight - movingElement.ActualHeight) / 2;
                }

                // Edge snapping.
                if (Math.Abs(newX - refLeft) < snapThreshold)
                    newX = refLeft;

                if (Math.Abs((newX + movingElement.ActualWidth) - (refLeft + reference.ActualWidth)) < snapThreshold)
                    newX = refLeft + reference.ActualWidth - movingElement.ActualWidth;

                if (Math.Abs(newY - refTop) < snapThreshold)
                    newY = refTop;

                if (Math.Abs((newY + movingElement.ActualHeight) - (refTop + reference.ActualHeight)) < snapThreshold)
                    newY = refTop + reference.ActualHeight - movingElement.ActualHeight;

                // Dynamic distance by Y-axis.
                foreach (var secondReference in elements)
                {
                    if (secondReference == reference) continue;

                    double secondRefTop = GetTop(secondReference);
                    double secondRefBottom = secondRefTop + secondReference.ActualHeight;

                    double expectedDistance = refTop - secondRefBottom;
                    if (expectedDistance > 0 && Math.Abs(newY - (refBottom + expectedDistance)) < snapThreshold)
                    {
                        newY = refBottom + expectedDistance;
                    }
                }

                // Dynamic distance by X-axis.
                foreach (var secondReference in elements)
                {
                    if (secondReference == reference) continue;

                    double secondRefLeft = GetLeft(secondReference);
                    double secondRefRight = secondRefLeft + secondReference.ActualWidth;

                    double expectedDistance = refLeft - secondRefRight;
                    if (expectedDistance > 0 && Math.Abs(newX - (refRight + expectedDistance)) < snapThreshold)
                    {
                        newX = refRight + expectedDistance;
                    }
                }
            }
        }


        private void CenterSelectedElement(ref double x, ref double y)
        {
            x -= _selectedElement!.ActualWidth / 2;
            y -= _selectedElement!.ActualHeight / 2;
        }

        private void EnsureElementInsideCanvas(ref double x, ref double y, FrameworkElement? elem = null)
        {
            elem ??= _selectedElement;

            x = Math.Max(0, Math.Min(x, ActualWidth - elem!.ActualWidth));
            y = Math.Max(0, Math.Min(y, ActualHeight - elem!.ActualHeight));
        }
        #endregion
        #region Event handlers
        private static void OnGridChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ExtendedCanvas canvas)
            {
                canvas.InvalidateVisual();
            }
        }
        private static void OnBorderChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        { 
            if (d is ExtendedCanvas canvas)
            {
                canvas.InvalidateVisual();
            }
        }

        protected override void OnVisualChildrenChanged(DependencyObject visualAdded, DependencyObject visualRemoved)
        {
            base.OnVisualChildrenChanged(visualAdded, visualRemoved);

            if (visualAdded is FrameworkElement elem)
            {
                Dispatcher.BeginInvoke(DispatcherPriority.Render, () =>
                {
                    ValidateAndSetElementPosition(elem);
                });
            }
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);

            if (IsGridVisible)
            {
                var penLight = new Pen(GridLineColor, 0.5);
                var penBold = new Pen(GridLineColor, 1);

                for (int x = 0, cellCount = 0; x < ActualWidth; x += GridStep, cellCount++)
                {
                    dc.DrawLine(cellCount % GridStep == 0 ? penBold : penLight, new Point(x, 0), new Point(x, ActualHeight));
                }

                for (int y = 0, cellCount = 0; y < ActualHeight; y += GridStep, cellCount++)
                {
                    dc.DrawLine(cellCount % GridStep == 0 ? penBold : penLight, new Point(0, y), new Point(ActualWidth, y));
                }
            }

            if (IsBorderVisible)
            {
                var borderPen = new Pen(GridLineColor, 1);
                dc.DrawRectangle(null, borderPen, new Rect(0, 0, ActualWidth, ActualHeight));
            }
        }
        #endregion
    }
}
