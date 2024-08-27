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
        /// Add startup services, e.g. <see cref="App"/> and <see cref="MainWindow"/> as Singletons.
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
        /// Add view models as Singletons.
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
    }
}
