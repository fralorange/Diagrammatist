using Diagrammatist.Presentation.WPF.Core.Models.Canvas;

namespace Diagrammatist.Presentation.WPF.Core.Models.Document
{
    /// <summary>
    /// A class that represents model of document.
    /// </summary>
    public class DocumentModel
    {
        /// <summary>
        /// Gets or sets canvas model.
        /// </summary>
        public required CanvasModel Canvas { get; set; }

        private Dictionary<string, object?> _payload = [];

        /// <summary>
        /// Sets payload data.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        public void SetPayload(string key, object? data) => _payload[key] = data;
        /// <summary>
        /// Gets payload data.
        /// </summary>
        public T? GetPayloadData<T>(string key) where T : class
        {
            if (_payload.TryGetValue(key, out var data))
                return data as T;
            return null;
        }
        /// <summary>
        /// Gets read-only payload.
        /// </summary>
        public IReadOnlyDictionary<string, object?> Payload => _payload;
    }
}
