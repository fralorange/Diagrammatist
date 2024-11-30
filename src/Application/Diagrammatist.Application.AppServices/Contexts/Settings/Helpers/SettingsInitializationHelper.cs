using Diagrammatist.Contracts.Settings;
using Diagrammatist.Domain.Settings;
using Diagrammatist.Infrastructure.ComponentRegistrar.Contexts.Settings.Mappers;

namespace Diagrammatist.Application.AppServices.Contexts.Settings.Helpers
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
