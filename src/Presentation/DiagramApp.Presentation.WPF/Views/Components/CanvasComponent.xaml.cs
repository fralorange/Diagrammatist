using DiagramApp.Presentation.WPF.ViewModels.Components;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;
using System.Windows.Input;

namespace DiagramApp.Presentation.WPF.Views.Components
{
    /// <summary>
    /// A class that represents canvas component and derives from <see cref="UserControl"/>.
    /// </summary>
    /// <remarks>
    /// Main module of the app that used to show current selected canvas and items on it.
    /// </remarks>
    public partial class CanvasComponent : UserControl
    {
        public CanvasComponent()
        {
            var viewModel = App.Current.Services.GetRequiredService<CanvasViewModel>();

            viewModel.OnRequestZoomIn += ZoomIn;
            viewModel.OnRequestZoomOut += ZoomOut;
            viewModel.OnRequestZoomReset += ZoomReset;

            DataContext = viewModel;

            InitializeComponent();
        }

        private void OnListBoxPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.Source is ListBox listBox && e.OriginalSource is Canvas)
            {
                listBox.UnselectAll();
                Keyboard.ClearFocus();
            }
        }

        private void ZoomIn()
        {
            extScrollViewer.ZoomIn();
        }

        private void ZoomOut()
        {
            extScrollViewer.ZoomOut();
        }

        private void ZoomReset()
        {
            extScrollViewer.ZoomReset();
        }
    }
}
