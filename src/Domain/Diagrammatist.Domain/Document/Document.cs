namespace Diagrammatist.Domain.Document
{
    /// <summary>
    /// A class that represents composite data source.
    /// </summary>
    public class Document
    {
        /// <summary>
        /// Gets or sets canvas data.
        /// </summary>
        public required Canvas.Canvas Canvas { get; set; }
        /// <summary>
        /// Gets or sets payload data, if it exists.
        /// </summary>
        public Dictionary<string, IPayloadData> Payloads { get; set; } = [];
    }
}
