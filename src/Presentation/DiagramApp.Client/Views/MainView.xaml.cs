using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Storage;
using DiagramApp.Client.Extensions.UIElement;
using DiagramApp.Client.ViewModels;
using DiagramApp.Client.ViewModels.Wrappers;
using DiagramApp.Domain.Canvas;
using DiagramApp.Domain.Canvas.Figures;
using DiagramApp.Domain.Toolbox;
using Microsoft.UI.Xaml.Media.Imaging;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Graphics.Imaging;
using Path = Microsoft.Maui.Controls.Shapes.Path;

namespace DiagramApp.Client
{
    public partial class MainView : ContentPage
    {
        private Point? pointerPos;

        // refactor later to contain this in dynamic resources maybe (clean code)
        private Point _deltaPosition;
        private double _horizontalScrollPosition;
        private double _verticalScrollPosition;

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
            if (CanvasView.Handler?.PlatformView is Microsoft.UI.Xaml.UIElement elem && BindingContext is MainViewModel viewModel && viewModel.IsCanvasNotNull)
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

        private async void OnPanCanvasUpdated(object sender, PanUpdatedEventArgs e)
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

        private void OnPointerEntered(object sender, PointerEventArgs e)
        {
            if (BindingContext is MainViewModel viewModel && viewModel.IsCanvasNotNull)
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
            if (BindingContext is MainViewModel viewModel && viewModel.IsCanvasNotNull)
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

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is CollectionView collectionView && collectionView.SelectedItem is not null)
            {
                if (BindingContext is MainViewModel viewModel && viewModel.IsCanvasNotNull)
                {
                    var current = e.CurrentSelection[0] as ToolboxItem;
                    var figure = new Figure
                    {
                        Name = current!.Name,
                        PathData = current!.PathData,
                    };

                    viewModel.CurrentCanvas!.Figures.Add(new ObservableFigure(figure));
                }

                Dispatcher.Dispatch(() =>
                {
                    collectionView.SelectedItem = null;
                });
            }
        }

        private void OnPanElementUpdated(object sender, PanUpdatedEventArgs e)
        {
            if (sender is Path path && path.Parent is AbsoluteLayout layout && BindingContext is MainViewModel viewModel && viewModel.CurrentCanvas!.Controls == ControlsType.Select)
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

        private void OnPointerMovedInsideCanvas(object sender, PointerEventArgs e) => pointerPos = e.GetPosition((AbsoluteLayout)sender);

        private void OnPointerExitedFromCanvas(object sender, PointerEventArgs e) => pointerPos = null;

        private async void OnScrollToPosition(double? scrollX = null, double? scrollY = null)
        {
            // Scrolls to center by default
            scrollX ??= (CanvasScrollWindow.ContentSize.Width - CanvasScrollWindow.Width) / 2.0;
            scrollY ??= (CanvasScrollWindow.ContentSize.Height - CanvasScrollWindow.Height) / 2.0;
            await CanvasScrollWindow.ScrollToAsync(scrollX.Value, scrollY.Value, false);
        }
    }
}
