using DiagramApp.Contracts.Settings;
using DiagramApp.Domain.Settings;
using DiagramApp.Infrastructure.ComponentRegistrar.Contexts.Settings.Mappers;

namespace DiagramApp.Application.AppServices.Contexts.Settings.Helpers
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
        public static DiagramSettingsDto InitializeDefaultSettings()
        {
            return new DiagramSettings().ToDto();
        }
    }
}
