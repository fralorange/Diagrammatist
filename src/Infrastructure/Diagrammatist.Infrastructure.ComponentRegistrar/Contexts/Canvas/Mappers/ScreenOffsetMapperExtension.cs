using DiagramApp.Contracts.Canvas;
using DiagramApp.Domain.Canvas;

namespace DiagramApp.Infrastructure.ComponentRegistrar.Contexts.Canvas.Mappers
{
    /// <summary>
    /// Screen offset mapper extension class.
    /// </summary>
    public static class ScreenOffsetMapperExtension
    {
        /// <summary>
        /// Map to Dto.
        /// </summary>
        /// <param name="screenOffset">Source.</param>
        /// <returns><see cref="ScreenOffsetDto"/></returns>
        public static ScreenOffsetDto ToDto(this ScreenOffset screenOffset)
        {
            return new ScreenOffsetDto
            {
                X = screenOffset.X,
                Y = screenOffset.Y,
            };
        }
        
        /// <summary>
        /// Map to Entity.
        /// </summary>
        /// <param name="screenOffset">Source.</param>
        /// <returns><see cref="ScreenOffset"/></returns>
        public static ScreenOffset ToEntity(this ScreenOffsetDto screenOffset)
        {
            return new ScreenOffset
            {
                X = screenOffset.X,
                Y = screenOffset.Y,
            };
        }
    }
}
