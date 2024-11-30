using DiagramApp.Contracts.Settings;
using DiagramApp.Domain.Settings;

namespace DiagramApp.Infrastructure.ComponentRegistrar.Contexts.Settings.Mappers
{
    /// <summary>
    /// Diagram settings mapper extension.
    /// </summary>
    public static class DiagramSettingsMapperExtension
    {
        /// <summary>
        /// Map diagram settings to dto.
        /// </summary>
        /// <param name="diagramSettings">Source.</param>
        /// <returns><see cref="DiagramSettingsDto"/></returns>
        public static DiagramSettingsDto ToDto(this DiagramSettings diagramSettings)
        {
            return new DiagramSettingsDto
            {
                FileName = diagramSettings.FileName,
                Width = diagramSettings.Width,
                Height = diagramSettings.Height,
                Background = diagramSettings.Background,
                Type = (DiagramTypeDto)Enum.Parse(typeof(DiagramTypeDto), diagramSettings.Type.ToString()),
            };
        }

        /// <summary>
        /// Map diagram settings dto to entity.
        /// </summary>
        /// <param name="diagramSettings">Source.</param>
        /// <returns><see cref="DiagramSettings"/></returns>
        public static DiagramSettings ToEntity(this DiagramSettingsDto diagramSettings)
        {
            return new DiagramSettings
            {
                FileName = diagramSettings.FileName,
                Width = diagramSettings.Width,
                Height = diagramSettings.Height,
                Background = diagramSettings.Background,
                Type = (DiagramType)Enum.Parse(typeof(DiagramType), diagramSettings.Type.ToString()),
            };
        }
    }
}
