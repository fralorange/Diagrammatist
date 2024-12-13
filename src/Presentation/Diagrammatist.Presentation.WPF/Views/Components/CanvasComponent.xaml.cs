using Diagrammatist.Presentation.WPF.Framework.Controls;
using Diagrammatist.Presentation.WPF.Framework.Extensions.DependencyObject;
using Diagrammatist.Presentation.WPF.ViewModels.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using System.Windows.Controls;
using System.Windows.Input;

namespace Diagrammatist.Presentation.WPF.Views.Components
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
            viewModel.OnRequestExport += Export;

            DataContext = viewModel;

            InitializeComponent();
        }

        private void OnListBoxPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.Source is ListBox && e.OriginalSource is Canvas)
            {
                ClearSelection();
            }
        }

        private void ClearSelection()
        {
            itemsHolder.UnselectAll();
            Keyboard.ClearFocus();
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

        private void Export()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "PNG|*.png",
            };

            ClearSelection();

            if (saveFileDialog.ShowDialog() == true)
            {
                string filePath = saveFileDialog.FileName;

                var canvas = itemsHolder.GetVisualDescendant<ExtendedCanvas>();

                canvas?.Export(filePath);
            }
        }
    }
}
