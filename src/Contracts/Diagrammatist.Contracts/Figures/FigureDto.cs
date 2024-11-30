using System.Drawing;

namespace Diagrammatist.Contracts.Figures
{
    /// <summary>
    /// A base class for figure dto objects.
    /// </summary>
    public abstract class FigureDto
    {
        /// <summary>
        /// Gets or sets figure name.
        /// </summary>
        /// <remarks>
        /// This property used to store figure name.
        /// </remarks>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets figure position by X-axis.
        /// </summary>
        /// <remarks>
        /// This property used to determine figure position by x-axis.
        /// </remarks>
        public double PosX { get; set; }
        /// <summary>
        /// Gets or sets figure position by Y-axis.
        /// </summary>
        /// <remarks>
        /// This property used to determine figure position by y-axis.
        /// </remarks>
        public double PosY { get; set; }
        /// <summary>
        /// Gets or sets figure rotation.
        /// </summary>
        /// <remarks>
        /// This property used to configure figure rotation.
        /// </remarks>
        public double Rotation { get; set; }
        /// <summary>
        /// Gets or sets figure Z index.
        /// </summary>
        /// <remarks>
        /// This property used to configure overlap order between figures.
        /// </remarks>
        public double ZIndex { get; set; }
        /// <summary>
        /// Gets or sets figure background color.
        /// </summary>
        /// <remarks>
        /// This property used to store figure background color.
        /// </remarks>
        public Color BackgroundColor { get; set; }
    }
}
