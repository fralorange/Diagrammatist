using Diagrammatist.Application.AppServices.Figures.Serializer.Witness;
using Diagrammatist.Domain.Figures;
using Diagrammatist.Domain.Figures.Special.Container;
using Diagrammatist.Domain.Figures.Special.Flowchart;
using Nerdbank.MessagePack;

namespace Diagrammatist.Application.AppServices.Figures.Serializer.Configuration
{
    /// <summary>
    /// Serialization configurator.
    /// </summary>
    public class SerializationConfigurator
    {
        /// <summary>
        /// Get required figure mappings.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DerivedTypeMapping> GetMappings()
        {
            var figureMapping = new DerivedShapeMapping<Figure>();
            figureMapping.Add<LineFigure, FigureWitness>(1);
            figureMapping.Add<ShapeFigure, FigureWitness>(2);
            figureMapping.Add<TextFigure, FigureWitness>(3);
            figureMapping.Add<ContainerFigure, FigureWitness>(4);
            figureMapping.Add<FlowchartFigure, FigureWitness>(5);

            return [figureMapping];
        }

        /// <summary>
        /// Creates and configures serializer.
        /// </summary>
        /// <returns>Configure <see cref="MessagePackSerializer"/>.</returns>
        public MessagePackSerializer CreateSerializer(IEnumerable<DerivedTypeMapping> additionalMappings)
        {
            var serializer = new MessagePackSerializer()
            {
                SerializeDefaultValues = SerializeDefaultValuesPolicy.Always,
            };

            var allMappings = new List<DerivedTypeMapping>();
            allMappings.AddRange(GetMappings());
            allMappings.AddRange(additionalMappings);

            return serializer with { DerivedTypeMappings = [.. serializer.DerivedTypeMappings, ..allMappings] };
        }
    }
}
