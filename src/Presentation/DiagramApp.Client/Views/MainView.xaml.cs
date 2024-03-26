using DiagramApp.Client.ViewModels;
using DiagramApp.Domain.Canvas;
using DiagramApp.Client.Extensions.UIElement;
using Microsoft.UI.Xaml;
using Microsoft.UI.Input;

namespace DiagramApp.Client
{
    public partial class MainView : ContentPage
    {
        private Point _deltaPosition;
        private double _horizontalScrollPosition;
        private double _verticalScrollPosition;

        public MainView(MainViewModel viewmodel)
        {
            InitializeComponent();

            BindingContext = viewmodel;
        }

        private async void OnScrollToPosition(double? scrollX = null, double? scrollY = null)
        {
            // Scrolls to center by default
            scrollX ??= (CanvasScrollWindow.ContentSize.Width - CanvasScrollWindow.Width) / 2.0;
            scrollY ??= (CanvasScrollWindow.ContentSize.Height - CanvasScrollWindow.Height) / 2.0;
            await CanvasScrollWindow.ScrollToAsync(scrollX.Value, scrollY.Value, false);
        }

        private void OnResetViewClicked(object sender, EventArgs e)
        {
            if (BindingContext is MainViewModel viewModel && viewModel.CurrentCanvas != null)
            {
                viewModel.ZoomResetCommand.Execute(null);
                OnScrollToPosition();
            }
        }

        private void OnExitClicked(object sender, EventArgs e)
        {
            App.Current!.Quit();
        }

        private async void OnPanUpdated(object sender, PanUpdatedEventArgs e)
        {
            if (BindingContext is MainViewModel viewModel && viewModel.IsCanvasNotNull && viewModel.CurrentCanvas!.Controls == ControlsType.Drag)
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

        private void OnPointerEntered(object sender, Microsoft.Maui.Controls.PointerEventArgs e)
        {
            if (BindingContext is MainViewModel viewModel && viewModel.IsCanvasNotNull)
            {
                if (sender is AbsoluteLayout layout)
                {
#if WINDOWS
                    if (layout.Handler?.PlatformView is Microsoft.UI.Xaml.Controls.Panel panel)
                    {
                        if (viewModel.CurrentCanvas!.Controls == ControlsType.Drag)
                            panel.ChangeCursor(InputSystemCursor.Create(InputSystemCursorShape.Hand));
                        else
                            panel.ChangeCursor(InputSystemCursor.Create(InputSystemCursorShape.Arrow));
                    }
#endif
                }
            }
        }
    }
}
