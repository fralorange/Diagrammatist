using Diagrammatist.Presentation.WPF.Core.Models.Connection;
using Diagrammatist.Presentation.WPF.Core.Models.Figures;

namespace Diagrammatist.Presentation.WPF.Core.Services.Figure.Manipulation
{
    /// <summary>
    /// An interface for manipulating figures.
    /// </summary>
    public interface IFigureManipulationService
    {
        /// <summary>
        /// Copies figure.
        /// </summary>
        /// <param name="figure">Target figure.</param>
        void Copy(FigureModel figure);
        /// <summary>
        /// Cuts figure from canvas.
        /// </summary>
        /// <param name="figure">Target figure.</param>
        /// <param name="figures">Figures that placed on canvas.</param>
        void Cut(FigureModel figure, ICollection<FigureModel> figures);
        /// <summary>
        /// Pastes figure to canvas.
        /// </summary>
        /// <param name="figures">Figures that placed on canvas.</param>
        void Paste(ICollection<FigureModel> figures);
        /// <summary>
        /// Pastes figure to canvas.
        /// </summary>
        /// <param name="figures">Figures that placed on canvas.</param>
        /// <param name="pos">Position.</param>
        void Paste(ICollection<FigureModel> figures, object pos);
        /// <summary>
        /// Duplicates figure on canvas.
        /// </summary>
        /// <param name="figure">Target figure.</param>
        /// <param name="figures">Figures that placed on canvas.</param>
        void Duplicate(FigureModel figure, ICollection<FigureModel> figures);
        /// <summary>
        /// Deletes figure from canvas.
        /// </summary>
        /// <param name="figure">Target figure.</param>
        /// <param name="figures">Figures that placed on canvas.</param>
        void Delete(FigureModel figure, ICollection<FigureModel> figures, ICollection<ConnectionModel> connections);
        /// <summary>
        /// Sends figure backward.
        /// </summary>
        /// <param name="figure">Target figure.</param>
        void SendBackward(FigureModel figure);
        /// <summary>
        /// Brings figure forward.
        /// </summary>
        /// <param name="figure">Target figure.</param>
        void BringForward(FigureModel figure);
    }
}
