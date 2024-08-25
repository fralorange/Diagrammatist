using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DiagramApp.Presentation.WPF
{
    /// <summary>
    /// Main program.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Program startup.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    services.AddStartupServices();
                })
                .Build();

            var app = host.Services.GetService<App>();

            app?.Run();
        }
    }
}
