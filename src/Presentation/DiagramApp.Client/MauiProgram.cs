using CommunityToolkit.Maui;
using DiagramApp.Client.Controls;
using DiagramApp.Client.Platforms.Windows.Handlers;
using LocalizationResourceManager.Maui;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;

namespace DiagramApp.Client
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .UseLocalizationResourceManager(settings =>
                {
                    settings.RestoreLatestCulture(true);
                    settings.AddResource(Resources.Localization.AppResources.ResourceManager);
                })
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                })
                .ConfigureLifecycleEvents(events =>
                {
#if WINDOWS
                    events.AddWindows(windowsLifecycleBuilder =>
                    {
                        windowsLifecycleBuilder.OnWindowCreated(window =>
                        {
                            var handle = WinRT.Interop.WindowNative.GetWindowHandle(window);
                            var id = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(handle);
                            var appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(id);

                            var serviceProvider = builder.Services.BuildServiceProvider();
                            var localizationResourceManager = serviceProvider.GetService<ILocalizationResourceManager>();
                            var windowClosingEventHandler = new WindowClosingEventHandler(localizationResourceManager!);

                            appWindow.Closing += windowClosingEventHandler.OnWindowClosing;
                        });
                    });
#endif
                })
                .ConfigureMauiHandlers(handlers =>
                {
                    handlers.AddHandler<ExtendedPolyline, ExtendedPolylineHandler>();
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif
            builder.Services.AddViews();
            builder.Services.AddViewModels();
            builder.Services.AddMappers();
            builder.Services.AddServices();
            builder.Services.AddPopups();

            return builder.Build();
        }
    }
}
