using System.Reflection;
using WPFLocalizeExtension.Extensions;

namespace Diagrammatist.Presentation.WPF.Core.Helpers
{
    /// <summary>
    /// A class that helps to easily localize values.
    /// </summary>
    public static class LocalizationHelper
    {
        /// <summary>
        /// Gets localized value from dictionary.
        /// </summary>
        /// <typeparam name="T">Output type.</typeparam>
        /// <param name="resource">Resource parameter.</param>
        /// <param name="key">Key parameter.</param>
        /// <returns>Localized <typeparamref name="T"/> value.</returns>
        public static T GetLocalizedValue<T>(string resource, string key)
        {
            return LocExtension.GetLocalizedValue<T>($"{Assembly.GetCallingAssembly().GetName().Name}:Resources.Localization.{resource}:{key}");
        }
    }
}
