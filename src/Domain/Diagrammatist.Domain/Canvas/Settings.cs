using Diagrammatist.Domain.Canvas.Constants;
using System.Drawing;

namespace Diagrammatist.Domain.Canvas
{
    /// <summary>
    /// Diagram settings.
    /// </summary>
    public class Settings
    {
        /// <summary>
        /// Gets or sets diagram file name.
        /// </summary>
        /// <remarks>
        /// This property used to set file name.
        /// </remarks>
        public string FileName { get; set; } = SettingsConstants.DefaultFileName;
        /// <summary>
        /// Gets or sets diagram canvas width.
        /// </summary>
        /// <remarks>
        /// This property used to store diagram's canvas width.
        /// </remarks>
        public int Width { get; set; } = SettingsConstants.DefaultWidth;
        /// <summary>
        /// Gets or sets diagram canvas height.
        /// </summary>
        /// <remarks>
        /// This property used to store diagram's canvas height.
        /// </remarks>
        public int Height { get; set; } = SettingsConstants.DefaultHeight;
        /// <summary>
        /// Gets or sets diagram canvas background color.
        /// </summary>
        /// <remarks>
        /// This property used to store diagram background color.
        /// </remarks>
        public Color Background { get; set; } = SettingsConstants.DefaultBackground;
        /// <summary>
        /// Gets or sets diagram type.
        /// </summary>
        /// <remarks>
        /// This property used to store diagram type.
        /// </remarks>
        public Diagrams Type { get; set; } = SettingsConstants.DefaultType;
    }
}
