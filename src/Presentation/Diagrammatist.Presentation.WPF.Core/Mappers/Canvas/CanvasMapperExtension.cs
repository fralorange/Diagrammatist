using Diagrammatist.Presentation.WPF.Core.Mappers.Connection;
using Diagrammatist.Presentation.WPF.Core.Mappers.Figures;
using Diagrammatist.Presentation.WPF.Core.Models.Canvas;
using Diagrammatist.Presentation.WPF.Core.Models.Connection;
using Diagrammatist.Presentation.WPF.Core.Models.Figures;
using System.Collections.ObjectModel;
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
            return new CanvasModel
            {
                ImaginaryWidth = canvas.ImaginaryWidth,
                ImaginaryHeight = canvas.ImaginaryHeight,
                Settings = canvas.Settings.ToModel(),
                Zoom = canvas.Zoom,
                Offset = canvas.Offset.ToModel(),
                Figures = new ObservableCollection<FigureModel>(canvas.Figures.Select(figure => figure.ToModel())),
                Connections = new ObservableCollection<ConnectionModel>(canvas.Connections.Select(connection => connection.ToModel()))
            };
        }

        /// <summary>
        /// Map canvas from model to domain.
        /// </summary>
        /// <param name="canvas">Source.</param>
        /// <returns><see cref="CanvasEntity"/></returns>
        public static CanvasEntity ToDomain(this CanvasModel canvas)
        {
            return new CanvasEntity
            {
                ImaginaryWidth = canvas.ImaginaryWidth,
                ImaginaryHeight = canvas.ImaginaryHeight,
                Settings = canvas.Settings.ToDomain(),
                Zoom = canvas.Zoom,
                Offset = canvas.Offset.ToDomain(),
                Figures = canvas.Figures.Select(figure => figure.ToDomain()).ToList(),
                Connections = canvas.Connections.Select(connection => connection.ToDomain()).ToList()
            };
        }
    }
}
