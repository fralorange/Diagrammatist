using CommunityToolkit.Mvvm.Messaging;
using Diagrammatist.Presentation.WPF.Core.Messaging.Messages;
using System.Collections.ObjectModel;
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
        /// Changes the current application theme.
        /// </summary>
        /// <param name="application">The application instance.</param>
        /// <param name="themeName">The name of the theme.</param>
        public static void ChangeTheme(this ApplicationEnt application, string themeName)
        {
            var mergedDictionaries = ApplicationEnt.Current.Resources.MergedDictionaries;

            var currentTheme = mergedDictionaries
                .FirstOrDefault(dict => dict.Source?.ToString().Contains("Colors") == true);
            var currentBrushes = mergedDictionaries
                .FirstOrDefault(dict => dict.Source?.ToString().Contains("Brushes") == true);
            var currentVectors = mergedDictionaries
                .FirstOrDefault(dict => dict.Source?.ToString().Contains("Vectors") == true);

            if (currentTheme?.Source?.ToString().Contains(themeName) == true || currentTheme is null || currentBrushes is null || currentVectors is null)
                return;

            ReplaceResource(mergedDictionaries, currentTheme, $"Resources/Styles/Colors/{themeName}.xaml");
            ReplaceResource(mergedDictionaries, currentBrushes, "Resources/Styles/Brushes.xaml");
            ReplaceResource(mergedDictionaries, currentVectors, "Resources/Styles/Vectors.xaml");

            WeakReferenceMessenger.Default.Send(new ThemeChangedMessage(themeName));
        }

        private static void ReplaceResource(Collection<ResourceDictionary> mergedDictionaries, ResourceDictionary oldResource, string newResourcePath)
        {
            var index = mergedDictionaries.IndexOf(oldResource);

            if (index < 0) 
                return;

            mergedDictionaries.RemoveAt(index);
            mergedDictionaries.Insert(index, new ResourceDictionary { Source = new Uri(newResourcePath, UriKind.Relative) });
        }
    }
}