using CanvasEnt = Diagrammatist.Domain.Canvas.Canvas;

namespace Diagrammatist.Application.AppServices.Canvas.Services
{
    /// <summary>
    /// An interface for canvas serialization operations.
    /// </summary>
    public interface ICanvasSerializationService
    {
        /// <summary>
        /// Saves <see cref="CanvasEnt"/> to binary file.
        /// </summary>
        /// <param name="canvas">Target canvas.</param>
        /// <param name="filePath">File path.</param>
        void SaveCanvas(CanvasEnt canvas, string filePath);
        /// <summary>
        /// Loads <see cref="CanvasEnt"/> from binary file.
        /// </summary>
        /// <param name="filePath">File path.</param>
        /// <returns><see cref="CanvasEnt"/> instance.</returns>
        CanvasEnt? LoadCanvas(string filePath);
    }
}
