using DiagramApp.Contracts.Canvas;
using DiagramApp.Infrastructure.ComponentRegistrar.Contexts.Figures.Mappers;
using DiagramApp.Infrastructure.ComponentRegistrar.Contexts.Settings.Mappers;
using CanvasEntity = DiagramApp.Domain.Canvas.Canvas;

namespace DiagramApp.Infrastructure.ComponentRegistrar.Contexts.Canvas.Mappers
{
    /// <summary>
    /// Canvas mapper extension.
    /// </summary>
    public static class CanvasMapperExtension
    {
        /// <summary>
        /// Map canvas to dto.
        /// </summary>
        /// <param name="canvas">Source.</param>
        /// <returns><see cref="CanvasDto"/></returns>
        public static CanvasDto ToDto(this CanvasEntity canvas)
        {
            return new CanvasDto
            {
                ImaginaryWidth = canvas.ImaginaryWidth,
                ImaginaryHeight = canvas.ImaginaryHeight,
                Settings = canvas.Settings.ToDto(),
                Zoom = canvas.Zoom,
                Rotation = canvas.Rotation,
                ScreenOffset = canvas.ScreenOffset.ToDto(),
                Figures = canvas.Figures.Select(figure => figure.ToDto()).ToList(),
            };
        }

        /// <summary>
        /// Map canvas dto to entity.
        /// </summary>
        /// <param name="canvas">Source.</param>
        /// <returns><see cref="CanvasEntity"/></returns>
        public static CanvasEntity ToEntity(this CanvasDto canvas)
        {
            return new CanvasEntity
            {
                ImaginaryWidth = canvas.ImaginaryWidth,
                ImaginaryHeight = canvas.ImaginaryHeight,
                Settings = canvas.Settings.ToEntity(),
                Zoom = canvas.Zoom,
                Rotation = canvas.Rotation,
                ScreenOffset = canvas.ScreenOffset.ToEntity(),
                Figures = canvas.Figures.Select(figure => figure.ToEntity()).ToList(),
            };
        }
    }
}
