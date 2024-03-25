using DiagramApp.Client.ViewModels;

namespace DiagramApp.Client
{
    public partial class MainView : ContentPage
    {
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
            Application.Current!.Quit();
        }

        private void OnPanUpdated(object sender, PanUpdatedEventArgs e)
        {
            switch (e.StatusType)
            {
                case GestureStatus.Running:
                    Point? currentLocation = new Point(e.TotalX, e.TotalY);

                    if (currentLocation.HasValue)
                    {
                        (BindingContext as MainViewModel)!.MoveCanvasCommand.Execute((Convert.ToInt32(currentLocation.Value.X), Convert.ToInt32(currentLocation.Value.Y)));
                    }
                    break;
            }
        }
    }
}
