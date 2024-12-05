using Diagrammatist.Domain.Canvas;

namespace Diagrammatist.Application.AppServices.Canvas.Helpers
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
        public static Settings InitializeDefaultSettings()
        {
            return new Settings();
        }
    }
}
