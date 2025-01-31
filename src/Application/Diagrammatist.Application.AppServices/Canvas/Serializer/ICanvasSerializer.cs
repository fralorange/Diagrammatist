using CanvasEnt = Diagrammatist.Domain.Canvas.Canvas;

namespace Diagrammatist.Application.AppServices.Canvas.Serializer
{
    /// <summary>
    /// An interface for canvas serialization.
    /// </summary>
    public interface ICanvasSerializer
    {
        /// <summary>
        /// Saves <see cref="CanvasEnt"/> to binary file.
        /// </summary>
        /// <param name="canvas">Target canvas.</param>
        /// <param name="filePath">File path.</param>
        void SaveToFile(CanvasEnt canvas, string filePath);
        /// <summary>
        /// Loads <see cref="CanvasEnt"/> from binary file.
        /// </summary>
        /// <param name="filePath">File path.</param>
        /// <returns><see cref="CanvasEnt"/> instance.</returns>
        CanvasEnt? LoadFromFile(string filePath);
    }
}
