using Diagrammatist.Presentation.WPF.Core.Foundation.Extensions;
using Diagrammatist.Presentation.WPF.Core.Helpers;
using Diagrammatist.Presentation.WPF.Core.Services.Alert;
using Diagrammatist.Presentation.WPF.Views;
using Diagrammatist.Presentation.WPF.Views.Splash;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.IO.Pipes;
using System.Windows.Threading;
using ApplicationEntity = System.Windows.Application;

namespace Diagrammatist.Presentation.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public sealed partial class App : ApplicationEntity
    {
        /// <summary>
        /// Gets the current <see cref="App"/> instance in use
        /// </summary>
        public new static App Current => (App)ApplicationEntity.Current;

        /// <summary>
        /// Gets the <see cref="IServiceProvider"/> instance to resolve application services.
        /// </summary>
        public IServiceProvider Services { get; }

        /// <summary>
        /// Mutex to ensure that only one instance of the application is running at a time.
        /// </summary>
        private static Mutex? _mutex;

        /// <summary>
        /// Cancellation token source for the named pipe server to handle inter-process communication.
        /// </summary>
        private CancellationTokenSource? _pipeCts;

        /// <summary>
        /// Initializes a new instance of the <see cref="App"/> class.
        /// </summary>
        public App()
        {
            Services = ConfigureServices();
        }

        private static ServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddStartupServices()
                .AddViewModels()
                .AddServices()
                .AddFacades()
                .AddSerializers()
                .AddRepositories()
                .AddDialogServices()
                .AddFactories()
                .AddManagers()
                .AddCulture()
                .AddMappers()
                .BuildServiceProvider();
        }

        /// <summary>
        /// Applies the theme based on user settings.
        /// </summary>
        private void ApplyTheme()
        {
            Current.ChangeTheme(WPF.Properties.Settings.Default.Theme);
        }

        /// <summary>
        /// Initializes the main window asynchronously and displays the splash screen while loading.
        /// </summary>
        /// <returns></returns>
        private async Task<MainWindow> InitializeMainWindowAsync()
        {
            var splash = Services.GetService<SplashScreen>();
            splash!.Show();

            var mainWindow = Services.GetService<MainWindow>();
            var progress = new Progress<(int, string)>((progress) =>
            {
                splash!.UpdateProgress(progress.Item1, progress.Item2);
            });

            await mainWindow!.LoadAsync(progress);

            splash.Close();
            mainWindow.Show();

            Current.MainWindow = mainWindow;

            return mainWindow;
        }

        /// <summary>
        /// Handles the startup file arguments and loads the specified file into the main window if it exists.
        /// </summary>
        /// <param name="args"></param>
        /// <param name="mainWindow"></param>
        /// <returns></returns>
        private async Task HandleStartupFileArgsAsync(string[] args, MainWindow mainWindow)
        {
            var filePath = args.FirstOrDefault();
            if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
            {
                await mainWindow.LoadFileAsync(filePath);
            }
        }

        /// <summary>
        /// Tries to send arguments to a running instance of the application using named pipes.
        /// </summary>
        /// <param name="args"> The arguments to send to the running instance. </param> 
        /// <returns></returns>
        private void TrySendArgsToRunningInstance(string[] args)
        {
            const int maxRetries = 5;
            const int delayMs = 500;

            using var client = new NamedPipeClientStream(".", AppMetadata.AppPipe, PipeDirection.Out);
            
            for (int i = 0; i < maxRetries; i++)
            {
                try
                {
                    client.Connect(1000);

                    using var writer = new StreamWriter(client) { AutoFlush = true };

                    writer.WriteLine(args.Length > 0 ? args[0] : "");
                    return;
                }
                catch (TimeoutException)
                {
                    Task.Delay(delayMs).Wait();
                }
                catch (Exception ex)
                {
                    Services.GetRequiredService<IAlertService>().ShowError(
                        ex.Message,
                        LocalizationHelper.GetLocalizedValue<string>("Alert.AlertResources", "SingleInstanceError"));
                    return;
                }
            }
        }

        /// <summary>
        /// Ensures that only a single instance of the application is running.
        /// </summary>
        /// <param name="args"> The startup arguments passed to the application. </param>
        /// <returns></returns>
        private bool EnsureSingleInstance(string[] args)
        {
            _mutex = new Mutex(true, AppMetadata.Title, out var isNewInstance);

            if (!isNewInstance)
            {
                TrySendArgsToRunningInstance(args);
            }

            return isNewInstance;
        }

        /// <summary>
        /// Starts the named pipe server to handle inter-process communication.
        /// </summary>
        private async Task StartPipeServerAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    using var server = new NamedPipeServerStream(
                        AppMetadata.AppPipe,
                        PipeDirection.In,
                        NamedPipeServerStream.MaxAllowedServerInstances,
                        PipeTransmissionMode.Message,
                        PipeOptions.Asynchronous
                    );

                    await server.WaitForConnectionAsync(token).ConfigureAwait(false);

                    using var reader = new StreamReader(server);
                    var arg = await reader.ReadLineAsync(token).ConfigureAwait(false);

                    await Current.Dispatcher.InvokeAsync(async () =>
                    {
                        if (Current.MainWindow is not MainWindow mainWindow)
                            return;

                        mainWindow.BringToForeground();
                        if (!string.IsNullOrEmpty(arg))
                        {
                            await mainWindow.LoadFileAsync(arg);
                        }
                    });
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    Services.GetRequiredService<IAlertService>().ShowError(
                        ex.Message,
                        LocalizationHelper.GetLocalizedValue<string>("Alert.AlertResources", "PipeServerError"));

                    await Task.Delay(500, token).ConfigureAwait(false);
                }
            }
        }

        /// <inheritdoc/>
        protected override async void OnStartup(System.Windows.StartupEventArgs e)
        {
            ApplyTheme();

            if (!EnsureSingleInstance(e.Args))
            {
                Shutdown();
                return;
            }

            _pipeCts = new CancellationTokenSource();

            _ = StartPipeServerAsync(_pipeCts.Token);

            var mainWindow = await InitializeMainWindowAsync();

            await HandleStartupFileArgsAsync(e.Args, mainWindow);

            base.OnStartup(e);
        }

        /// <inheritdoc/>
        protected override void OnExit(System.Windows.ExitEventArgs e)
        {
            _mutex?.Dispose();

            if (Services is ServiceProvider serviceProvider)
            {
                serviceProvider.Dispose();
            }

            _pipeCts?.Cancel();

            base.OnExit(e);
        }
    }
}
