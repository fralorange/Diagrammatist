using DiagramApp.Application.AppServices.Helpers;
using DiagramApp.Client.Extensions.UIElement;
using DiagramApp.Client.ViewModels;
using DiagramApp.Client.ViewModels.Wrappers;
using DiagramApp.Domain.Canvas;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.UI.Xaml;
using System;
using Point = Microsoft.Maui.Graphics.Point;

namespace DiagramApp.Client.Components;

public partial class EditorView : Frame
{
    //refactor those attributes, maybe combine something, maybe replace something
    private readonly List<System.Drawing.Point> _tappedPoints = [];
    private readonly List<View> _clickedObjects = [];

    private Point? pointerPos;

    private double _horizontalScrollPosition;
    private double _verticalScrollPosition;

    private Point _deltaPosition;

    private (double X, double Y) initialElemPos;
    private (double X, double Y) clampedElemPos;

    public EditorView()
    {
        InitializeComponent();
    }

    private async void OnPanCanvasUpdated(object sender, PanUpdatedEventArgs e)
    {
        if (BindingContext is MainViewModel { CurrentCanvas.Controls: ControlsType.Drag })
        {
            if (e.StatusType == GestureStatus.Started)
            {
                _horizontalScrollPosition = CanvasScrollWindow.ScrollX;
                _verticalScrollPosition = CanvasScrollWindow.ScrollY;
            }
            else if (e.StatusType == GestureStatus.Running)
            {
                _deltaPosition = new Point(e.TotalX, e.TotalY);
                await CanvasScrollWindow.ScrollToAsync(_horizontalScrollPosition - _deltaPosition.X, _verticalScrollPosition - _deltaPosition.Y, true);
            }
            else if (e.StatusType == GestureStatus.Completed || e.StatusType == GestureStatus.Canceled)
            {
                _deltaPosition = Point.Zero;
            }
        }
    }

    private void OnPanElementUpdated(object sender, PanUpdatedEventArgs e)
    {
        if (sender is View { Parent: AbsoluteLayout layout } view && BindingContext is MainViewModel { CurrentCanvas.Controls: ControlsType.Select, CurrentCanvas.IsNotBlocked: true } viewModel)
        {
            if (e.StatusType == GestureStatus.Started)
            {
                initialElemPos.X = view.TranslationX;
                initialElemPos.Y = view.TranslationY;

                var figure = (ObservableFigure)view.BindingContext;
                viewModel.SelectItemInCanvasCommand.Execute(figure);
            }
            else if (e.StatusType == GestureStatus.Running && pointerPos is not null)
            {
                var newX = pointerPos.Value.X - view.Width / 2;
                var newY = pointerPos.Value.Y - view.Height / 2;

                clampedElemPos.X = Math.Max(0, Math.Min(newX, layout.Width - view.Width));
                clampedElemPos.Y = Math.Max(0, Math.Min(newY, layout.Height - view.Height));

                view.TranslationX = clampedElemPos.X;
                view.TranslationY = clampedElemPos.Y;
            }
            else if (e.StatusType == GestureStatus.Completed)
            {
                var (initialX, initialY, clampedX, clampedY) = (initialElemPos.X, initialElemPos.Y, clampedElemPos.X, clampedElemPos.Y);

                var undoAction = new Action(() =>
                {
                    view.TranslationX = initialX;
                    view.TranslationY = initialY;
                });

                var redoAction = new Action(() =>
                {
                    view.TranslationX = clampedX;
                    view.TranslationY = clampedY;
                });

                UndoableCommandHelper.ExecuteAction(viewModel.CurrentCanvas, redoAction, undoAction);
            }
        }
    }

    private void OnPointerEntered(object sender, PointerEventArgs e)
    {
        if (BindingContext is MainViewModel { CurrentCanvas: { } } viewModel)
        {
            if (sender is AbsoluteLayout layout)
            {
#if WINDOWS
                if (layout.Handler?.PlatformView is Microsoft.UI.Xaml.Controls.Panel panel)
                {
                    switch (viewModel.CurrentCanvas!.Controls)
                    {
                        case ControlsType.Drag:
                            panel.ChangeCursor(Microsoft.UI.Input.InputSystemCursor.Create(Microsoft.UI.Input.InputSystemCursorShape.SizeAll));
                            break;
                        case ControlsType.Select when viewModel.CurrentCanvas.IsBlocked:
                            panel.ChangeCursor(Microsoft.UI.Input.InputSystemCursor.Create(Microsoft.UI.Input.InputSystemCursorShape.Hand));
                            break;
                        case ControlsType.Select:
                            panel.ChangeCursor(Microsoft.UI.Input.InputSystemCursor.Create(Microsoft.UI.Input.InputSystemCursorShape.Arrow));
                            break;
                    }
                }
#endif
            }
        }
    }

    private void OnPointerElementEntered(object sender, PointerEventArgs e)
    {
        if (BindingContext is MainViewModel { CurrentCanvas: { } } viewModel)
        {
            View? view = sender as Shape;
            view ??= sender as Border;
#if WINDOWS
            if (view!.Handler?.PlatformView is UIElement panel)
            {
                switch (viewModel.CurrentCanvas!.Controls)
                {
                    case ControlsType.Select when viewModel.CurrentCanvas.IsBlocked:
                        panel.ChangeCursor(Microsoft.UI.Input.InputSystemCursor.Create(Microsoft.UI.Input.InputSystemCursorShape.Hand));
                        break;
                    case ControlsType.Select:
                        panel.ChangeCursor(Microsoft.UI.Input.InputSystemCursor.Create(Microsoft.UI.Input.InputSystemCursorShape.SizeAll));
                        break;
                }
            }
#endif
        }
    }

    private void OnPointerMovedInsideCanvas(object sender, PointerEventArgs e) => pointerPos = e.GetPosition((AbsoluteLayout)sender);

    private void OnPointerExitedFromCanvas(object sender, PointerEventArgs e) => pointerPos = null;

    private void OnTappedInsideCanvas(object sender, TappedEventArgs e)
    {
        if (BindingContext is MainViewModel { CurrentCanvas.SelectedFigure: null, CurrentCanvas.IsBlocked: true } viewModel)
        {
            var canvas = (View)sender;
            var point = e.GetPosition(canvas);
            (int x, int y) = (Convert.ToInt32(point!.Value.X), Convert.ToInt32(point!.Value.Y));

            _tappedPoints!.Add(new System.Drawing.Point(x, y));

            if (_tappedPoints.Count > 1)
            {
                var dashedLine = new Line
                {
                    X1 = _tappedPoints[_tappedPoints.Count - 2].X,
                    Y1 = _tappedPoints[_tappedPoints.Count - 2].Y,
                    X2 = _tappedPoints[_tappedPoints.Count - 1].X,
                    Y2 = _tappedPoints[_tappedPoints.Count - 1].Y,
                    StrokeThickness = 2,
                    StrokeDashArray = new double[] { 4, 2 }
                };

                (canvas as AbsoluteLayout)!.Children.Add(dashedLine);

                _clickedObjects.Add(dashedLine);
            }
        }
    }

    private void OnConfirmButtonClicked(object sender, EventArgs e)
    {
        if (BindingContext is MainViewModel { CurrentCanvas.IsBlocked: true } viewModel)
        {
            viewModel.CurrentCanvas.OnBlockedResourcesReceived(_tappedPoints);
            ClearTappedObjects();
        }
    }

    private void OnCancelButtonClicked(object sender, EventArgs e)
    {
        if (BindingContext is MainViewModel { CurrentCanvas.IsBlocked: true } viewModel)
        {
            viewModel.CurrentCanvas.OnBlockedResourcesReceived(null);
            ClearTappedObjects();
        }
    }

    private void ClearTappedObjects()
    {
        foreach (var item in _clickedObjects)
        {
            CanvasContent.Children.Remove(item);
        }

        _tappedPoints.Clear();
        _clickedObjects.Clear();
    }
}