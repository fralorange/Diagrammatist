using DiagramApp.Application.AppServices.Contexts.Figures.Repositories;
using DiagramApp.Application.AppServices.Contexts.Figures.Services;
using DiagramApp.Infrastructure.DataAccess.Contexts.Figures.Repositories;
using DiagramApp.Presentation.WPF.ViewModels;
using DiagramApp.Presentation.WPF.Views;
using Microsoft.Extensions.DependencyInjection;

namespace DiagramApp.Presentation.WPF
{
    /// <summary>
    /// Service collection extensions.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds startup services, e.g. <see cref="App"/> and <see cref="MainWindow"/> as Singletons.
        /// </summary>
        /// <param name="services"></param>
        /// <returns>A reference to this <see cref="IServiceCollection"/> after the operation has completed.</returns>
        public static IServiceCollection AddStartupServices(this IServiceCollection services)
        {
            services.AddSingleton<MainWindow>();
            services.AddSingleton<MainViewModel>();

            return services;
        }

        /// <summary>
        /// Adds view models as Singletons.
        /// </summary>
        /// <param name="services"></param>
        /// <returns>A reference to this <see cref="IServiceCollection"/> after the operation has completed.</returns>
        public static IServiceCollection AddViewModels(this IServiceCollection services)
        {
            services.AddSingleton<CanvasViewModel>();
            services.AddSingleton<FiguresViewModel>();
            services.AddSingleton<ObjectTreeViewModel>();
            services.AddSingleton<PropertiesViewModel>();
            services.AddSingleton<TabsViewModel>();
            services.AddSingleton<ToolbarViewModel>();

            return services;
        }

        /// <summary>
        /// Adds services as Scoped.
        /// </summary>
        /// <param name="services"></param>
        /// <returns>A reference to this <see cref="IServiceCollection"/> after the operation has completed.</returns>
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IFigureService, FigureService>();

            return services;
        }

        /// <summary>
        /// Adds repositories as Scoped.
        /// </summary>
        /// <param name="services"></param>
        /// <returns>A reference to this <see cref="IServiceCollection"/> after the operation has completed.</returns>
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IFigureRepository, JsonFigureRepository>();

            return services;
        }
    }
}
