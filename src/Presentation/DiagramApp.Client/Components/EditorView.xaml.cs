using DiagramApp.Client.Extensions.UIElement;
using DiagramApp.Client.ViewModels;
using DiagramApp.Client.ViewModels.Wrappers;
using DiagramApp.Domain.Canvas;
using Path = Microsoft.Maui.Controls.Shapes.Path;

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
        if (sender is Path { Parent: AbsoluteLayout layout } path && BindingContext is MainViewModel { CurrentCanvas.Controls: ControlsType.Select } viewModel)
        {
            if (e.StatusType == GestureStatus.Started)
            {
                var figure = (ObservableFigure)path.BindingContext;

                viewModel.SelectItemInCanvasCommand.Execute(figure);
            }
            else if (e.StatusType == GestureStatus.Running && pointerPos is not null)
            {
                var newX = pointerPos.Value.X - path.Width / 2;
                var newY = pointerPos.Value.Y - path.Height / 2;

                double clampedX = Math.Max(0, Math.Min(newX, layout.Width - path.Width));
                double clampedY = Math.Max(0, Math.Min(newY, layout.Height - path.Height));

                path.TranslationX = clampedX;
                path.TranslationY = clampedY;
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

    private void OnPointerPathEntered(object sender, PointerEventArgs e)
    {
        if (BindingContext is MainViewModel { CurrentCanvas: { } } viewModel)
        {
            if (sender is Path path)
            {
#if WINDOWS
                if (path.Handler?.PlatformView is Microsoft.Maui.Graphics.Win2D.W2DGraphicsView panel)
                {
                    if (viewModel.CurrentCanvas!.Controls == ControlsType.Select)
                        panel.ChangeCursor(Microsoft.UI.Input.InputSystemCursor.Create(Microsoft.UI.Input.InputSystemCursorShape.SizeAll));
                }
#endif
            }
        }
    }

    private void OnPointerMovedInsideCanvas(object sender, PointerEventArgs e) => pointerPos = e.GetPosition((AbsoluteLayout)sender);

    private void OnPointerExitedFromCanvas(object sender, PointerEventArgs e) => pointerPos = null;

}