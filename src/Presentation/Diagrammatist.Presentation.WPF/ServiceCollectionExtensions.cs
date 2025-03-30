using Diagrammatist.Application.AppServices.Canvas.Serializer;
using Diagrammatist.Application.AppServices.Canvas.Services;
using Diagrammatist.Application.AppServices.Figures.Repositories;
using Diagrammatist.Application.AppServices.Figures.Serializer;
using Diagrammatist.Application.AppServices.Figures.Serializer.Configuration;
using Diagrammatist.Application.AppServices.Figures.Services;
using Diagrammatist.Infrastructure.DataAccess.Contexts.Figures.Repositories;
using Diagrammatist.Presentation.WPF.Core.Commands.Managers;
using Diagrammatist.Presentation.WPF.Core.Services.Connection;
using Diagrammatist.Presentation.WPF.Core.Models.Figures;
using Diagrammatist.Presentation.WPF.Core.Services.Alert;
using Diagrammatist.Presentation.WPF.Core.Services.Clipboard;
using Diagrammatist.Presentation.WPF.Core.Services.Clipboard.Figure;
using Diagrammatist.Presentation.WPF.ViewModels;
using Diagrammatist.Presentation.WPF.ViewModels.Components;
using Diagrammatist.Presentation.WPF.Views;
using Microsoft.Extensions.DependencyInjection;
using MvvmDialogs;

namespace Diagrammatist.Presentation.WPF
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
            services.AddSingleton<ActionViewModel>();

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
            services.AddTransient<IFigureSerializationService, FigureSerializationService>();
            #endregion
            #region Canvases
            services.AddTransient<ICanvasManipulationService, CanvasManipulationService>();
            services.AddTransient<ICanvasSerializationService, CanvasSerializationService>();
            #endregion

            #region Presentation
            services.AddTransient<IAlertService, AlertService>();
            services.AddTransient<IClipboardService<FigureModel>, FigureClipboardService>();
            services.AddTransient<IConnectionService, ConnectionService>();
            #endregion
            return services;
        }

        /// <summary>
        /// Add serializers as Singletons.
        /// </summary>
        /// <param name="services"></param>
        /// <returns>A reference to this <see cref="IServiceCollection"/> after the operation has completed.</returns>
        public static IServiceCollection AddSerializers(this IServiceCollection services)
        {
            services.AddSingleton<SerializationConfigurator>();

            services.AddSingleton(provider =>
            {
                var configurator = provider.GetRequiredService<SerializationConfigurator>();
                return configurator.Configure();
            });

            #region Canvases
            services.AddTransient<ICanvasSerializer, CanvasSerializer>();
            #endregion
            #region Figures
            services.AddTransient<IFigureSerializer, FigureSerializer>();
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
        /// Add managers.
        /// </summary>
        /// <param name="services"></param>
        /// <returns>A reference to this <see cref="IServiceCollection"/> after the operation has completed.</returns>
        public static IServiceCollection AddManagers(this IServiceCollection services)
        {
            services.AddSingleton<IUndoableCommandManager, UndoableCommandManager>();
            services.AddSingleton<ITrackableCommandManager, TrackableCommandManager>();

            return services;
        }
    }
}
