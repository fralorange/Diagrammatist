using Diagrammatist.Application.AppServices.Canvas.Serializer;
using CanvasEnt = Diagrammatist.Domain.Canvas.Canvas;

namespace Diagrammatist.Application.AppServices.Canvas.Services
{
    /// <summary>
    /// A class that implements <see cref="ICanvasSerializationService"/>.
    /// Used to serialize and deserialize <see cref="CanvasEnt"/> objects.
    /// </summary>
    public class CanvasSerializationService : ICanvasSerializationService
    {
        private readonly ICanvasSerializer _canvasSerializer;

        /// <summary>
        /// Initializes <see cref="CanvasSerializationService"/>.
        /// </summary>
        /// <param name="canvasSerializer">Canvas serializer.</param>
        public CanvasSerializationService(ICanvasSerializer canvasSerializer)
        {
            _canvasSerializer = canvasSerializer;
        }

        /// <inheritdoc/>
        public void SaveCanvas(CanvasEnt canvas, string filePath)
        {
            _canvasSerializer.SaveToFile(canvas, filePath);
        }

        /// <inheritdoc/>
        public CanvasEnt? LoadCanvas(string filePath)
        {
            return _canvasSerializer.LoadFromFile(filePath);
        }
    }
}
