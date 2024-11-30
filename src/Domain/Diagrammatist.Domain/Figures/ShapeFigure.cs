using Diagrammatist.Domain.Figures.Constants;

namespace Diagrammatist.Domain.Figures
{
    /// <summary>
    /// A shape figure class. Derived class from <see cref="Figure"/>.
    /// </summary>
    public class ShapeFigure : Figure
    {
        /// <summary>
        /// Gets or sets width.
        /// </summary>
        /// <remarks>
        /// This property used to store figure width.
        /// </remarks>
        public double Width { get; set; } = FigureManipulationConstants.DefaultWidth;
        /// <summary>
        /// Gets or sets height.
        /// </summary>
        /// <remarks>
        /// This property used to store figure height.
        /// </remarks>
        public double Height { get; set; } = FigureManipulationConstants.DefaultHeight;
        /// <summary>
        /// Gets or sets aspect ratio keep parameter.
        /// </summary>
        /// <remarks>
        /// This property is used to determine whether or not to keep the aspect ratio.
        /// </remarks>
        public bool KeepAspectRatio { get; set; } = ShapeFigureBoolConstants.DefaultAspectRatioParameter;
        /// <summary>
        /// Gets or sets collection of data.
        /// </summary>
        /// <remarks>
        /// This property used to store data.
        /// </remarks>
        public List<string> Data { get; set; } = [];
    }
}
