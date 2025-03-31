using Diagrammatist.Domain.Figures;
using Diagrammatist.Domain.Figures.Magnetic;
using Diagrammatist.Presentation.WPF.Core.Models.Figures;
using Diagrammatist.Presentation.WPF.Core.Models.Figures.Magnetic;

namespace Diagrammatist.Presentation.WPF.Core.Mappers.Figures
{
    /// <summary>
    /// Magnetic point mapper extension.
    /// </summary>
    public static class MagneticPointMapperExtension
    {
        /// <summary>
        /// Map magnetic point from domain to model.
        /// </summary>
        /// <param name="magneticPoint"></param>
        /// <param name="figures"></param>
        /// <returns></returns>
        public static MagneticPointModel ToModel(this MagneticPoint magneticPoint, ICollection<FigureModel> figures)
        {
            return new MagneticPointModel
            {
                Owner = figures.OfType<ShapeFigureModel>().FirstOrDefault(shape => magneticPoint.Owner.Id == shape.Id)!,
                Position = new(magneticPoint.PosX, magneticPoint.PosY),
            };
        }

        /// <summary>
        /// Map magnetic point from model to domain.
        /// </summary>
        /// <param name="magneticPoint"></param>
        /// <param name="figures"></param>
        /// <returns></returns>
        public static MagneticPoint ToDomain(this MagneticPointModel magneticPoint, ICollection<Figure> figures)
        {
            return new MagneticPoint
            {
                Owner = figures.OfType<ShapeFigure>().FirstOrDefault(shape => magneticPoint.Owner.Id == shape.Id)!,
                PosX = magneticPoint.Position.X,
                PosY = magneticPoint.Position.Y,
            };
        }
    }
}
