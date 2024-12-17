using Diagrammatist.Application.AppServices.Canvas.Serializer.Witness;
using Nerdbank.MessagePack;
using CanvasEnt = Diagrammatist.Domain.Canvas.Canvas;

namespace Diagrammatist.Application.AppServices.Canvas.Serializer
{
    /// <summary>
    /// A class that implements <see cref="ICanvasSerializer"/>. 
    /// Serializes <see cref="CanvasEnt"/> domains.
    /// </summary>
    public class CanvasSerializer : ICanvasSerializer
    {
        private readonly MessagePackSerializer _serializer;

        public CanvasSerializer(MessagePackSerializer serializer)
        {
            _serializer = serializer;
        }

        /// <inheritdoc/>
        public void SaveToFile(CanvasEnt canvas, string filePath)
        {
            var data = _serializer.Serialize<CanvasEnt, CanvasWitness>(canvas);
            File.WriteAllBytes(filePath, data);
        }

        /// <inheritdoc/>
        public CanvasEnt? LoadFromFile(string filePath)
        {
            var data = File.ReadAllBytes(filePath);
            return _serializer.Deserialize<CanvasEnt, CanvasWitness>(data);
        }
    }
}
