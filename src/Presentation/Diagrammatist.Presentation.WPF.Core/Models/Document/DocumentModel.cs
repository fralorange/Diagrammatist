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

        private Dictionary<string, object?> _payloads = [];

        /// <summary>
        /// Sets payload data.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        public void SetPayload(string key, object? data) => _payloads[key] = data;
        /// <summary>
        /// Sets payload data as new dictionary.
        /// </summary>
        /// <param name="dictionary"></param>
        public void SetPayloads(Dictionary<string, object?> dictionary) => _payloads = dictionary;
        /// <summary>
        /// Gets payload data.
        /// </summary>
        public T? GetPayloadData<T>(string key) where T : class
        {
            if (_payloads.TryGetValue(key, out var data))
                return data as T;
            return null;
        }
        /// <summary>
        /// Gets read-only payload.
        /// </summary>
        public IReadOnlyDictionary<string, object?> Payloads => _payloads;
    }
}
