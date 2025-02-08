using Diagrammatist.Application.AppServices.Figures.Serializer.Witness;
using Diagrammatist.Domain.Figures;
using Nerdbank.MessagePack;

namespace Diagrammatist.Application.AppServices.Figures.Serializer.Configuration
{
    /// <summary>
    /// Serialization configurator.
    /// </summary>
    public class SerializationConfigurator
    {
        /// <summary>
        /// Configures figures through runtime registration.
        /// </summary>
        /// <param name="serializer"></param>
        internal void ConfigureFiguresMapping(MessagePackSerializer serializer)
        {
            KnownSubTypeMapping<Figure> mapping = new();
            mapping.Add<LineFigure, FigureWitness>(1);
            mapping.Add<ShapeFigure, FigureWitness>(2);
            mapping.Add<TextFigure, FigureWitness>(3);

            serializer.RegisterKnownSubTypes(mapping);
        }

        /// <summary>
        /// Configures serializer.
        /// </summary>
        /// <returns>Configure <see cref="MessagePackSerializer"/>.</returns>
        public MessagePackSerializer Configure()
        {
            var serializer = new MessagePackSerializer()
            {
                SerializeDefaultValues = SerializeDefaultValuesPolicy.Always,
            };

            ConfigureFiguresMapping(serializer);

            return serializer;
        }
    }
}
