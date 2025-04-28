using Diagrammatist.Presentation.WPF.Core.Helpers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
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
        /// Gets or sets 'show centered title'.
        /// </summary>
        public bool ShowCenteredTitle
        {
            get => (bool)GetValue(ShowCenteredTitleProperty);
            set => SetValue(ShowCenteredTitleProperty, value);
        }

        /// <summary>
        /// Gets or sets 'show minimize button'.
        /// </summary>
        public bool ShowMinimizeButton
        {
            get => (bool)GetValue(ShowMinimizeButtonProperty);
            set => SetValue(ShowMinimizeButtonProperty, value);
        }

        /// <summary>
        /// Gets or sets 'show maximize button'.
        /// </summary>
        public bool ShowMaximizeButton
        {
            get => (bool)GetValue(ShowMaximizeButtonProperty);
            set => SetValue(ShowMaximizeButtonProperty, value);
        }

        /// <summary>
        /// Initializes title bar window.
        /// </summary>
#pragma warning disable CS8618
        public TitleBarWindow()
        {
            InitializeComponents();

            InitializeWindowChrome();

            Style = (Style)FindResource(typeof(Window));
            ResizeMode = ResizeMode.CanResize;
            WindowStyle = WindowStyle.SingleBorderWindow;

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
                CornerRadius = SystemParameters.WindowCornerRadius,
                GlassFrameThickness = new Thickness(1, 0, 1, 1),
                NonClientFrameEdges = NonClientFrameEdges.Left | NonClientFrameEdges.Right | NonClientFrameEdges.Bottom,
                ResizeBorderThickness = SystemParameters.WindowResizeBorderThickness,
                UseAeroCaptionButtons = false
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
                Stroke = GetResource<Brush>("TitleBarButtonForeground"),
                StrokeThickness = 1
            };
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
                Stroke = GetResource<Brush>("TitleBarButtonForeground"),
                StrokeThickness = 1
            };

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
                Stroke = GetResource<Brush>("TitleBarButtonForeground"),
                StrokeThickness = 1
            };
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
            if (WindowState == WindowState.Maximized)
            {
                titleBar.Margin = new Thickness(4, 6, 4, 0);
            }
            else
            {
                titleBar.Margin = new Thickness(0);
            }
        }

        private void OnMaximizeRestoreButtonToolTipOpening(object sender, ToolTipEventArgs e)
        {
            maximizeRestoreButton.ToolTip = WindowState == WindowState.Normal
                ? LocalizationHelper.GetLocalizedValue<string>("MainResources", "Maximize")
                : LocalizationHelper.GetLocalizedValue<string>("MainResources", "Restore");
        }
    }
}
