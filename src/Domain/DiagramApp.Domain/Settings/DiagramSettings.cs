using DiagramApp.Domain.Settings.Constants;

namespace DiagramApp.Domain.Settings
{
    /// <summary>
    /// Diagram settings.
    /// </summary>
    public class DiagramSettings
    {
        /// <summary>
        /// Diagram file name.
        /// </summary>
        public string FileName { get; set; } = DiagramSettingsConstants.DefaultFileName;
        /// <summary>
        /// Diagram canvas width.
        /// </summary>
        public int Width { get; set; } = DiagramSettingsConstants.DefaultWidth;
        /// <summary>
        /// Diagram canvas height.
        /// </summary>
        public int Height { get; set; } = DiagramSettingsConstants.DefaultHeight;
        /// <summary>
        /// Diagram canvas background type.
        /// </summary>
        public BackgroundType Background { get; set; } = DiagramSettingsConstants.DefaultBackground;
        /// <summary>
        /// Diagram type.
        /// </summary>
        public DiagramType Type { get; set; } = DiagramSettingsConstants.DefaultType;
    }
}
