using DiagramApp.Application.AppServices.Contexts.Canvas.Services;
using DiagramApp.Application.AppServices.Contexts.Figures.Repositories;
using DiagramApp.Application.AppServices.Contexts.Figures.Services;
using DiagramApp.Infrastructure.DataAccess.Contexts.Figures.Repositories;
using DiagramApp.Presentation.WPF.Framework.Commands.Manager;
using DiagramApp.Presentation.WPF.ViewModels;
using DiagramApp.Presentation.WPF.ViewModels.Components;
using DiagramApp.Presentation.WPF.Views;
using Microsoft.Extensions.DependencyInjection;
using MvvmDialogs;

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

        /// <summary>
        /// Add services (from application layer) as Transient.
        /// </summary>
        /// <param name="services"></param>
        /// <returns>A reference to this <see cref="IServiceCollection"/> after the operation has completed.</returns>
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            #region Figures
            services.AddTransient<IFigureService, FigureService>();
            #endregion
            #region Canvases
            services.AddTransient<ICanvasManipulationService, CanvasManipulationService>();
            #endregion
            return services;
        }

        /// <summary>
        /// Add repositories as Transient.
        /// </summary>
        /// <param name="services"></param>
        /// <returns>A reference to this <see cref="IServiceCollection"/> after the operation has completed.</returns>
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddTransient<IFigureRepository, JsonFigureRepository>();

            return services;
        }

        /// <summary>
        /// Add dialog services as singleton.
        /// </summary>
        /// <param name="services"></param>
        /// <returns>A reference to this <see cref="IServiceCollection"/> after the operation has completed.</returns>
        public static IServiceCollection AddDialogServices(this IServiceCollection services)
        {
            services.AddSingleton<IDialogService, DialogService>();

            return services;
        }

        /// <summary>
        /// Add managers as singleton.
        /// </summary>
        /// <param name="services"></param>
        /// <returns>A reference to this <see cref="IServiceCollection"/> after the operation has completed.</returns>
        public static IServiceCollection AddManagers(this IServiceCollection services)
        {
            services.AddSingleton<IUndoableCommandManager, UndoableCommandManager>();

            return services;
        }
    }
}
