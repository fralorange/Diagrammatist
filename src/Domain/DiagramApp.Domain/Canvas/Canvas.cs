using DiagramApp.Domain.Canvas.Constants;
using DiagramApp.Domain.Figures;
using DiagramSettingsEntity = DiagramApp.Domain.DiagramSettings.DiagramSettings;

namespace DiagramApp.Domain.Canvas
{
    public class Canvas
    {
        /// <summary>
        /// Invisible border width which prevents user from scrolling into abyss...
        /// </summary>
        public int ImaginaryWidth { get; set; }
        /// <summary>
        /// Invisible border height which prevents user from scrolling into abyss...
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
        /// Controls parameter.
        /// </summary>
        public ControlsType Controls { get; set; } = CanvasControlsConstants.DefaultControls;
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
