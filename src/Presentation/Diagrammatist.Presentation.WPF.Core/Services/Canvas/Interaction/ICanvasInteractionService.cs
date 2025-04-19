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
        /// <param name="oldPos">Old position.</param>
        /// <param name="newPos">New position.</param>
        /// <param name="connections">Connections that figure may have.</param>
        void MoveFigure(FigureModel figure, Point oldPos, Point newPos, ICollection<ConnectionModel> connections);
    }
}
