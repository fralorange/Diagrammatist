using Diagrammatist.Presentation.WPF.Core.Controls;
using Diagrammatist.Presentation.WPF.ViewModels.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using System.Windows.Controls;
using System.Windows.Input;
using Diagrammatist.Presentation.WPF.Core.Foundation.Extensions;
using System.Windows;
using System.Windows.Threading;

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

            InitializeEvents(viewModel);

            DataContext = viewModel;

            InitializeComponent();
        }

        private void InitializeEvents(CanvasViewModel viewModel)
        {
            viewModel.RequestZoomIn += ZoomIn;
            viewModel.RequestZoomOut += ZoomOut;
            viewModel.RequestZoomReset += ZoomReset;
            viewModel.RequestSaveAs += SaveAs;
            viewModel.RequestExport += Export;
        }

        private void ClearSelection(IInputElement? focus = null)
        {
            itemsHolder.UnselectAll();

            if (focus is null)
            {
                Keyboard.ClearFocus();
            }
            else
            {
                Keyboard.Focus(focus);
            }
        }

        private void OnListBoxPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.Source is ListBox && e.OriginalSource is Canvas canvas)
            {
                ClearSelection(canvas);
            }
        }

        private void CaptureMousePosition(ExtendedCanvas canvas, Action<Point> positionHandler)
        {
            if (canvas == null) return;

            var mousePos = Mouse.GetPosition(canvas);
            positionHandler?.Invoke(mousePos);
        }

        private void OnExtendedCanvasPreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.Source is ExtendedCanvas itemsPanel)
            {
                CaptureMousePosition(itemsPanel, pos =>
                {
                    if (itemsPanel.ContextMenu != null)
                    {
                        itemsPanel.ContextMenu.Tag = pos;
                    }
                });
            }
        }

        private void OnExtendedCanvasPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.V && e.Source is ExtendedCanvas itemsPanel)
            {
                CaptureMousePosition(itemsPanel, pos => Tag = pos);
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

        private string SaveAs(string fileName)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = $"{App.Current.Resources["Filter"]}|*.{App.Current.Resources["Extension"]}",
                FileName = fileName,
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                return saveFileDialog.FileName;
            }

            return string.Empty;
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
