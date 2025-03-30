using Diagrammatist.Presentation.WPF.Core.Mappers.Figures;
using Diagrammatist.Presentation.WPF.Core.Models.Connection;
using ConnectionEntity = Diagrammatist.Domain.Connection.Connection;

namespace Diagrammatist.Presentation.WPF.Core.Mappers.Connection
{
    /// <summary>
    /// Connection mapper extension.
    /// </summary>
    public static class ConnectionMapperExtension
    {
        /// <summary>
        /// Map connection from domain to model.
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        public static ConnectionModel ToModel(this ConnectionEntity connection)
        {
            return new ConnectionModel
            {
                SourceMagneticPoint = connection.SourceMagneticPoint?.ToModel(),
                DestinationMagneticPoint = connection.DestinationMagneticPoint?.ToModel(),
                Line = connection.Line.ToModel(),
            };
        }

        /// <summary>
        /// Map connection from model to domain.
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        public static ConnectionEntity ToDomain(this ConnectionModel connection)
        {
            return new ConnectionEntity
            {
                SourceMagneticPoint = connection.SourceMagneticPoint?.ToDomain(),
                DestinationMagneticPoint = connection.DestinationMagneticPoint?.ToDomain(),
                Line = connection.Line.ToDomain(),
            };
        }
    }
}
