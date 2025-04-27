using Diagrammatist.Presentation.WPF.Core.Foundation.Extensions;
using Diagrammatist.Presentation.WPF.Views;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using ApplicationEntity = System.Windows.Application;

namespace Diagrammatist.Presentation.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public sealed partial class App : ApplicationEntity
    {
        /// <summary>
        /// Gets the current <see cref="App"/> instance in use
        /// </summary>
        public new static App Current => (App)ApplicationEntity.Current;

        /// <summary>
        /// Gets the <see cref="IServiceProvider"/> instance to resolve application services.
        /// </summary>
        public IServiceProvider Services { get; }

        public App()
        {
            Services = ConfigureServices();
        }

        private static ServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddStartupServices()
                .AddViewModels()
                .AddServices()
                .AddFacades()
                .AddSerializers()
                .AddRepositories()
                .AddDialogServices()
                .AddManagers()
                .AddCulture()
                .AddMappers()
                .BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            Current.ChangeTheme(WPF.Properties.Settings.Default.Theme);

            Services.GetService<MainWindow>()?.Show();

            base.OnStartup(e);
        }
    }
}
