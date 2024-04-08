using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Storage;
using DiagramApp.Client.Controls;
using DiagramApp.Client.ViewModels;
using DiagramApp.Client.ViewModels.Wrappers;
using DiagramApp.Domain.Canvas.Figures;
using DiagramApp.Domain.Toolbox;
using Microsoft.UI.Xaml.Media.Imaging;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Graphics.Imaging;

namespace DiagramApp.Client
{
    public partial class MainView : ContentPage
    {
        public MainView(MainViewModel viewmodel)
        {
            InitializeComponent();

            BindingContext = viewmodel;
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

        private async void ExportButtonClicked(object sender, EventArgs e)
        {
#if WINDOWS
            var canvasView = CanvasWindow.GetVisualTreeDescendants().OfType<Border>().FirstOrDefault(b => b.StyleId == "CanvasView");
            if (canvasView?.Handler?.PlatformView is Microsoft.UI.Xaml.UIElement elem && BindingContext is MainViewModel { IsCanvasNotNull: true } viewModel)
            {
                RenderTargetBitmap renderTargetBitmap = new();
                await renderTargetBitmap.RenderAsync(elem);

                var width = renderTargetBitmap.PixelWidth;
                var height = renderTargetBitmap.PixelHeight;

                var pixels = await renderTargetBitmap.GetPixelsAsync();

                using var stream = new MemoryStream();
                var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, stream.AsRandomAccessStream());
                encoder.SetPixelData(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Ignore, (uint)width, (uint)height, 96, 96, pixels.ToArray());
                await encoder.FlushAsync();

                stream.Seek(0, SeekOrigin.Begin);

                var fileSaverResult = await FileSaver.Default.SaveAsync($"{viewModel.CurrentCanvas!.Settings.FileName}.png", stream);
#if DEBUG
                if (fileSaverResult.IsSuccessful)
                {
                    await Toast.Make($"File saved in: {fileSaverResult.FilePath}").Show();
                }
                else
                {
                    await Toast.Make($"File saving occurred an error: {fileSaverResult.Exception.Message}").Show();
                }
#endif
            }
#endif
        }

        private async void OnScrollToPosition(double? scrollX = null, double? scrollY = null)
        {
            if (CanvasWindow.Content is BindableScrollView canvasScrollWindow)
            {
                scrollX ??= (canvasScrollWindow.ContentSize.Width - canvasScrollWindow.Width) / 2.0;
                scrollY ??= (canvasScrollWindow.ContentSize.Height - canvasScrollWindow.Height) / 2.0;
                await canvasScrollWindow.ScrollToAsync(scrollX.Value, scrollY.Value, false);
            }
        }
    }
}
