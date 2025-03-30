using Diagrammatist.Domain.Figures.Magnetic;
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
        /// <returns></returns>
        public static MagneticPointModel ToModel(this MagneticPoint magneticPoint)
        {
            return new MagneticPointModel
            {
                Owner = magneticPoint.Owner.ToModel(),
                Position = new(magneticPoint.PosX, magneticPoint.PosY),
            };
        }

        /// <summary>
        /// Map magnetic point from model to domain.
        /// </summary>
        /// <param name="magneticPoint"></param>
        /// <returns></returns>
        public static MagneticPoint ToDomain(this MagneticPointModel magneticPoint)
        {
            return new MagneticPoint
            {
                Owner = magneticPoint.Owner.ToDomain(),
                PosX = magneticPoint.Position.X,
                PosY = magneticPoint.Position.Y,
            };
        }
    }
}
