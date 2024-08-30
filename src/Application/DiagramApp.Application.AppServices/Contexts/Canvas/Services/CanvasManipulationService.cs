using DiagramApp.Application.AppServices.Contexts.Canvas.Helpers;
using DiagramApp.Contracts.Canvas;
using DiagramApp.Contracts.Settings;
using DiagramApp.Infrastructure.ComponentRegistrar.Contexts.Canvas.Mappers;
using DiagramApp.Infrastructure.ComponentRegistrar.Contexts.Settings.Mappers;
using CanvasEntity = DiagramApp.Domain.Canvas.Canvas;

namespace DiagramApp.Application.AppServices.Contexts.Canvas.Services
{
    /// <summary>
    /// A class that implements <see cref="ICanvasManipulationService"/>. A canvas manipulation service.
    /// </summary>
    public class CanvasManipulationService : ICanvasManipulationService
    {
        /// <inheritdoc/>
        public Task<CanvasDto> CreateCanvasAsync(DiagramSettingsDto settings)
        {
            var canvas = new CanvasEntity
            {
                Settings = settings.ToEntity(),
            };

            var dto = canvas.ToDto();

            CanvasBoundaryHelper.UpdateCanvasBounds(dto);

            return Task.FromResult(dto);
        }

        /// <inheritdoc/>
        public void UpdateCanvas(CanvasDto canvas, DiagramSettingsDto settings)
        {
            canvas.Settings = settings;

            CanvasBoundaryHelper.UpdateCanvasBounds(canvas);
        }
    }
}
