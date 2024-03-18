using CommunityToolkit.Maui;
using DiagramApp.Client.ViewModels;
using DiagramApp.Client.Views;
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
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton<MainView>();
            builder.Services.AddTransientPopup<NewDiagramPopupView, NewDiagramPopupViewmodel>();
            return builder.Build();
        }
    }
}
