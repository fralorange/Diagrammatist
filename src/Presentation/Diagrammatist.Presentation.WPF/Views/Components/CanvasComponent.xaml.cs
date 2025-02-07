using Diagrammatist.Presentation.WPF.Core.Controls;
using Diagrammatist.Presentation.WPF.Core.Foundation.Extensions;
using Diagrammatist.Presentation.WPF.Core.Renderers.Line;
using Diagrammatist.Presentation.WPF.ViewModels.Components;
using Diagrammatist.Presentation.WPF.ViewModels.Components.Enums.Modes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
        private LineDrawer _lineDrawer;

#pragma warning disable CS8618
        public CanvasComponent()
        {
            var viewModel = App.Current.Services.GetRequiredService<CanvasViewModel>();

            InitializeEvents(viewModel);

            DataContext = viewModel;

            InitializeComponent();
        }
#pragma warning restore CS8618

        private void InitializeEvents(CanvasViewModel viewModel)
        {
            viewModel.RequestZoomIn += ZoomIn;
            viewModel.RequestZoomOut += ZoomOut;
            viewModel.RequestZoomReset += ZoomReset;
            viewModel.RequestSaveAs += SaveAs;
            viewModel.RequestExport += Export;
        }

        #region Event handlers

        /// <summary>
        /// Disables selection when clicked outside of an object.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnListBoxPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.Source is ListBox && e.OriginalSource is Canvas canvas)
            {
                ClearSelection(canvas);
            }
        }

        /// <summary>
        /// Loads renderers.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnExtendedCanvasLoaded(object sender, RoutedEventArgs e)
        {
            if (sender is ExtendedCanvas canvas)
            {
                var actionViewModel = App.Current.Services.GetRequiredService<ActionViewModel>();

                Dispatcher.BeginInvoke(new Action(() =>
                {
                    _lineDrawer = new LineDrawer(drawingCanvas, canvas.GridStep);

                    actionViewModel.RequestEndDrawing += _lineDrawer.EndDrawing;
                }), DispatcherPriority.Loaded);
            }
        }

        /// <summary>
        /// Processes left mouse button click in renderers.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnExtendedCanvasPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is ExtendedCanvas itemsPanel && ModeIsLine())
            {
                var startPoint = e.GetPosition(itemsPanel);

                _lineDrawer.AddPoint(startPoint);
            }
        }

        /// <summary>
        /// Processes mouse move in renderers.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnExtendedCanvasPreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (sender is ExtendedCanvas itemsPanel && _lineDrawer.IsDrawing && ModeIsLine())
            {
                var movePoint = e.GetPosition(itemsPanel);

                var isShiftPressed = Keyboard.IsKeyDown(Key.LeftShift);

                _lineDrawer.UpdateLine(movePoint, isShiftPressed);
            }
        }

        /// <summary>
        /// Translates mouse position to context menu, so object can appear in the same spot.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Translates mouse position to context menu, so object can be pasted in the same spot.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnExtendedCanvasPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.V && e.Source is ExtendedCanvas itemsPanel)
            {
                CaptureMousePosition(itemsPanel, pos => Tag = pos);
            }
        }

        #endregion

        private bool ModeIsLine()
        {
            return DataContext is CanvasViewModel viewModel && viewModel.CurrentMouseMode is MouseMode.Line;
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

        private void CaptureMousePosition(ExtendedCanvas canvas, Action<Point> positionHandler)
        {
            if (canvas == null) return;

            var mousePos = Mouse.GetPosition(canvas);
            positionHandler?.Invoke(mousePos);
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
