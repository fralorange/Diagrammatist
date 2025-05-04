using Diagrammatist.Presentation.WPF.Core.Models.Connection;
using Diagrammatist.Presentation.WPF.Core.Models.Figures;
using System.Windows;

namespace Diagrammatist.Presentation.WPF.Core.Services.Canvas.Interaction
{
    /// <summary>
    /// An interface for interacting with canvas items.
    /// </summary>
    public interface ICanvasInteractionService
    {
        /// <summary>
        /// Moves figure.
        /// </summary>
        /// <param name="figure">Target figure.</param>
        /// <param name="initPos"></param>
        /// <param name="oldPos">Old position.</param>
        /// <param name="newPos">New position.</param>
        /// <param name="connections">Connections that figure may have.</param>
        void MoveFigure(FigureModel figure, Point initPos, Point oldPos, Point newPos, ICollection<ConnectionModel> connections);
        /// <summary>
        /// Moves figure visuals.
        /// </summary>
        /// <param name="figure"></param>
        /// <param name="oldPos"></param>
        /// <param name="newPos"></param>
        /// <param name="connections"></param>
        void MoveFigureVisuals(FigureModel figure, Point oldPos, Point newPos, ICollection<ConnectionModel> connections);
    }
}
