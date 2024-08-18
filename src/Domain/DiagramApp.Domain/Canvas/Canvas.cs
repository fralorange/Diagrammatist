using DiagramApp.Domain.Canvas.Constants;
using DiagramApp.Domain.Figures;
using DiagramSettingsEntity = DiagramApp.Domain.DiagramSettings.DiagramSettings;

namespace DiagramApp.Domain.Canvas
{
    public class Canvas
    {
        /// <summary>
        /// Width of Invisible border which prevents user from scrolling in to the abyss...
        /// </summary>
        public int ImaginaryWidth { get; set; }
        /// <summary>
        /// Height of Invisible border which prevents user from scrolling in to the abyss...
        /// </summary>
        public int ImaginaryHeight { get; set; }
        /// <summary>
        /// Diagram settings.
        /// </summary>
        public required DiagramSettingsEntity Settings { get; set; }
        /// <summary>
        /// Zoom parameter.
        /// </summary>
        public double Zoom { get; set; } = CanvasZoomConstants.DefaultZoom;
        /// <summary>
        /// Rotation parameter.
        /// </summary>
        public double Rotation { get; set; } = CanvasRotationConstants.DefaultRotation;
        /// <summary>
        /// Screen offset.
        /// Determines canvas position on the user's screen.
        /// </summary>
        public ScreenOffset ScreenOffset { get; set; } = new();
        /// <summary>
        /// Grid spacing parameter.
        /// </summary>
        public double GridSpacing { get; set; } = CanvasGridConstants.DefaultGridSpacing;
        /// <summary>
        /// Grid visible parameter.
        /// </summary>
        public bool IsGridVisible { get; set; } = CanvasGridConstants.DefaultGridVisible;
        /// <summary>
        /// Drawing context for canvas.
        /// </summary>
        public FigureCollection FigureCollection { get; set; } = new();
    }
}
