using Diagrammatist.Domain.Figures;

namespace Diagrammatist.Application.AppServices.Figures.Services
{
    /// <summary>
    /// An interface for figure serialization operations. 
    /// </summary>
    public interface IFigureSerializationService
    {
        /// <summary>
        /// Serializes <see cref="Figure"/> object.
        /// </summary>
        /// <param name="figure">Target figure.</param>
        /// <returns>An array of bytes.</returns>
        byte[] Serialize(Figure figure);
        /// <summary>
        /// Deserializes <see cref="Array"/> of <see cref="byte"/>.
        /// </summary>
        /// <param name="data">Target data.</param>
        /// <returns>A <see cref="Figure"/> or <see cref="null"/> in case of failure.</returns>
        Figure? Deserialize(byte[] data);
    }
}
