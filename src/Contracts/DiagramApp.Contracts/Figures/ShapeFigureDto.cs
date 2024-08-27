namespace DiagramApp.Contracts.Figures
{
    /// <summary>
    /// A shape figure dto. Derived class from <see cref="FigureDto"/>.
    /// </summary>
    public class ShapeFigureDto : FigureDto
    {
        /// <summary>
        /// Gets or sets width.
        /// </summary>
        /// <remarks>
        /// This property used to store figure width.
        /// </remarks>
        public required double Width { get; set; }
        /// <summary>
        /// Gets or sets height.
        /// </summary>
        /// <remarks>
        /// This property used to store figure height.
        /// </remarks>
        public required double Height { get; set; }
    }
}
