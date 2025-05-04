using Diagrammatist.Presentation.WPF.Core.Foundation.Extensions;
using Diagrammatist.Presentation.WPF.Core.Mappers.Connection;
using Diagrammatist.Presentation.WPF.Core.Mappers.Figures;
using Diagrammatist.Presentation.WPF.Core.Models.Canvas;
using CanvasEntity = Diagrammatist.Domain.Canvas.Canvas;

namespace Diagrammatist.Presentation.WPF.Core.Mappers.Canvas
{
    /// <summary>
    /// Canvas mapper extension.
    /// </summary>
    public static class CanvasMapperExtension
    {
        /// <summary>
        /// Map canvas from domain to model.
        /// </summary>
        /// <param name="canvas">Source.</param>
        /// <returns><see cref="CanvasModel"/></returns>
        public static CanvasModel ToModel(this CanvasEntity canvas)
        {
            var figureModels = canvas.Figures
                .Select(figure => figure.ToModel())
                .ToObservableCollection();

            var connectionModels = canvas.Connections
                .Select(connection => connection.ToModel(figureModels))
                .ToObservableCollection();

            return new CanvasModel
            {
                ImaginaryWidth = canvas.ImaginaryWidth,
                ImaginaryHeight = canvas.ImaginaryHeight,
                Settings = canvas.Settings.ToModel(),
                Zoom = canvas.Zoom,
                Offset = canvas.Offset.ToModel(),
                Figures = figureModels,
                Connections = connectionModels,
            };
        }

        /// <summary>
        /// Map canvas from model to domain.
        /// </summary>
        /// <param name="canvas">Source.</param>
        /// <returns><see cref="CanvasEntity"/></returns>
        public static CanvasEntity ToDomain(this CanvasModel canvas)
        {
            var figureDomains = canvas.Figures
                .Select(figure => figure.ToDomain())
                .ToList();

            var connectionDomains = canvas.Connections
                .Select(connection => connection.ToDomain(figureDomains))
                .ToList();

            return new CanvasEntity
            {
                ImaginaryWidth = canvas.ImaginaryWidth,
                ImaginaryHeight = canvas.ImaginaryHeight,
                Settings = canvas.Settings.ToDomain(),
                Zoom = canvas.Zoom,
                Offset = canvas.Offset.ToDomain(),
                Figures = figureDomains,
                Connections = connectionDomains,
            };
        }
    }
}
