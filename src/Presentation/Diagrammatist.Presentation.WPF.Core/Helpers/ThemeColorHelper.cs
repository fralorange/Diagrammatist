using System.Windows.Media;

namespace Diagrammatist.Presentation.WPF.Core.Helpers
{
    /// <summary>
    /// A static class that provides methods to get theme colors based on conditions.
    /// </summary>
    public static class ThemeColorHelper
    {
        /// <summary>
        /// Gets the background color based on a condition.
        /// </summary>
        /// <returns></returns>
        public static Color GetBackgroundColor()
        {
            return (Color)System.Windows.Application.Current.Resources["AppEditorColor"];
        }
    }
}
