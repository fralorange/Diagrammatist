using Diagrammatist.Application.AppServices.Canvas.Helpers;
using Diagrammatist.Domain.Canvas;
using CanvasEntity = Diagrammatist.Domain.Canvas.Canvas;

namespace Diagrammatist.Application.AppServices.Canvas.Services
{
    /// <summary>
    /// A class that implements <see cref="ICanvasManipulationService"/>. A canvas manipulation service.
    /// </summary>
    public class CanvasManipulationService : ICanvasManipulationService
    {
        /// <inheritdoc/>
        public Task<CanvasEntity> CreateCanvasAsync(Settings settings)
        {
            var canvas = new CanvasEntity { Settings = settings };

            CanvasBoundaryHelper.UpdateCanvasBounds(canvas);

            return Task.FromResult(canvas);
        }

        /// <inheritdoc/>
        public void UpdateCanvasSettings(CanvasEntity canvas, Settings settings)
        {
            CanvasBoundaryHelper.UpdateCanvasBounds(canvas);
        }
    }
}
