using Diagrammatist.Presentation.WPF.Core.Helpers;
using Diagrammatist.Presentation.WPF.Core.Services.Alert;
using Diagrammatist.Presentation.WPF.ViewModels;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;

namespace Diagrammatist.Presentation.WPF.Views
{
    /// <summary>
    /// A class that represents main window and derives from <see cref="Window"/>.
    /// </summary>
    /// <remarks>
    /// This class can be considered as the whole app that builds from different components from <see cref="Components"/> namespace as the puzzle.
    /// </remarks>
    public partial class MainWindow : Window
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

        // TO-DO Create 'Custom TitleBar' component and inherit from it in here.
        private void OnSourceInitialized(object? sender, EventArgs e)
        {
            var source = (HwndSource)PresentationSource.FromVisual(this);
            source.AddHook(WndProc);
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case NativeHelper.WM_NCHITTEST:
                    if (NativeHelper.IsSnapLayoutEnabled())
                    {
                        // Return HTMAXBUTTON when the mouse is over the maximize/restore button
                        var point = PointFromScreen(new Point(lParam.ToInt32() & 0xFFFF, lParam.ToInt32() >> 16));
                        if (WpfHelper.GetElementBoundsRelativeToWindow(maximizeRestoreButton, this).Contains(point))
                        {
                            handled = true;
                            // Apply hover button style
                            maximizeRestoreButton.Background = (Brush)App.Current.Resources["TitleBarButtonHoverBackground"];
                            maximizeRestoreButton.Foreground = (Brush)App.Current.Resources["TitleBarButtonHoverForeground"];
                            return new IntPtr(NativeHelper.HTMAXBUTTON);
                        }
                        else
                        {
                            // Apply default button style (cursor is not on the button)
                            maximizeRestoreButton.Background = (Brush)App.Current.Resources["TitleBarButtonBackground"];
                            maximizeRestoreButton.Foreground = (Brush)App.Current.Resources["TitleBarButtonForeground"];
                        }
                    }
                    break;
                case NativeHelper.WM_NCLBUTTONDOWN:
                    if (NativeHelper.IsSnapLayoutEnabled())
                    {
                        if (wParam.ToInt32() == NativeHelper.HTMAXBUTTON)
                        {
                            handled = true;
                            // Apply pressed button style
                            maximizeRestoreButton.Background = (Brush)App.Current.Resources["TitleBarButtonPressedBackground"];
                            maximizeRestoreButton.Foreground = (Brush)App.Current.Resources["TitleBarButtonPressedForeground"];
                        }
                    }
                    break;
                case NativeHelper.WM_NCLBUTTONUP:
                    if (NativeHelper.IsSnapLayoutEnabled())
                    {
                        if (wParam.ToInt32() == NativeHelper.HTMAXBUTTON)
                        {
                            // Apply default button style
                            maximizeRestoreButton.Background = (Brush)App.Current.Resources["TitleBarButtonBackground"];
                            maximizeRestoreButton.Foreground = (Brush)App.Current.Resources["TitleBarButtonForeground"];
                            // Maximize or restore the window
                            ToggleWindowState();
                        }
                    }
                    break;
            }
            return IntPtr.Zero;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            if (DataContext is MainViewModel viewModel && viewModel.HasGlobalChangesFlag)
            {
                var result = _alertService.RequestConfirmation(LocalizationHelper.GetLocalizedValue<string>("Alert.AlertResources", "UnsavedAppMessage"),
                    LocalizationHelper.GetLocalizedValue<string>("Alert.AlertResources", "UnsavedAppCaption"));

                if (result == MessageBoxResult.Yes && !viewModel.SaveAll() || result == MessageBoxResult.Cancel)
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

        private void OnMinimizeButtonClick(object sender, RoutedEventArgs e)
        {
            SystemCommands.MinimizeWindow(this);
        }

        private void OnMaximizeRestoreButtonClick(object sender, RoutedEventArgs e)
        {
            ToggleWindowState();
        }

        private void OnMaximizeRestoreButtonToolTipOpening(object sender, ToolTipEventArgs e)
        {
            maximizeRestoreButton.ToolTip = WindowState == WindowState.Normal
                ? LocalizationHelper.GetLocalizedValue<string>("MainResources", "Maximize")
                : LocalizationHelper.GetLocalizedValue<string>("MainResources", "Restore");
        }

        private void OnCloseButtonClick(object sender, RoutedEventArgs e)
        {
            TitleBarHelper.CloseWindow(this);
        }

        private void OnIconMouseDown(object sender, MouseButtonEventArgs e)
        {
            TitleBarHelper.DoubleClickProcess(this, e);
        }

        public void ToggleWindowState()
        {
            if (WindowState == WindowState.Maximized)
            {
                SystemCommands.RestoreWindow(this);
            }
            else
            {
                SystemCommands.MaximizeWindow(this);
            }
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
