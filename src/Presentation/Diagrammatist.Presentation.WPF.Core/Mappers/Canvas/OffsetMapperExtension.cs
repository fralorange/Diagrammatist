using Diagrammatist.Domain.Canvas;
using Diagrammatist.Presentation.WPF.Core.Models.Canvas;

namespace Diagrammatist.Presentation.WPF.Core.Mappers.Canvas
{
    /// <summary>
    /// Screen offset mapper extension class.
    /// </summary>
    internal static class OffsetMapperExtension
    {
        /// <summary>
        /// Map from domain to model.
        /// </summary>
        /// <param name="offset">Source.</param>
        /// <returns><see cref="OffsetModel"/></returns>
        public static OffsetModel ToModel(this Offset offset)
        {
            return new OffsetModel
            {
                X = offset.X,
                Y = offset.Y,
            };
        }

        /// <summary>
        /// Map from model to domain.
        /// </summary>
        /// <param name="offset">Source.</param>
        /// <returns><see cref="Offset"/></returns>
        public static Offset ToDomain(this OffsetModel offset)
        {
            return new Offset
            {
                X = offset.X,
                Y = offset.Y,
            };
        }
    }
}
