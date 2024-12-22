using Diagrammatist.Application.AppServices.Figures.Serializer.Witness;
using Diagrammatist.Domain.Figures;
using Nerdbank.MessagePack;

namespace Diagrammatist.Application.AppServices.Figures.Serializer
{
    /// <summary>
    /// A class that implements <see cref="IFigureSerializer"/>. 
    /// Serializes <see cref="Figure"/> domains.
    /// </summary>
    public class FigureSerializer : IFigureSerializer
    {
        private readonly MessagePackSerializer _serializer;

        public FigureSerializer(MessagePackSerializer serializer)
        {
            _serializer = serializer;
        }

        /// <inheritdoc/>
        public byte[] Serialize(Figure figure)
        {
            return _serializer.Serialize<Figure, FigureWitness>(figure);
        }

        /// <inheritdoc/>
        public Figure? Deserialize(byte[] data)
        {
            return _serializer.Deserialize<Figure, FigureWitness>(data);
        }
    }
}
