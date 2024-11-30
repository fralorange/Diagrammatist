using Diagrammatist.Domain.Canvas.Constants;
using Diagrammatist.Domain.Figures;
using DiagramSettingsEntity = Diagrammatist.Domain.Settings.DiagramSettings;

namespace Diagrammatist.Domain.Canvas
{
    /// <summary>
    /// Canvas class.
    /// </summary>
    public class Canvas
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
        public required DiagramSettingsEntity Settings { get; set; }
        /// <summary>
        /// Gets or sets zoom parameter.
        /// </summary>
        /// <remarks>
        /// This property used to set canvas scale.
        /// </remarks>
        public double Zoom { get; set; } = CanvasZoomConstants.DefaultZoom;
        /// <summary>
        /// Gets or sets screen offset.
        /// </summary>
        /// <remarks>
        /// This property used to determine canvas position in window.
        /// </remarks>
        public ScreenOffset ScreenOffset { get; set; } = new();
        /// <summary>
        /// Gets or sets figure collection.
        /// </summary>
        /// <remarks>
        /// This property used to set drawing context for canvas.
        /// </remarks>
        public List<Figure> Figures { get; set; } = [];
    }
}
