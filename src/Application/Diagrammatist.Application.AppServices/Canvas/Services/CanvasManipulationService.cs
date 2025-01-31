using Diagrammatist.Application.AppServices.Canvas.Helpers;
using Diagrammatist.Domain.Canvas;
using System.Drawing;
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
        public void UpdateCanvas(CanvasEntity canvas, Size size)
        {
            var settings = canvas.Settings;

            settings.Width = size.Width;
            settings.Height = size.Height;

            CanvasBoundaryHelper.UpdateCanvasBounds(canvas);
        }

        /// <inheritdoc/>
        public void UpdateCanvas(CanvasEntity canvas, Color background)
        {
            var settings = canvas.Settings;

            settings.Background = background;
        }
    }
}
