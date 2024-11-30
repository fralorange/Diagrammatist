using DiagramApp.Contracts.Settings;
using DiagramApp.Contracts.Figures;

namespace DiagramApp.Contracts.Canvas
{
    /// <summary>
    /// Canvas DTO.
    /// </summary>
    public class CanvasDto
    {
        /// <summary>
        /// Gets or sets width of invisible border.
        /// </summary>
        /// <remarks>
        /// This property used to prevent user from scrolling by X-axis in to the abyss...
        /// </remarks>
        public int ImaginaryWidth { get; set; }
        /// <summary>
        /// Gets or sets height of invisible border.
        /// </summary>
        /// <remarks>
        /// This property used to prevent user from scrolling by Y-axis in to the abyss...
        /// </remarks>
        public int ImaginaryHeight { get; set; }
        /// <summary>
        /// Gets or sets diagram settings.
        /// </summary>
        /// <remarks>
        /// This property used to configure canvas.
        /// </remarks>
        public required DiagramSettingsDto Settings { get; set; }
        /// <summary>
        /// Gets or sets zoom parameter.
        /// </summary>
        /// <remarks>
        /// This property used to set canvas scale.
        /// </remarks>
        public double Zoom { get; set; }
        /// <summary>
        /// Gets or sets screen offset.
        /// </summary>
        /// <remarks>
        /// This property used to determine canvas position in window.
        /// </remarks>
        public ScreenOffsetDto ScreenOffset { get; set; }
        /// <summary>
        /// Gets or sets figure collection.
        /// </summary>
        /// <remarks>
        /// This property used to set drawing context for canvas.
        /// </remarks>
        public List<FigureDto> Figures { get; set; } = [];
    }
}
