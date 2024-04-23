using CommunityToolkit.Maui;
using DiagramApp.Application.AppServices.Services;
using DiagramApp.Client.ViewModels;
using DiagramApp.Client.Views;
using LocalizationResourceManager.Maui;
using Microsoft.Extensions.Logging;

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
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton<MainView>();
            builder.Services.AddSingleton<MainViewModel>();
            builder.Services.AddTransient<IToolboxService, ToolboxService>();
            builder.Services.AddTransientPopup<AboutPopupView, AboutPopupViewModel>();
            builder.Services.AddTransientPopup<ChangeDiagramSizePopupView, ChangeDiagramSizePopupViewModel>();
            builder.Services.AddTransientPopup<NewDiagramPopupView, NewDiagramPopupViewModel>();
            return builder.Build();
        }
    }
}
