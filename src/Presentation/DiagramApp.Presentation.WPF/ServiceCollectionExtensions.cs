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
            services.AddSingleton<App>();
            services.AddSingleton<MainWindow>();

            return services;
        }
    }
}
