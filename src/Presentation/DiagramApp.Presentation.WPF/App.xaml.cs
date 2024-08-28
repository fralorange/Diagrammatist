using DiagramApp.Presentation.WPF.Views;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

using ApplicationEntity = System.Windows.Application;

namespace DiagramApp.Presentation.WPF
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
            var services = new ServiceCollection();

            services.AddStartupServices();
            services.AddViewModels();

            services.AddServices();
            services.AddRepositories();

            return services.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            Services.GetService<MainWindow>()?.Show();

            base.OnStartup(e);
        }
    }
}
