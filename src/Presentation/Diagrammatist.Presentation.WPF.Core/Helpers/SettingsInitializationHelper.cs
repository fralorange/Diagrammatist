using Diagrammatist.Presentation.WPF.Core.Models.Canvas;

namespace Diagrammatist.Presentation.WPF.Core.Helpers
{
    /// <summary>
    /// Helps to initialize diagram settings in presentation layer.
    /// </summary>
    public static class SettingsInitializationHelper
    {
        /// <summary>
        /// Initializes new default diagram settings.
        /// </summary>
        /// <returns><see cref="DiagramSettingsDto"/> with default values.</returns>
        public static SettingsModel InitializeDefaultSettings()
        {
            return new SettingsModel();
        }
    }
}
