namespace Diagrammatist.Infrastructure.DataAccess.Contexts.Figures.Serialization
{
    /// <summary>
    /// A json figure.
    /// </summary>
    public class JsonFigure
    {
        /// <summary>
        /// Gets json's figure name.
        /// </summary>
        /// <remarks>
        /// This property used to store figure name.
        /// </remarks>
        public required string Name { get; init; }
        /// <summary>
        /// Gets json's figure category.
        /// </summary>
        /// <remarks>
        /// This property used to store figure category.
        /// </remarks>
        public required string Category { get; init; }
        /// <summary>
        /// Gets json's figure data.
        /// </summary>
        /// <remarks>
        /// This property used to configure figure.
        /// </remarks>
        public required JsonData Data { get; init; }
    }
}
