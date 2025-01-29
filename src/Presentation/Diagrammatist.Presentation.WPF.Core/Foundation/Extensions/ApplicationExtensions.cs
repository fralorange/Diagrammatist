using System.Windows;
using ApplicationEnt = System.Windows.Application;

namespace Diagrammatist.Presentation.WPF.Core.Foundation.Extensions
{
    /// <summary>
    /// Extension methods for the <see cref="ApplicationEnt" /> type.
    /// </summary>
    public static class ApplicationExtensions
    {
        /// <summary>
        /// Changes current app theme.
        /// </summary>
        /// <param name="application"></param>
        /// <param name="themeName">Theme name.</param>
        public static void ChangeTheme(this ApplicationEnt application, string themeName)
        {
            var currentTheme = ApplicationEnt.Current.Resources.MergedDictionaries
                .FirstOrDefault(dict => dict.Source != null && dict.Source.ToString().Contains("Colors"));

            if (currentTheme != null)
            {
                ApplicationEnt.Current.Resources.MergedDictionaries.Remove(currentTheme);
            }

            var newTheme = new ResourceDictionary { Source = new Uri($"Resources/Styles/Colors/{themeName}.xaml", UriKind.Relative) };

            ApplicationEnt.Current.Resources.MergedDictionaries.Insert(0, newTheme);
        }
    }
}