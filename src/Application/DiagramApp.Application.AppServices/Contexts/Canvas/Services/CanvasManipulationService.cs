using DiagramApp.Application.AppServices.Contexts.Canvas.Helpers;
using DiagramApp.Contracts.Canvas;
using DiagramApp.Domain.Settings;
using DiagramApp.Infrastructure.ComponentRegistrar.Contexts.Canvas.Mappers;
using DiagramApp.Infrastructure.ComponentRegistrar.Contexts.Settings.Mappers;
using CanvasEntity = DiagramApp.Domain.Canvas.Canvas;

namespace DiagramApp.Application.AppServices.Contexts.Canvas.Services
{
    /// <inheritdoc cref="ICanvasManipulationService"/>
    public class CanvasManipulationService : ICanvasManipulationService
    {
        /// <inheritdoc/>
        public CanvasDto CreateCanvas(DiagramSettings settings)
        {
            var canvas = new CanvasEntity
            {
                Settings = settings,
            };

            var dto = canvas.ToDto();

            CanvasBoundaryHelper.UpdateCanvasBounds(dto);

            return dto;
        }
    }
}
