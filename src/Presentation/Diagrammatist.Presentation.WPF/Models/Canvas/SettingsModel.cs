using CommunityToolkit.Mvvm.ComponentModel;
using System.Drawing;

namespace Diagrammatist.Presentation.WPF.Models.Canvas
{
    /// <summary>
    /// Diagram settings.
    /// </summary>
    public class SettingsModel : ObservableObject
    {
        /// <summary>
        /// Gets or sets diagram file name.
        /// </summary>
        /// <remarks>
        /// This property used to set file name.
        /// </remarks>
        public string FileName { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets diagram canvas width.
        /// </summary>
        /// <remarks>
        /// This property used to store diagram's canvas width.
        /// </remarks>
        public int Width { get; set; }
        /// <summary>
        /// Gets or sets diagram canvas height.
        /// </summary>
        /// <remarks>
        /// This property used to store diagram's canvas height.
        /// </remarks>
        public int Height { get; set; }
        /// <summary>
        /// Gets or sets diagram canvas background color.
        /// </summary>
        /// <remarks>
        /// This property used to store diagram background color.
        /// </remarks>
        public Color Background { get; set; }
        /// <summary>
        /// Gets or sets diagram type.
        /// </summary>
        /// <remarks>
        /// This property used to store diagram type.
        /// </remarks>
        public DiagramsModel Type { get; set; }
    }
}
