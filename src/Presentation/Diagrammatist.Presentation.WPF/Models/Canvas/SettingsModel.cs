using CommunityToolkit.Mvvm.ComponentModel;
using System.Drawing;

namespace Diagrammatist.Presentation.WPF.Models.Canvas
{
    /// <summary>
    /// Diagram settings.
    /// </summary>
    public partial class SettingsModel : ObservableObject
    {
        /// <summary>
        /// Gets or sets diagram file name.
        /// </summary>
        /// <remarks>
        /// This property used to set file name.
        /// </remarks>
        [ObservableProperty]
        private string _fileName = string.Empty;
        /// <summary>
        /// Gets or sets diagram canvas width.
        /// </summary>
        /// <remarks>
        /// This property used to store diagram's canvas width.
        /// </remarks>
        [ObservableProperty]
        private int _width;
        /// <summary>
        /// Gets or sets diagram canvas height.
        /// </summary>
        /// <remarks>
        /// This property used to store diagram's canvas height.
        /// </remarks>
        [ObservableProperty]
        private int _height;
        /// <summary>
        /// Gets or sets diagram canvas background color.
        /// </summary>
        /// <remarks>
        /// This property used to store diagram background color.
        /// </remarks>
        [ObservableProperty]
        private Color _background;
        /// <summary>
        /// Gets or sets diagram type.
        /// </summary>
        /// <remarks>
        /// This property used to store diagram type.
        /// </remarks>
        public DiagramsModel Type { get; set; }
    }
}
