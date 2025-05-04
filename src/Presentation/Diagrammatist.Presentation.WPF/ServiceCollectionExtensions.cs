using Diagrammatist.Application.AppServices.Document.Serializer;
using Diagrammatist.Application.AppServices.Document.Services;
using Diagrammatist.Application.AppServices.Figures.Repositories;
using Diagrammatist.Application.AppServices.Figures.Serializer;
using Diagrammatist.Application.AppServices.Figures.Serializer.Configuration;
using Diagrammatist.Application.AppServices.Figures.Services;
using Diagrammatist.Application.AppServices.Simulator.Serializer.Configuration;
using Diagrammatist.Infrastructure.DataAccess.Contexts.Figures.Repositories;
using Diagrammatist.Presentation.WPF.Core.Facades.Canvas;
using Diagrammatist.Presentation.WPF.Core.Factories.Figures.Line;
using Diagrammatist.Presentation.WPF.Core.Managers.Command;
using Diagrammatist.Presentation.WPF.Core.Managers.Tabs;
using Diagrammatist.Presentation.WPF.Core.Mappers.Document;
using Diagrammatist.Presentation.WPF.Core.Models.Figures;
using Diagrammatist.Presentation.WPF.Core.Services.Alert;
using Diagrammatist.Presentation.WPF.Core.Services.Canvas.Interaction;
using Diagrammatist.Presentation.WPF.Core.Services.Canvas.Manipulation;
using Diagrammatist.Presentation.WPF.Core.Services.Clipboard;
using Diagrammatist.Presentation.WPF.Core.Services.Clipboard.Figure;
using Diagrammatist.Presentation.WPF.Core.Services.Connection;
using Diagrammatist.Presentation.WPF.Core.Services.Figure.Manipulation;
using Diagrammatist.Presentation.WPF.Core.Services.Figure.Placement;
using Diagrammatist.Presentation.WPF.Core.Services.Settings;
using Diagrammatist.Presentation.WPF.Core.Services.Sound;
using Diagrammatist.Presentation.WPF.Properties;
using Diagrammatist.Presentation.WPF.Simulator.Mappers;
using Diagrammatist.Presentation.WPF.ViewModels;
using Diagrammatist.Presentation.WPF.ViewModels.Components;
using Diagrammatist.Presentation.WPF.Views;
using Diagrammatist.Presentation.WPF.Views.Splash;
using Microsoft.Extensions.DependencyInjection;
using MvvmDialogs;
using System.Globalization;
using WPFLocalizeExtension.Engine;

namespace Diagrammatist.Presentation.WPF
{
    /// <summary>
    /// A static class that represents service collection extensions.
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
            services.AddTransient<SplashScreen>();

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
            services.AddSingleton<ActionViewModel>();

            return services;
        }

        /// <summary>
        /// Adds services as Transient.
        /// </summary>
        /// <param name="services"></param>
        /// <returns>A reference to this <see cref="IServiceCollection"/> after the operation has completed.</returns>
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            #region Figures
            services.AddTransient<IFigureService, FigureService>();
            services.AddTransient<IFigureSerializationService, FigureSerializationService>();
            services.AddTransient<IFigureManipulationService, FigureManipulationService>();
            services.AddTransient<IFigurePlacementService, FigurePlacementService>();
            #endregion
            #region Canvases
            services.AddTransient<ICanvasManipulationService, CanvasManipulationService>();
            services.AddTransient<ICanvasInteractionService, CanvasInteractionService>();
            #endregion
            #region Document
            services.AddTransient<IDocumentSerializationService, DocumentSerializationService>();
            #endregion

            #region Presentation
            services.AddTransient<IAlertService, AlertService>();
            services.AddTransient<IClipboardService<FigureModel>, FigureClipboardService>();
            services.AddTransient<IConnectionService, ConnectionService>();
            services.AddTransient<ISoundService, SoundService>();
            services.AddSingleton<IUserSettingsService>(sp =>
            {
                var storage = new PropertiesSettingsStorage();
                return new UserSettingsService(storage);
            });
            #endregion
            return services;
        }

        /// <summary>
        /// Adds facade as Transient.
        /// </summary>
        /// <param name="services"></param>
        /// <returns>A reference to this <see cref="IServiceCollection"/> after the operation has completed.</returns>
        public static IServiceCollection AddFacades(this IServiceCollection services)
        {
            services.AddTransient<ICanvasServiceFacade, CanvasServiceFacade>();

            return services;
        }

        /// <summary>
        /// Adds serializers as Singletons.
        /// </summary>
        /// <param name="services"></param>
        /// <returns>A reference to this <see cref="IServiceCollection"/> after the operation has completed.</returns>
        public static IServiceCollection AddSerializers(this IServiceCollection services)
        {
            services.AddSingleton<SerializationConfigurator>();
            services.AddSingleton<SimulationSerializationConfigurator>();

            services.AddSingleton(provider =>
            {
                var figures = provider.GetRequiredService<SerializationConfigurator>();
                var sim = provider.GetRequiredService<SimulationSerializationConfigurator>();

                var simMappings = sim.GetMappings();
                return figures.CreateSerializer(simMappings);
            });

            #region Figures
            services.AddTransient<IFigureSerializer, FigureSerializer>();
            #endregion
            #region Document
            services.AddTransient<IDocumentSerializer, DocumentSerializer>();
            #endregion
            return services;
        }

        /// <summary>
        /// Adds repositories as Transient.
        /// </summary>
        /// <param name="services"></param>
        /// <returns>A reference to this <see cref="IServiceCollection"/> after the operation has completed.</returns>
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddTransient<IFigureRepository, JsonFigureRepository>();

            return services;
        }

        /// <summary>
        /// Adds dialog services as singleton.
        /// </summary>
        /// <param name="services"></param>
        /// <returns>A reference to this <see cref="IServiceCollection"/> after the operation has completed.</returns>
        public static IServiceCollection AddDialogServices(this IServiceCollection services)
        {
            services.AddSingleton<IDialogService, DialogService>();

            return services;
        }

        /// <summary>
        /// Adds factories as Transient.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddFactories(this IServiceCollection services)
        {
            #region Figures
            services.AddTransient<ILineFactory, LineFactory>();
            #endregion

            return services;
        }

        /// <summary>
        /// Adds managers.
        /// </summary>
        /// <param name="services"></param>
        /// <returns>A reference to this <see cref="IServiceCollection"/> after the operation has completed.</returns>
        public static IServiceCollection AddManagers(this IServiceCollection services)
        {
            services.AddSingleton<IUndoableCommandManager, UndoableCommandManager>();
            services.AddSingleton<ITrackableCommandManager, TrackableCommandManager>();
            services.AddSingleton<IDocumentTabsManager, DocumentTabsManager>();

            return services;
        }

        /// <summary>
        /// Adds culture.
        /// </summary>
        /// <param name="services"></param>
        /// <returns>A reference to this <see cref="IServiceCollection"/> after the operation has completed.</returns>
        public static IServiceCollection AddCulture(this IServiceCollection services)
        {
            var culture = new CultureInfo(Properties.Settings.Default.Culture);

            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.CurrentCulture = culture;

            CultureInfo.DefaultThreadCurrentUICulture = culture;
            CultureInfo.CurrentUICulture = culture;

            LocalizeDictionary.Instance.SetCultureCommand.Execute(culture.ToString());

            return services;
        }

        /// <summary>
        /// Adds mappers.
        /// </summary>
        /// <param name="services"></param>
        /// <returns>A reference to this <see cref="IServiceCollection"/> after the operation has completed.</returns>
        public static IServiceCollection AddMappers(this IServiceCollection services)
        {
            #region Simulator
            DocumentMapperExtension.RegisterPayloadMapper(new SimulationDocumentMapper());
            #endregion

            return services;
        }
    }
}
