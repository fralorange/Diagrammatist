using Diagrammatist.Presentation.WPF.Core.Models.Connection;
using Diagrammatist.Presentation.WPF.Core.Models.Figures;

namespace Diagrammatist.Presentation.WPF.Core.Services.Figure.Placement
{
    /// <summary>
    /// An interface that specializes on figure placement inside canvas.
    /// </summary>
    public interface IFigurePlacementService
    {
        /// <summary>
        /// Adds figure with undoable properties.
        /// </summary>
        /// <param name="figure">Target figure.</param>
        /// <param name="figures">Target collection.</param>
        /// <param name="connections">Connections.</param>
        void AddFigureWithUndo(FigureModel figure, ICollection<FigureModel> figures, ICollection<ConnectionModel> connections);
        /// <summary>
        /// Tracks changes on target figure.
        /// </summary>
        /// <param name="figure">Target figure.</param>
        void TrackChanges(FigureModel figure);
    }
}
