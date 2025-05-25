using Diagrammatist.Presentation.WPF.Core.Controls;
using Diagrammatist.Presentation.WPF.Core.Helpers;
using Diagrammatist.Presentation.WPF.Core.Services.Alert;
using Diagrammatist.Presentation.WPF.Core.Shared.Enums;
using Diagrammatist.Presentation.WPF.ViewModels;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace Diagrammatist.Presentation.WPF.Views
{
    /// <summary>
    /// A class that represents main window and derives from <see cref="TitleBarWindow"/>.
    /// </summary>
    /// <remarks>
    /// This class can be considered as the whole app that builds from different components from <see cref="Components"/> namespace as the puzzle.
    /// </remarks>
    public partial class MainWindow : TitleBarWindow
    {
        private readonly IAlertService _alertService;

        /// <summary>
        /// Initializes main window services and events.
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="alertService"></param>
        public MainWindow(MainViewModel viewModel, IAlertService alertService)
        {
            DataContext = viewModel;
            _alertService = alertService;

            viewModel.OnRequestClose += CloseWindow;

            // Launch on top.
            Loaded += (s, e) =>
            {
                Topmost = true;
                Topmost = false;
            };
        }

        /// <summary>
        /// Brings main window to the foreground, if it is minimized or hidden.
        /// </summary>
        public void BringToForeground()
        {
            if (WindowState == WindowState.Minimized || Visibility == Visibility.Hidden)
            {
                Show();
                WindowState = WindowState.Normal;
            }
    
            Activate();
            Topmost = true;
            Topmost = false;
            Focus();
        }

        /// <summary>
        /// Loads main window.
        /// </summary>
        /// <param name="progress"></param>
        /// <returns></returns>
        public async Task LoadAsync(IProgress<(int, string)> progress)
        {
            await Dispatcher.InvokeAsync(InitializeComponent);
            progress.Report((10, "Core"));

            var loadingSteps = new[]
            {
                ("ToolbarComponentName", 30, "Toolbar"),
                ("FiguresComponentName", 50, "Figures"),
                ("CanvasComponentName", 65, "Canvas"),
                ("TabsComponentName", 75, "Tabs"),
                ("ObjectTreeComponentName", 85, "Tree"),
                ("PropertiesComponentName", 90, "Properties"),
                ("ActionComponentName", 95, "Action")
            };

            // Loads components.
            foreach (var step in loadingSteps)
            {
                await LoadComponentAsync(step.Item1);
                progress.Report((step.Item2, step.Item3));
            }

            // Smooth finish.
            progress.Report((100, "Finish"));
            await Task.Delay(200);
        }

        /// <summary>
        /// Loads file into the application from upper layer (e.g. from command line or file association).
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public async Task LoadFileAsync(string filePath)
        {
            await Dispatcher.InvokeAsync(() =>
            {
                TabsComponentName.OpenFile(filePath);
            });
        }

        private async Task LoadComponentAsync(string componentName)
        {
            await Dispatcher.InvokeAsync(() =>
            {
                if (FindName(componentName) is FrameworkElement component)
                {
                    component.UpdateLayout();
                }
            });
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            if (DataContext is MainViewModel viewModel && viewModel.HasGlobalChangesFlag)
            {
                var result = _alertService.RequestConfirmation(LocalizationHelper.GetLocalizedValue<string>("Alert.AlertResources", "UnsavedAppMessage"),
                    LocalizationHelper.GetLocalizedValue<string>("Alert.AlertResources", "UnsavedAppCaption"));

                if (result == ConfirmationResult.Yes && !viewModel.SaveAll() || result == ConfirmationResult.Cancel)
                {
                    e.Cancel = true;
                }
            }

            SaveGridSizes();
        }

        private void CloseWindow()
        {
            Close();
        }

        private CustomPopupPlacement[] PlacePopup(Size popupSize, Size targetSize, Point offset)
        {
            const double leftOffset = 25;

            double x = targetSize.Width - popupSize.Width - leftOffset;
            double y = -popupSize.Height;

            var point = new Point(x, y);
            return [new CustomPopupPlacement(point, PopupPrimaryAxis.Horizontal)];
        }

        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            LeftPanel.Width = new GridLength(Properties.Settings.Default.LeftPanelWidth);
            RightPanel.Width = new GridLength(Properties.Settings.Default.RightPanelWidth);
            BottomPanel.Height = new GridLength(Properties.Settings.Default.BottomPanelHeight);
        }

        private void SaveGridSizes()
        {
            Properties.Settings.Default.LeftPanelWidth = LeftPanel.ActualWidth;
            Properties.Settings.Default.RightPanelWidth = RightPanel.ActualWidth;
            Properties.Settings.Default.BottomPanelHeight = BottomPanel.ActualHeight;

            Properties.Settings.Default.Save();
        }
    }
}
