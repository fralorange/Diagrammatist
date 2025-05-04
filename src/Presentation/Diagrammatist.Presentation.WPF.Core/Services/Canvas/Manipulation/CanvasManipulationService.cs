using Diagrammatist.Presentation.WPF.Core.Helpers;
using Diagrammatist.Presentation.WPF.Core.Models.Canvas;
using System.Windows;
using System.Windows.Media;

namespace Diagrammatist.Presentation.WPF.Core.Services.Canvas.Manipulation
{
    /// <summary>
    /// A class that implements <see cref="ICanvasManipulationService"/>. A canvas manipulation service.
    /// </summary>
    public class CanvasManipulationService : ICanvasManipulationService
    {
        /// <inheritdoc/>
        public Task<CanvasModel> CreateCanvasAsync(SettingsModel settings)
        {
            var canvas = new CanvasModel { Settings = settings };

            CanvasBoundaryHelper.UpdateCanvasBounds(canvas);

            return Task.FromResult(canvas);
        }

        /// <inheritdoc/>
        public void UpdateCanvas(CanvasModel canvas, Size size)
        {
            var settings = canvas.Settings;

            settings.Width = ((int)size.Width);
            settings.Height = ((int)size.Height);

            CanvasBoundaryHelper.UpdateCanvasBounds(canvas);
        }

        /// <inheritdoc/>
        public void UpdateCanvas(CanvasModel canvas, Color background)
        {
            var settings = canvas.Settings;

            settings.Background = background;
        }

        /// <inheritdoc/>
        public void UpdateCanvas(CanvasModel canvas, DiagramsModel type)
        {
            var settings = canvas.Settings;

            settings.Type = type;
        }
    }
}
