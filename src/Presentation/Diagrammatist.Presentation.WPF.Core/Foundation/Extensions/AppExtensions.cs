using Diagrammatist.Presentation.WPF.Core.Models.ColorScheme;
using System.Windows;
using System.Windows.Media;
using ApplicationEnt = System.Windows.Application;

namespace Diagrammatist.Presentation.WPF.Core.Foundation.Extensions
{
    /// <summary>
    /// Extension methods for the <see cref="ApplicationEnt" /> type.
    /// </summary>
    public static class AppExtensions
    {
        /// <summary>
        /// Gets current app color scheme.
        /// </summary>
        /// <param name="application"></param>
        /// <returns><see cref="IEnumerable{T}"/> of <see cref="ColorSchemeEntry"/> if Colors.xaml exists, otherwise null.</returns>
        public static IEnumerable<ColorSchemeEntry>? GetCurrentColorScheme(this ApplicationEnt application)
        {
            var resourceDictionary = new ResourceDictionary
            {
                Source = new Uri("Resources/Styles/Colors.xaml", UriKind.Relative)
            };

            if (resourceDictionary.Keys.Count == 0)
            {
                return null;
            }

            return resourceDictionary.Keys
                .Cast<object>()
                .Where(key => resourceDictionary[key] is Color)
                .Select(key =>
                {
                    var color = (Color)resourceDictionary[key];
                    return new ColorSchemeEntry
                    {
                        Name = key.ToString() ?? string.Empty,
                        Hex = $"#{color.A:X2}{color.R:X2}{color.G:X2}{color.B:X2}"
                    };
                })
                .OrderBy(key => key.Name);
        }
    }
}
