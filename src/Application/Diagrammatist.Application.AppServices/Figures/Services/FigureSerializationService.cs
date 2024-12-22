using Diagrammatist.Application.AppServices.Figures.Serializer;
using Diagrammatist.Domain.Figures;

namespace Diagrammatist.Application.AppServices.Figures.Services
{
    /// <summary>
    /// A class that implements <see cref="IFigureSerializationService"/>.
    /// Used to serialize and deserialize <see cref="Figure"/> objects.
    /// </summary>
    public class FigureSerializationService : IFigureSerializationService
    {
        private readonly IFigureSerializer _figureSerializer;

        public FigureSerializationService(IFigureSerializer figureSerializer)
        {
            _figureSerializer = figureSerializer;
        }

        /// <inheritdoc/>
        public byte[] Serialize(Figure figure)
        {
            return _figureSerializer.Serialize(figure);
        }

        /// <inheritdoc/>
        public Figure? Deserialize(byte[] data)
        {
            return _figureSerializer.Deserialize(data);
        }
    }
}
