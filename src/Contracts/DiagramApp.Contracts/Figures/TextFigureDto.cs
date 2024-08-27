namespace DiagramApp.Contracts.Figures
{
    /// <summary>
    /// A text figure dto. Derived class from <see cref="FigureDto"/>.
    /// </summary>
    public class TextFigureDto : FigureDto
    {
        /// <summary>
        /// Gets or sets figure Text.
        /// </summary>
        /// <remarks>
        /// This property used to display text in figure UI.
        /// </remarks>
        public required string Text { get; set; }
        /// <summary>
        /// Gets or sets font size.
        /// </summary>
        /// <remarks>
        /// This property used to configure font size.
        /// </remarks>
        public required double FontSize { get; set; }
        /// <summary>
        /// Gets or sets figure outline condition.
        /// </summary>
        /// <remarks>
        /// This property indicates whether figure has outline.
        /// </remarks>
        public required bool HasOutline { get; set; }
        /// <summary>
        /// Gets or sets figure background condition,
        /// </summary>
        /// <remarks>
        /// This property indicates whether figure has background.
        /// </remarks>
        public required bool HasBackground { get; set; }
    }
}
