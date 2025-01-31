
using Diagrammatist.Domain.Figures;

namespace Diagrammatist.Application.AppServices.Figures.Serializer
{
    /// <summary>
    /// An interface for figure serialization.
    /// </summary>
    public interface IFigureSerializer
    {
        /// <summary>
        /// Serializes <see cref="Figure"/> to array of bytes.
        /// </summary>
        /// <param name="figure">Target figure.</param>
        byte[] Serialize(Figure figure);
        /// <summary>
        /// Deserializes array of bytes to <see cref="Figure"/>
        /// </summary>
        /// <returns><see cref="Figure"/> instance.</returns>
        Figure? Deserialize(byte[] data);
    }
}
