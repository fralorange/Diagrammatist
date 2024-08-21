namespace DiagramApp.Contracts.Settings
{
    /// <summary>
    /// Diagram settings dto.
    /// </summary>
    public class DiagramSettingsDto
    {
        /// <summary>
        /// Diagram file name.
        /// </summary>
        public required string FileName { get; set; }
        /// <summary>
        /// Diagram canvas width.
        /// </summary>
        public required int Width { get; set; }
        /// <summary>
        /// Diagram canvas height.
        /// </summary>
        public required int Height { get; set; }
        /// <summary>
        /// Diagram canvas background type.
        /// </summary>
        public required string Background { get; set; }
        /// <summary>
        /// Diagram type.
        /// </summary>
        public required string Type { get; set; }
    }
}
