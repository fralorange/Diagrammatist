using Diagrammatist.Presentation.WPF.Core.Helpers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Shell;
using ApplicationEnt = System.Windows.Application;

namespace Diagrammatist.Presentation.WPF.Core.Controls
{
    /// <summary>
    /// A class that derives from <see cref="Window"/>.
    /// This class represents window with custom title bar.
    /// </summary>
    [ContentProperty(nameof(WindowContent))]
    public class TitleBarWindow : Window
    {
        private Button maximizeRestoreButton;
        private Grid titleBar;

        /// <summary>
        /// A title bar menu content dependency property.
        /// </summary>
        public static readonly DependencyProperty TitleBarMenuContentProperty =
            DependencyProperty.Register(nameof(TitleBarMenuContent), typeof(object), typeof(TitleBarWindow));

        /// <summary>
        /// A window content dependency property.
        /// </summary>
        public static readonly DependencyProperty WindowContentProperty =
            DependencyProperty.Register(nameof(WindowContent), typeof(object), typeof(TitleBarWindow));

        /// <summary>
        /// A center title parameter dependency property.
        /// </summary>
        public static readonly DependencyProperty ShowCenteredTitleProperty =
            DependencyProperty.Register(
                nameof(ShowCenteredTitle),
                typeof(bool),
                typeof(TitleBarWindow),
                new PropertyMetadata(false));

        /// <summary>
        /// A minimize button parameter dependency property.
        /// </summary>
        public static readonly DependencyProperty ShowMinimizeButtonProperty =
            DependencyProperty.Register(
                nameof(ShowMinimizeButton),
                typeof(bool),
                typeof(TitleBarWindow),
                new PropertyMetadata(true));

        /// <summary>
        /// A maximize button parameter dependency property.
        /// </summary>
        public static readonly DependencyProperty ShowMaximizeButtonProperty =
            DependencyProperty.Register(
                nameof(ShowMaximizeButton),
                typeof(bool),
                typeof(TitleBarWindow),
                new PropertyMetadata(true));

        /// <summary>
        /// A dialog parameter dependency property.
        /// </summary>
        public static readonly DependencyProperty IsDialogProperty =
            DependencyProperty.Register(
                nameof(IsDialog),
                typeof(bool),
                typeof(TitleBarWindow),
                new PropertyMetadata(false));

        /// <summary>
        /// Gets or sets title bar menu content.
        /// </summary>
        public object TitleBarMenuContent
        {
            get => GetValue(TitleBarMenuContentProperty);
            set => SetValue(TitleBarMenuContentProperty, value);
        }

        /// <summary>
        /// Gets or sets window content.
        /// </summary>
        public object WindowContent
        {
            get => GetValue(WindowContentProperty);
            set => SetValue(WindowContentProperty, value);
        }

        /// <summary>
        /// Gets or sets 'show centered title' parameter.
        /// </summary>
        public bool ShowCenteredTitle
        {
            get => (bool)GetValue(ShowCenteredTitleProperty);
            set => SetValue(ShowCenteredTitleProperty, value);
        }

        /// <summary>
        /// Gets or sets 'show minimize button' parameter.
        /// </summary>
        public bool ShowMinimizeButton
        {
            get => (bool)GetValue(ShowMinimizeButtonProperty);
            set => SetValue(ShowMinimizeButtonProperty, value);
        }

        /// <summary>
        /// Gets or sets 'show maximize button' parameter.
        /// </summary>
        public bool ShowMaximizeButton
        {
            get => (bool)GetValue(ShowMaximizeButtonProperty);
            set => SetValue(ShowMaximizeButtonProperty, value);
        }

        /// <summary>
        /// Gets or sets 'is dialog' parameter.
        /// </summary>
        public bool IsDialog
        {
            get => (bool)GetValue(IsDialogProperty);
            set => SetValue(IsDialogProperty, value);
        }

        /// <summary>
        /// Initializes title bar window.
        /// </summary>
#pragma warning disable CS8618
        public TitleBarWindow()
        {
            InitializeComponents();

            InitializeWindowChrome();

            ResizeMode = ResizeMode.CanResize;
            WindowStyle = WindowStyle.SingleBorderWindow;
            SnapsToDevicePixels = true;

            StateChanged += OnWindowStateChanged;
        }
#pragma warning restore CS8618

        private void InitializeWindowChrome()
        {
            // Get converters from resources
            var captionConverter = GetResource<IMultiValueConverter>("CaptionHeightMultiConverter");

            // Configure WindowChrome
            var chrome = new WindowChrome
            {
                CornerRadius = new CornerRadius(0),
                GlassFrameThickness = new Thickness(0),
                NonClientFrameEdges = NonClientFrameEdges.None,
                ResizeBorderThickness = new Thickness(5),
                UseAeroCaptionButtons = false
            };

            Loaded += (s, e) =>
            {
                if (IsDialog)
                {
                    chrome.GlassFrameThickness = new Thickness(0.1, 0, 0.1, 0.1);
                    chrome.NonClientFrameEdges = NonClientFrameEdges.Left | NonClientFrameEdges.Right | NonClientFrameEdges.Bottom;
                }
            };

            // Setup caption height binding
            var captionBinding = new MultiBinding { Converter = captionConverter };
            captionBinding.Bindings.Add(new Binding("ActualHeight") { Source = titleBar });
            captionBinding.Bindings.Add(new Binding("BorderThickness.Top") { Source = this });
            BindingOperations.SetBinding(this, WindowChrome.CaptionHeightProperty, captionBinding);

            WindowChrome.SetWindowChrome(this, chrome);
        }

        private void InitializeComponents()
        {
            // Main container
            var dockPanel = new DockPanel();
            Content = dockPanel;

            // Create title bar
            CreateTitleBar(dockPanel);

            // Window content
            var contentPresenter = new ContentPresenter();
            contentPresenter.SetBinding(ContentPresenter.ContentProperty,
                new Binding("WindowContent") { Source = this });

            dockPanel.Children.Add(contentPresenter);
        }

        private void CreateTitleBar(DockPanel dockPanel)
        {
            titleBar = new Grid
            {
                MinHeight = GetResource<double>("TitleBarHeight"),
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = GridLength.Auto },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star), MinWidth = 40 },
                    new ColumnDefinition { Width = GridLength.Auto },
                    new ColumnDefinition { Width = GridLength.Auto },
                    new ColumnDefinition { Width = GridLength.Auto }
                }
            };
            titleBar.SetResourceReference(Grid.BackgroundProperty, "TitleBarBackground");
            DockPanel.SetDock(titleBar, Dock.Top);
            dockPanel.Children.Add(titleBar);
            WindowChrome.SetIsHitTestVisibleInChrome(titleBar, false);

            // Window icon
            CreateIcon();

            // Menu and title
            CreateMenuSection();

            // Control buttons
            CreateMinimizeButton();
            CreateMaximizeButton();
            CreateCloseButton();
        }

        private void CreateIcon()
        {
            var icon = new Image
            {
                Style = GetResource<Style>("IconImage"),
            };
            icon.SetBinding(Image.SourceProperty, new Binding(nameof(Icon)) { Source = this });
            icon.MouseDown += OnIconMouseDown;
            titleBar.Children.Add(icon);
            Grid.SetColumn(icon, 0);
            WindowChrome.SetIsHitTestVisibleInChrome(icon, true);
        }

        private void CreateMenuSection()
        {
            var dockPanelInner = new DockPanel { HorizontalAlignment = HorizontalAlignment.Stretch };
            Grid.SetColumn(dockPanelInner, 1);
            titleBar.Children.Add(dockPanelInner);

            // Menu presenter
            var menuPresenter = new ContentPresenter
            {
                VerticalAlignment = VerticalAlignment.Center
            };
            menuPresenter.SetBinding(ContentPresenter.ContentProperty,
                new Binding("TitleBarMenuContent") { Source = this });
            DockPanel.SetDock(menuPresenter, Dock.Left);
            WindowChrome.SetIsHitTestVisibleInChrome(menuPresenter, true);
            dockPanelInner.Children.Add(menuPresenter);

            // Centered Title (hidden by default)
            var titleTextBlock = new TextBlock
            {
                Margin = new Thickness(10, 0, 10, 0),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                TextTrimming = TextTrimming.CharacterEllipsis,
                TextWrapping = TextWrapping.NoWrap
            };

            // Bind Text to Window.Title
            titleTextBlock.SetBinding(TextBlock.TextProperty,
                new Binding("Title") { Source = this });

            // Bind Visibility to ShowCenteredTitle with converter
            titleTextBlock.SetBinding(TextBlock.VisibilityProperty,
                new Binding(nameof(ShowCenteredTitle))
                {
                    Source = this,
                    Converter = new BooleanToVisibilityConverter()
                });

            dockPanelInner.Children.Add(titleTextBlock);
        }

        private void CreateMinimizeButton()
        {
            var button = new Button
            {
                Style = GetResource<Style>("TitleBarButtonStyle"),
                ToolTip = LocalizationHelper.GetLocalizedValue<string>("MainResources", "Minimize")
            };

            button.SetBinding(VisibilityProperty,
                new Binding(nameof(ShowMinimizeButton))
                {
                    Source = this,
                    Converter = new BooleanToVisibilityConverter()
                });

            button.Click += OnMinimizeButtonClick;

            var path = new Path
            {
                Width = 36,
                Height = 32,
                Data = Geometry.Parse("M 13,15 H 23"),
                StrokeThickness = 1
            };
            path.SetResourceReference(Path.StrokeProperty, "TitleBarButtonForeground");
            button.Content = path;

            titleBar.Children.Add(button);
            Grid.SetColumn(button, 2);
        }

        private void CreateMaximizeButton()
        {
            maximizeRestoreButton = new Button
            {
                Style = GetResource<Style>("TitleBarButtonStyle"),
                ToolTip = LocalizationHelper.GetLocalizedValue<string>("MainResources", "Maximize")
            };

            maximizeRestoreButton.SetBinding(VisibilityProperty,
                new Binding(nameof(ShowMaximizeButton))
                {
                    Source = this,
                    Converter = new BooleanToVisibilityConverter()
                });

            maximizeRestoreButton.Click += OnMaximizeRestoreButtonClick;
            maximizeRestoreButton.ToolTipOpening += OnMaximizeRestoreButtonToolTipOpening;

            var path = new Path
            {
                Width = 36,
                Height = 32,
                StrokeThickness = 1
            };
            path.SetResourceReference(Path.StrokeProperty, "TitleBarButtonForeground");

            var stateConverter = GetResource<IValueConverter>("WindowStateToPathConverter");
            path.SetBinding(Path.DataProperty, new Binding("WindowState")
            {
                Source = this,
                Converter = stateConverter
            });

            maximizeRestoreButton.Content = path;
            titleBar.Children.Add(maximizeRestoreButton);
            Grid.SetColumn(maximizeRestoreButton, 3);
        }

        private void CreateCloseButton()
        {
            var button = new Button
            {
                Style = GetResource<Style>("TitleBarCloseButtonStyle"),
                ToolTip = LocalizationHelper.GetLocalizedValue<string>("MainResources", "Close")
            };
            button.Click += OnCloseButtonClick;

            var path = new Path
            {
                Width = 36,
                Height = 32,
                Data = Geometry.Parse("M 13,11 22,20 M 13,20 22,11"),
                StrokeThickness = 1
            };
            path.SetResourceReference(Path.StrokeProperty, "TitleBarButtonForeground");
            button.Content = path;

            titleBar.Children.Add(button);
            Grid.SetColumn(button, 4);
        }

        private T GetResource<T>(string key)
        {
            if (ApplicationEnt.Current.Resources.Contains(key))
            {
                var resource = ApplicationEnt.Current.Resources[key];
                if (resource is T typedResource)
                {
                    return typedResource;
                }
                throw new InvalidCastException($"Resource '{key}' is not of type {typeof(T)}");
            }
            throw new ArgumentException($"Resource '{key}' not found");
        }

        // Event handlers
        private void OnMinimizeButtonClick(object sender, RoutedEventArgs e) => SystemCommands.MinimizeWindow(this);
        private void OnMaximizeRestoreButtonClick(object sender, RoutedEventArgs e) => ToggleWindowState();
        private void OnCloseButtonClick(object sender, RoutedEventArgs e) => SystemCommands.CloseWindow(this);
        private void OnIconMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2 && e.ChangedButton == MouseButton.Left)
            {
                Close();
            }
            else if (e.ChangedButton == MouseButton.Left || e.ChangedButton == MouseButton.Right)
            {
                ShowSystemMenu(this, e.GetPosition(this));
            }
        }

        private void ShowSystemMenu(Window window, Point point)
        {
            // Increment coordinates to allow double-click
            ++point.X;
            ++point.Y;
            if (window.WindowState == WindowState.Normal)
            {
                point.X += window.Left;
                point.Y += window.Top;
            }
            SystemCommands.ShowSystemMenu(window, point);
        }

        private void ToggleWindowState()
        {
            WindowState = WindowState == WindowState.Maximized
                ? WindowState.Normal
                : WindowState.Maximized;
        }

        private void OnWindowStateChanged(object? sender, EventArgs e)
        {
            var content = (WindowContent as FrameworkElement);

            if (WindowState == WindowState.Maximized)
            {
                titleBar.Margin = new Thickness(6, 6, 6, 0);
                content?.SetValue(FrameworkElement.MarginProperty, new Thickness(6, 0, 6, 6));
            }
            else
            {
                titleBar.Margin = new Thickness(0);
                content?.SetValue(FrameworkElement.MarginProperty, new Thickness(0));
            }
        }

        private void OnMaximizeRestoreButtonToolTipOpening(object sender, ToolTipEventArgs e)
        {
            maximizeRestoreButton.ToolTip = WindowState == WindowState.Normal
                ? LocalizationHelper.GetLocalizedValue<string>("MainResources", "Maximize")
                : LocalizationHelper.GetLocalizedValue<string>("MainResources", "Restore");
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
                            maximizeRestoreButton.Background = (Brush)ApplicationEnt.Current.Resources["TitleBarButtonHoverBackground"];
                            maximizeRestoreButton.Foreground = (Brush)ApplicationEnt.Current.Resources["TitleBarButtonHoverForeground"];
                            return new IntPtr(NativeHelper.HTMAXBUTTON);
                        }
                        else
                        {
                            // Apply default button style (cursor is not on the button)
                            maximizeRestoreButton.Background = (Brush)ApplicationEnt.Current.Resources["TitleBarButtonBackground"];
                            maximizeRestoreButton.Foreground = (Brush)ApplicationEnt.Current.Resources["TitleBarButtonForeground"];
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
                            maximizeRestoreButton.Background = (Brush)ApplicationEnt.Current.Resources["TitleBarButtonPressedBackground"];
                            maximizeRestoreButton.Foreground = (Brush)ApplicationEnt.Current.Resources["TitleBarButtonPressedForeground"];
                        }
                    }
                    break;
                case NativeHelper.WM_NCLBUTTONUP:
                    if (NativeHelper.IsSnapLayoutEnabled())
                    {
                        if (wParam.ToInt32() == NativeHelper.HTMAXBUTTON)
                        {
                            // Apply default button style
                            maximizeRestoreButton.Background = (Brush)ApplicationEnt.Current.Resources["TitleBarButtonBackground"];
                            maximizeRestoreButton.Foreground = (Brush)ApplicationEnt.Current.Resources["TitleBarButtonForeground"];
                            // Maximize or restore the window
                            ToggleWindowState();
                        }
                    }
                    break;
            }
            return IntPtr.Zero;
        }

        /// <inheritdoc/>
        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            var source = (HwndSource)PresentationSource.FromVisual(this);
            source.AddHook(WndProc);
        }
    }
}
