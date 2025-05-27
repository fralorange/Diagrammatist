using System.Reflection;
using System.Text.RegularExpressions;

namespace Diagrammatist.Presentation.WPF.Core.Helpers
{
    /// <summary>
    /// A static class that provides helper methods for figure names.
    /// </summary>
    public static partial class FigureNameHelper
    {
        /// <summary>
        /// Generates a unique name based on the provided base name and a collection of existing names.
        /// </summary>
        /// <param name="baseName"></param>
        /// <param name="existingNames"></param>
        /// <returns></returns>
        public static string GetUniqueName(string baseName, IEnumerable<string>? existingNames)
        {
            if (existingNames == null)
                return baseName;

            var namesSet = existingNames.ToHashSet();
            if (!namesSet.Contains(baseName))
                return baseName;

            var match = SuffixRegex().Match(baseName);
            string coreName;
            int startIndex;

            if (match.Success && int.TryParse(match.Groups[2].Value, out var parsed))
            {
                coreName = match.Groups[1].Value;
                startIndex = parsed + 1;
            }
            else
            {
                coreName = baseName;
                startIndex = 1;
            }

            string candidate;
            int index = startIndex;

            do
            {
                candidate = $"{coreName} {index}";
                index++;
            } while (namesSet.Contains(candidate));

            return candidate;
        }

        /// <summary>
        /// Retrieves the translated name for a given key from the localization resources.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetTranslatedName(string key)
        {
            var resourcesAssembly = Assembly.GetCallingAssembly().GetName().Name;
            return LocalizationHelper.GetLocalizedValue<string>(
                resourcesAssembly ?? string.Empty,
                "Figures.FiguresResources",
                key);
        }

        [GeneratedRegex(@"^(.*) (\d+)$", RegexOptions.Compiled)]
        private static partial Regex SuffixRegex();
    }
}
