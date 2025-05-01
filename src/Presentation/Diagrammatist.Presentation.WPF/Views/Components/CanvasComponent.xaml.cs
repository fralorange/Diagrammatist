using Diagrammatist.Presentation.WPF.Core.Controls;
using Diagrammatist.Presentation.WPF.Core.Foundation.Extensions;
using Diagrammatist.Presentation.WPF.Core.Helpers;
using Diagrammatist.Presentation.WPF.Core.Models.Figures;
using Diagrammatist.Presentation.WPF.Core.Models.Figures.Magnetic;
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
        private Canvas _drawingCanvas;
        private ListBox _itemsHolder;

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
            viewModel.RequestExport += Export;
        }

        #region Event handlers

        /// <summary>
        /// Sets items holder to the field.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnListBoxLoaded(object sender, RoutedEventArgs e) => _itemsHolder = (sender as ListBox)!;

        /// <summary>
        /// Sets drawing canvas to the field.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCanvasLoaded(object sender, RoutedEventArgs e) => _drawingCanvas = (sender as Canvas)!;

        /// <summary>
        /// Disables selection when clicked outside of an object.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnListBoxPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.Source is ListBox listBox && e.OriginalSource is Canvas canvas)
            {
                FocusHelper.ClearFocusAndSelection(listBox, canvas);
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
                    _lineDrawer = new LineDrawer(_drawingCanvas, canvas.GridStep);

                    _lineDrawer.RequestEarlyExit += actionViewModel.EarlyConfirm;
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
            if (sender is ExtendedCanvas itemsPanel && EnsureModeIsLineFromViewModel())
            {
                var startPoint = e.GetPosition(itemsPanel);

                var magneticPoints = GetMagneticPointsFromViewModel();

                _lineDrawer.AddPoint(startPoint, magneticPoints);
            }
        }

        /// <summary>
        /// Processes mouse move in renderers.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnExtendedCanvasPreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (sender is ExtendedCanvas itemsPanel && _lineDrawer.IsDrawing && EnsureModeIsLineFromViewModel())
            {
                var movePoint = e.GetPosition(itemsPanel);

                var isShiftPressed = Keyboard.IsKeyDown(Key.LeftShift);
                var isCtrlPressed = Keyboard.IsKeyDown(Key.LeftCtrl);

                var magneticPoints = GetMagneticPointsFromViewModel();

                _lineDrawer.UpdateLine(movePoint, isShiftPressed, isCtrlPressed, magneticPoints);
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

        #region Data context handlers

        private IEnumerable<MagneticPointModel> GetMagneticPointsFromViewModel()
        {
            if (DataContext is CanvasViewModel viewModel && viewModel.Figures?.OfType<ShapeFigureModel>() is { } figures)
            {
                var points = new List<MagneticPointModel>();

                foreach (var figure in figures)
                {
                    var figurePosition = new Point(figure.PosX, figure.PosY);

                    foreach (var magneticPoint in figure.MagneticPoints)
                    {
                        var globalPoint = new Point(
                            figurePosition.X + magneticPoint.Position.X,
                            figurePosition.Y + magneticPoint.Position.Y);

                        points.Add(new MagneticPointModel() { Owner = magneticPoint.Owner, Position = globalPoint });
                    }
                }

                return points;
            }

            return Enumerable.Empty<MagneticPointModel>();
        }

        private bool EnsureModeIsLineFromViewModel()
        {
            return DataContext is CanvasViewModel viewModel && viewModel.CurrentMouseMode is MouseMode.Line;
        }

        #endregion

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

        private void Export()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "PNG|*.png",
            };

            FocusHelper.ClearFocusAndSelection(_itemsHolder);

            if (saveFileDialog.ShowDialog() == true)
            {
                string filePath = saveFileDialog.FileName;

                var canvas = _itemsHolder.GetVisualDescendant<ExtendedCanvas>();

                canvas?.Export(filePath);
            }
        }
    }
}
