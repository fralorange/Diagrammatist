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
        /// <summary>
        /// Gets or sets aspect ratio keep parameter.
        /// </summary>
        /// <remarks>
        /// This property is used to determine whether or not to keep the aspect ratio.
        /// </remarks>
        public required bool KeepAspectRatio { get; set; }
        /// <summary>
        /// Gets or sets collection of data.
        /// </summary>
        /// <remarks>
        /// This property used to store data.
        /// </remarks>
        public required List<string> Data { get; set; } = [];
    }
}
