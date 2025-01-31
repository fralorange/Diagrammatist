using Diagrammatist.Domain.Canvas;
using Diagrammatist.Presentation.WPF.Core.Models.Canvas;

namespace Diagrammatist.Presentation.WPF.Core.Mappers.Canvas
{
    /// <summary>
    /// Diagram settings mapper extension.
    /// </summary>
    public static class SettingsMapperExtension
    {
        /// <summary>
        /// Map diagram settings from domain to model.
        /// </summary>
        /// <param name="settings">Source.</param>
        /// <returns><see cref="SettingsModel"/></returns>
        public static SettingsModel ToModel(this Settings settings)
        {
            return new SettingsModel
            {
                FileName = settings.FileName,
                Width = settings.Width,
                Height = settings.Height,
                Background = settings.Background,
                Type = (DiagramsModel)Enum.Parse(typeof(DiagramsModel), settings.Type.ToString()),
            };
        }

        /// <summary>
        /// Map diagram settings from model to domain.
        /// </summary>
        /// <param name="settings">Source.</param>
        /// <returns><see cref="Settings"/></returns>
        public static Settings ToDomain(this SettingsModel settings)
        {
            return new Settings
            {
                FileName = settings.FileName,
                Width = settings.Width,
                Height = settings.Height,
                Background = settings.Background,
                Type = (Diagrams)Enum.Parse(typeof(Diagrams), settings.Type.ToString()),
            };
        }
    }
}
