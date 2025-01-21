using Diagrammatist.Presentation.WPF.Views;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using System.Windows;
using WPFLocalizeExtension.Engine;
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

            ConfigureCulture();
        }

        private static ServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            services.AddStartupServices();
            services.AddViewModels();

            services.AddServices();
            services.AddSerializers();
            services.AddRepositories();

            services.AddDialogServices();

            services.AddManagers();

            return services.BuildServiceProvider();
        }

        private static void ConfigureCulture()
        {
            var culture = new CultureInfo(WPF.Properties.Settings.Default.Culture);

            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.CurrentCulture = culture;

            CultureInfo.DefaultThreadCurrentUICulture = culture;
            CultureInfo.CurrentUICulture = culture;

            LocalizeDictionary.Instance.SetCultureCommand.Execute(culture.ToString());
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            Services.GetService<MainWindow>()?.Show();

            base.OnStartup(e);
        }
    }
}
