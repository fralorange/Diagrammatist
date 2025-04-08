using Diagrammatist.Domain.Figures;
using Diagrammatist.Presentation.WPF.Core.Mappers.Figures;
using Diagrammatist.Presentation.WPF.Core.Models.Connection;
using Diagrammatist.Presentation.WPF.Core.Models.Figures;
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
        /// <param name="figures"></param>
        /// <returns></returns>
        public static ConnectionModel ToModel(this ConnectionEntity connection, ICollection<FigureModel> figures)
        {
            return new ConnectionModel
            {
                SourceMagneticPoint = connection.SourceMagneticPoint?.ToModel(figures),
                DestinationMagneticPoint = connection.DestinationMagneticPoint?.ToModel(figures),
                Line = figures.OfType<LineFigureModel>().FirstOrDefault(line => connection.Line.Id == line.Id)!
            };
        }

        /// <summary>
        /// Map connection from model to domain.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="figures"></param>
        /// <returns></returns>
        public static ConnectionEntity ToDomain(this ConnectionModel connection, ICollection<Figure> figures)
        {
            return new ConnectionEntity
            {
                SourceMagneticPoint = connection.SourceMagneticPoint?.ToDomain(figures),
                DestinationMagneticPoint = connection.DestinationMagneticPoint?.ToDomain(figures),
                Line = figures.OfType<LineFigure>().FirstOrDefault(line => connection.Line.Id == line.Id)!
            };
        }
    }
}
