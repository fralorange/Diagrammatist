namespace Diagrammatist.Domain.Canvas
{
    /// <summary>
    /// Screen offset axis.
    /// </summary>
    public class ScreenOffset
    {
        /// <summary>
        /// Gets or sets X-axis.
        /// </summary>
        /// <remarks>
        /// This property used to determine screen offset position by X-axis.
        /// </remarks>
        public double X { get; set; } = 0;
        /// <summary>
        /// Gets or sets Y-axis.
        /// </summary>
        /// <remarks>
        /// This property used to determine screen offset position by Y-axis.
        /// </remarks>
        public double Y { get; set; } = 0;
    }
}
