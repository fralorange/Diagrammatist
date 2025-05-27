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

        /// <summary>
        /// Gets localized value from dictionary using assembly name, resource and key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assemblyName"></param>
        /// <param name="resource"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T GetLocalizedValue<T>(string assemblyName, string resource, string key)
        {
            return LocExtension.GetLocalizedValue<T>($"{assemblyName}:Resources.Localization.{resource}:{key}");
        }
    }
}
