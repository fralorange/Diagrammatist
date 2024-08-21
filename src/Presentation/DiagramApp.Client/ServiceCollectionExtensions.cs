using CommunityToolkit.Maui;
using DiagramApp.Application.AppServices.Contexts.Clipboard.Services;
using DiagramApp.Application.AppServices.Contexts.File.Services;
using DiagramApp.Application.AppServices.Contexts.Toolbox.Services;
using DiagramApp.Application.AppServices.Services.Toolbox;
using DiagramApp.Client.Mappers.Canvas;
using DiagramApp.Client.Mappers.Figure;
using DiagramApp.Client.ViewModels;
using DiagramApp.Client.Views;

namespace DiagramApp.Client
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddViews(this IServiceCollection services)
        {
            services.AddSingleton<MainView>();

            return services;
        }

        public static IServiceCollection AddViewModels(this IServiceCollection services)
        {
            services.AddSingleton<MainViewModel>();

            return services;
        }

        public static IServiceCollection AddMappers(this IServiceCollection services)
        {
            services.AddSingleton<IFigureMapper, FigureMapper>();
            services.AddSingleton<ICanvasMapper, CanvasMapper>();

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddTransient<IToolboxService, ToolboxService>();
            services.AddTransient<IFileService, FileService>();
            services.AddTransient<IClipboardService, ClipboardService>();

            return services;
        }

        public static IServiceCollection AddPopups(this IServiceCollection services)
        {
            services.AddTransientPopup<AboutPopupView, AboutPopupViewModel>();
            services.AddTransientPopup<ChangeDiagramSizePopupView, ChangeDiagramSizePopupViewModel>();
            services.AddTransientPopup<NewDiagramPopupView, NewDiagramPopupViewModel>();

            return services;
        }
    }
}
