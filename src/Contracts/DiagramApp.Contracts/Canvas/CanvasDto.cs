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
        /// Width of Invisible border.
        /// </summary>
        public int ImaginaryWidth { get; set; }
        /// <summary>
        /// Height of Invisible border.
        /// </summary>
        public int ImaginaryHeight { get; set; }
        /// <summary>
        /// Diagram settings.
        /// </summary>
        public required DiagramSettingsDto Settings { get; set; }
        /// <summary>
        /// Zoom parameter.
        /// </summary>
        public double Zoom { get; set; }
        /// <summary>
        /// Rotation parameter.
        /// </summary>
        public double Rotation { get; set; }
        /// <summary>
        /// Screen offset.
        /// Determines canvas position on the user's screen.
        /// </summary>
        public ScreenOffsetDto ScreenOffset { get; set; }
        /// <summary>
        /// Grid spacing parameter.
        /// </summary>
        public double GridSpacing { get; set; }
        /// <summary>
        /// Grid visible parameter.
        /// </summary>
        public bool IsGridVisible { get; set; }
        /// <summary>
        /// Figures.
        /// </summary>
        public List<FigureDto> Figures { get; set; } = [];
    }
}
