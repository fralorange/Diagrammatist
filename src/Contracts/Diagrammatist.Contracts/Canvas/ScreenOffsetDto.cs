namespace DiagramApp.Contracts.Canvas
{
    /// <summary>
    /// Screen offset dto.
    /// </summary>
    public class ScreenOffsetDto
    {
        /// <summary>
        /// Gets or sets X-axis.
        /// </summary>
        /// <remarks>
        /// This property used to determine screen offset position by X-axis.
        /// </remarks>
        public required double X { get; set; }
        /// <summary>
        /// Gets or sets Y-axis.
        /// </summary>
        /// <remarks>
        /// This property used to determine screen offset position by Y-axis.
        /// </remarks>
        public required double Y { get; set; }
    }
}
