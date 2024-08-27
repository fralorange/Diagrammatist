namespace DiagramApp.Contracts.Settings
{
    /// <summary>
    /// Diagram settings dto.
    /// </summary>
    public class DiagramSettingsDto
    {
        /// <summary>
        /// Gets or sets diagram file name.
        /// </summary>
        /// <remarks>
        /// This property used to set file name.
        /// </remarks>
        public required string FileName { get; set; }
        /// <summary>
        /// Gets or sets diagram canvas width.
        /// </summary>
        /// <remarks>
        /// This property used to store diagram's canvas width.
        /// </remarks>
        public required int Width { get; set; }
        /// <summary>
        /// Gets or sets diagram canvas height.
        /// </summary>
        /// <remarks>
        /// This property used to store diagram's canvas height.
        /// </remarks>
        public required int Height { get; set; }
        /// <summary>
        /// Gets or sets diagram canvas background type.
        /// </summary>
        /// <remarks>
        /// This property used to store diagram background type.
        /// </remarks>
        public required string Background { get; set; }
        /// <summary>
        /// Gets or sets diagram type.
        /// </summary>
        /// <remarks>
        /// This property used to store diagram type.
        /// </remarks>
        public required string Type { get; set; }
    }
}
