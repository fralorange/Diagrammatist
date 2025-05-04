using Diagrammatist.Presentation.WPF.Core.Helpers;
using System.Windows;

namespace Diagrammatist.Presentation.WPF.Views.Splash
{
    /// <summary>
    /// A class that derives from <see cref="Window"/>.
    /// This class represents splash screen window.
    /// </summary>
    public partial class SplashScreen : Window
    {
        public SplashScreen()
        {
            InitializeComponent();

            Loaded += (s, e) =>
            {
                Topmost = true;
                Topmost = false;
            };
        }
        
        /// <summary>
        /// Updates splash screen progress.
        /// </summary>
        /// <param name="percent"></param>
        /// <param name="stage"></param>
        public void UpdateProgress(int percent, string stage)
        {
            progressBar.Value = percent;
            progressText.Text = $"{LocalizationHelper.GetLocalizedValue<string>("Splash.SplashResources", stage)}";
        }
    }
}
