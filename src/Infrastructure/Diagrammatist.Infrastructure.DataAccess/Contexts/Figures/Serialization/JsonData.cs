namespace Diagrammatist.Infrastructure.DataAccess.Contexts.Figures.Serialization
{
    /// <summary>
    /// A json data about figure.
    /// </summary>
    public class JsonData
    {
        /// <summary>
        /// Gets type.
        /// </summary>
        /// <remarks>
        /// This property used to configure figure type.
        /// </remarks>
        public required string Type { get; init; }
        /// <summary>
        /// Gets value.
        /// </summary>
        /// <remarks>
        /// This property used to store figure type related properties.
        /// </remarks>
        public required string Value { get; init; }
    }
}
