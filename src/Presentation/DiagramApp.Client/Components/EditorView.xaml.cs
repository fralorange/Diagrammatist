using DiagramApp.Client.Extensions.UIElement;
using DiagramApp.Client.ViewModels;
using DiagramApp.Client.ViewModels.Wrappers;
using DiagramApp.Domain.Canvas;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Platform;
using Microsoft.UI.Xaml;

namespace DiagramApp.Client.Components;

public partial class EditorView : Frame
{
    private Point? pointerPos;

    private double _horizontalScrollPosition;
    private double _verticalScrollPosition;

    // refactor later to contain this in dynamic resources maybe (clean code)
    private Point _deltaPosition;

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
        if (sender is View { Parent: AbsoluteLayout layout } view && BindingContext is MainViewModel { CurrentCanvas.Controls: ControlsType.Select } viewModel)
        {
            if (e.StatusType == GestureStatus.Started)
            {
                var figure = (ObservableFigure)view.BindingContext;

                viewModel.SelectItemInCanvasCommand.Execute(figure);
            }
            else if (e.StatusType == GestureStatus.Running && pointerPos is not null)
            {
                var newX = pointerPos.Value.X - view.Width / 2;
                var newY = pointerPos.Value.Y - view.Height / 2;

                double clampedX = Math.Max(0, Math.Min(newX, layout.Width - view.Width));
                double clampedY = Math.Max(0, Math.Min(newY, layout.Height - view.Height));

                view.TranslationX = clampedX;
                view.TranslationY = clampedY;
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
                    if (viewModel.CurrentCanvas!.Controls == ControlsType.Drag)
                        panel.ChangeCursor(Microsoft.UI.Input.InputSystemCursor.Create(Microsoft.UI.Input.InputSystemCursorShape.SizeAll));
                    else
                        panel.ChangeCursor(Microsoft.UI.Input.InputSystemCursor.Create(Microsoft.UI.Input.InputSystemCursorShape.Arrow));
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
                if (viewModel.CurrentCanvas!.Controls == ControlsType.Select)
                    panel.ChangeCursor(Microsoft.UI.Input.InputSystemCursor.Create(Microsoft.UI.Input.InputSystemCursorShape.SizeAll));
            }
#endif
        }
    }

    private void OnPointerMovedInsideCanvas(object sender, PointerEventArgs e) => pointerPos = e.GetPosition((AbsoluteLayout)sender);

    private void OnPointerExitedFromCanvas(object sender, PointerEventArgs e) => pointerPos = null;

}