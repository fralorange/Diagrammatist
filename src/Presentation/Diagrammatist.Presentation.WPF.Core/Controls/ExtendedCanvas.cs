using Diagrammatist.Presentation.WPF.Core.Controls.Adorners;
using Diagrammatist.Presentation.WPF.Core.Controls.Args;
using Diagrammatist.Presentation.WPF.Core.Foundation.Extensions;
using Diagrammatist.Presentation.WPF.Core.Helpers;
using Diagrammatist.Presentation.WPF.Core.Interactions.Behaviors;
using Diagrammatist.Presentation.WPF.Core.Shared.Enums;
using Diagrammatist.Presentation.WPF.Core.Shared.Records;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
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
        private Point _initialElementPos;
        private Point _lastReportedPos;
        private Cursor? _previousCursor;

        private AlignmentAdorner? _alignmentAdorner;

        /// <summary>
        /// Occurs when any element position changes.
        /// </summary>
        public event EventHandler<PositionChangedEventArgs>? ItemPositionChanged;

        /// <summary>
        /// Occurs while any element is being moved.
        /// </summary>
        public event EventHandler<PositionChangingEventArgs>? ItemPositionChanging;

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

        public static readonly DependencyProperty SnapToGridProperty =
            DependencyProperty.RegisterAttached(nameof(SnapToGrid), typeof(bool), typeof(ExtendedCanvas), new PropertyMetadata(true));

        public static readonly DependencyProperty AltSnapToGridProperty =
            DependencyProperty.RegisterAttached(nameof(AltSnapToGrid), typeof(bool), typeof(ExtendedCanvas), new PropertyMetadata(false));


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

        /// <summary>
        /// Gets or sets the snap to grid parameter.
        /// </summary>
        public bool SnapToGrid
        {
            get { return (bool)GetValue(SnapToGridProperty); }
            set { SetValue(SnapToGridProperty, value); }
        }

        /// <summary>
        /// Gets or sets the alter snap to grid parameter.
        /// </summary>
        public bool AltSnapToGrid
        {
            get { return (bool)GetValue(AltSnapToGridProperty); }
            set { SetValue(AltSnapToGridProperty, value); }
        }

        /// <summary>
        /// Initializes a new instance of <see cref="ExtendedCanvas"/> class.
        /// </summary>
        public ExtendedCanvas()
        {
            PreviewMouseLeftButtonDown += OnMouseLeftButtonDown;
            MouseMove += OnMouseMove;
            MouseLeftButtonUp += OnMouseLeftButtonUp;
        }

        /// <summary>
        /// Exports the current canvas content to a file with the specified settings.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="settings"></param>
        public void Export(string filePath, ExportSettings settings)
        {
            var originalStates = CaptureOriginalStates();
            try
            {
                HideOverlay();
                UpdateLayout();

                var dpiScale = GetDpiScale(settings.Ppi);
                var exportBounds = CalculateExportBounds(settings);
                var bitmap = RenderBitmap(exportBounds, dpiScale);

                SaveBitmap(bitmap, filePath);
            }
            finally
            {
                RestoreOriginalStates(originalStates);
            }
        }

        #region Export handlers

        private (bool GridVisible, bool BorderVisible) CaptureOriginalStates()
        {
            return (IsGridVisible, IsBorderVisible);
        }

        private void HideOverlay()
        {
            SetCurrentValue(IsGridVisibleProperty, false);
            SetCurrentValue(IsBorderVisibleProperty, false);
        }

        private void RestoreOriginalStates((bool GridVisible, bool BorderVisible) originalStates)
        {
            SetCurrentValue(IsGridVisibleProperty, originalStates.GridVisible);
            SetCurrentValue(IsBorderVisibleProperty, originalStates.BorderVisible);
        }

        /// <summary>
        /// Calculates the dpi scale based on standard WPF DPI (96).
        /// </summary>
        /// <param name="ppi"></param>
        /// <returns></returns>
        private double GetDpiScale(int ppi)
        {
            return ppi / 96.0;
        }

        private Rect CalculateExportBounds(ExportSettings settings)
        {
            if (settings.ExportScenario == ExportScenario.Content)
            {
                Rect contentBounds = Rect.Empty;

                foreach (UIElement child in Children)
                {
                    double left = GetLeft(child);
                    double top = GetTop(child);

                    double width = child.RenderSize.Width;
                    double height = child.RenderSize.Height;

                    var bounds = new Rect(left, top, width, height);

                    if (child.RenderTransform is Transform transform)
                    {
                        var transformedBounds = transform.TransformBounds(bounds);
                        bounds = new Rect(
                            transformedBounds.Left,
                            transformedBounds.Top,
                            transformedBounds.Width,
                            transformedBounds.Height
                        );
                    }

                    if (contentBounds == Rect.Empty)
                    {
                        contentBounds = bounds;
                    }
                    else
                    {
                        contentBounds.Union(bounds);
                    }
                }

                contentBounds.Inflate(settings.ContentMargin, settings.ContentMargin);
                return contentBounds;
            }

            return new Rect(0, 0, ActualWidth, ActualHeight);
        }

        private RenderTargetBitmap RenderBitmap(Rect bounds, double scale)
        {
            VisualOffset = new(0, 0);

            int pixelWidth = (int)Math.Round(bounds.Width * scale);
            int pixelHeight = (int)Math.Round(bounds.Height * scale);
            double dpi = 96 * scale;

            var bmp = new RenderTargetBitmap(
                pixelWidth,
                pixelHeight,
                dpi,
                dpi,
                PixelFormats.Pbgra32);

            var dv = new DrawingVisual();
            using (var ctx = dv.RenderOpen())
            {
                if (Background != null)
                {
                    ctx.DrawRectangle(
                        Background.Clone(),
                        null,
                        new Rect(bounds.Size));
                }

                var vb = new VisualBrush(this)
                {
                    Viewbox = bounds,
                    ViewboxUnits = BrushMappingMode.Absolute,
                    Stretch = Stretch.None,
                    AlignmentX = AlignmentX.Left,
                    AlignmentY = AlignmentY.Top
                };

                ctx.DrawRectangle(vb, null, new Rect(bounds.Size));
            }

            bmp.Render(dv);
            return bmp;
        }

        private void SaveBitmap(RenderTargetBitmap bmp, string path)
        {
            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bmp));

            using var stream = new FileStream(path, FileMode.Create);
            encoder.Save(stream);
        }
        #endregion
        #region Event invokers
        /// <summary>
        /// Triggers <see cref="ItemPositionChanged"/> event when and object position changes.
        /// </summary>
        /// <param name="oldX">The previous X-coordinate of the object.</param>
        /// <param name="oldY">The previous Y-coordinate of the object.</param>
        /// <param name="newX">The new X-coordinate of the object.</param>
        /// <param name="newY">The new Y-coordinate of the object.</param>
        protected virtual void OnItemPositionChanged(FrameworkElement item,
                                                     double oldX,
                                                     double oldY,
                                                     double newX,
                                                     double newY,
                                                     double initX,
                                                     double initY)
        {
            ItemPositionChanged?.Invoke(null, new PositionChangedEventArgs(item.DataContext, oldX, oldY, newX, newY, initX, initY));
        }

        protected virtual void OnItemPositionChanging(FrameworkElement item, double oldX, double oldY, double newX, double newY)
        {
            ItemPositionChanging?.Invoke(this, new PositionChangingEventArgs(item.DataContext, oldX, oldY, newX, newY));
        }
        #endregion
        #region Mouse event handlers
        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (IsElementPanEnabled
                && (e.Source as DependencyObject)?.GetVisualAncestor<ListBoxItem>() is ListBoxItem item
                && CanvasMoveableBehavior.GetIsMovable(item))
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
                _lastReportedPos = _initialElementPos;

                CaptureMouse();
            }
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (IsElementPanEnabled && IsMouseCaptured)
            {
                if (!IsElementPanEnabled || !IsMouseCaptured || _selectedElement == null)
                    return;

                double rawX = e.GetPosition(this).X;
                double rawY = e.GetPosition(this).Y;
                CenterSelectedElement(ref rawX, ref rawY);

                TrySnapWithDynamicSpacing(_selectedElement, ref rawX, ref rawY, out var guides);
                bool altPressed = Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightAlt);
                bool shouldSnap = SnapToGrid ^ (AltSnapToGrid && altPressed);
                if (shouldSnap)
                    GridHelper.SnapCoordinatesToGrid(ref rawX, ref rawY, GridStep);

                var oldPos = _lastReportedPos;

                ValidateAndSetElementPosition(_selectedElement, rawX, rawY);

                double actualX = GetLeft(_selectedElement);
                double actualY = GetTop(_selectedElement);

                if (actualX != oldPos.X || actualY != oldPos.Y)
                {
                    OnItemPositionChanging(_selectedElement, oldPos.X, oldPos.Y, actualX, actualY);
                    _lastReportedPos = new Point(actualX, actualY);

                    _alignmentAdorner?.SetGuides(guides);
                }
            }
        }

        private void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_selectedElement is not null)
            {
                OnItemPositionChanged(_selectedElement,
                                      _lastReportedPos.X,
                                      _lastReportedPos.Y,
                                      GetLeft(_selectedElement),
                                      GetTop(_selectedElement),
                                      _initialElementPos.X,
                                      _initialElementPos.Y);

                _selectedElement = null;
                Cursor = _previousCursor;

                ReleaseMouseCapture();
                _alignmentAdorner?.ClearGuides();
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

        private void TrySnapWithDynamicSpacing(FrameworkElement movingElement, ref double newX, ref double newY, out List<Line> guides)
        {
            guides = [];

            const double snapThreshold = 10;

            var elements = Children
                .OfType<FrameworkElement>()
                .Where(e => e != movingElement
                         && CanvasMoveableBehavior.GetIsMovable(e))
                .ToList();

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
                    guides.Add(new Line
                    {
                        X1 = refCenterX,
                        Y1 = 0,
                        X2 = refCenterX,
                        Y2 = ActualHeight,
                    });
                }

                // Center by Y.
                if (Math.Abs(movingCenterY - refCenterY) < snapThreshold)
                {
                    newY = refTop + (reference.ActualHeight - movingElement.ActualHeight) / 2;
                    guides.Add(new Line
                    {
                        X1 = 0,
                        Y1 = refCenterY,
                        X2 = ActualWidth,
                        Y2 = refCenterY,
                    });
                }

                // Edge snapping.
                if (Math.Abs(newX - refLeft) < snapThreshold)
                {
                    newX = refLeft;
                    guides.Add(new Line
                    {
                        X1 = refLeft,
                        Y1 = 0,
                        X2 = refLeft,
                        Y2 = ActualHeight,
                    });
                }

                if (Math.Abs((newX + movingElement.ActualWidth) - (refLeft + reference.ActualWidth)) < snapThreshold)
                {
                    newX = refLeft + reference.ActualWidth - movingElement.ActualWidth;
                    guides.Add(new Line
                    {
                        X1 = refLeft + reference.ActualWidth,
                        Y1 = 0,
                        X2 = refLeft + reference.ActualWidth,
                        Y2 = ActualHeight,
                    });
                }

                if (Math.Abs(newY - refTop) < snapThreshold)
                {
                    newY = refTop;
                    guides.Add(new Line
                    {
                        X1 = 0,
                        Y1 = refTop,
                        X2 = ActualWidth,
                        Y2 = refTop,
                    });
                }

                if (Math.Abs((newY + movingElement.ActualHeight) - (refTop + reference.ActualHeight)) < snapThreshold)
                {
                    newY = refTop + reference.ActualHeight - movingElement.ActualHeight;
                    guides.Add(new Line
                    {
                        X1 = 0,
                        Y1 = refTop + reference.ActualHeight,
                        X2 = ActualWidth,
                        Y2 = refTop + reference.ActualHeight,
                    });
                }

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

                        guides.Add(new Line
                        {
                            X1 = 0,
                            Y1 = refBottom + expectedDistance,
                            X2 = ActualWidth,
                            Y2 = refBottom + expectedDistance,
                        });
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

                        guides.Add(new Line
                        {
                            X1 = refRight + expectedDistance,
                            Y1 = 0,
                            X2 = refRight + expectedDistance,
                            Y2 = ActualHeight,
                        });
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
            if (elem == null) return;

            // Initial rectangle of the element (in its local coordinates)
            var rect = new Rect(0, 0, elem.ActualWidth, elem.ActualHeight);

            // Rotation matrix around the center of the element
            var center = new Point(elem.ActualWidth / 2, elem.ActualHeight / 2);
            var rotate = new RotateTransform
            {
                Angle = (elem.RenderTransform as RotateTransform)?.Angle ?? 0,
                CenterX = center.X,
                CenterY = center.Y
            };

            // Getting axis-oriented (AABB) rectangle after rotation
            var rotatedBounds = rotate.TransformBounds(rect);

            // Calculate the bounds of allowable x,y coordinates (element position is given by its local left-top corner)
            double leftBound = -rotatedBounds.X;
            double topBound = -rotatedBounds.Y;
            double rightBound = ActualWidth - rotatedBounds.Width - rotatedBounds.X;
            double bottomBound = ActualHeight - rotatedBounds.Height - rotatedBounds.Y;

            x = Math.Max(leftBound, Math.Min(x, rightBound));
            y = Math.Max(topBound, Math.Min(y, bottomBound));
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            Loaded += (_, _) =>
            {
                var adornerLayer = AdornerLayer.GetAdornerLayer(this);
                if (adornerLayer is not null)
                {
                    _alignmentAdorner = new AlignmentAdorner(this);
                    adornerLayer.Add(_alignmentAdorner);
                }
            };
        }

        /// <inheritdoc/>
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
