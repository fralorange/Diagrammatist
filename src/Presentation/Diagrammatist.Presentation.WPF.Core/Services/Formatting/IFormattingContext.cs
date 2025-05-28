using Diagrammatist.Presentation.WPF.Core.Models.Figures;
using System.Windows;

namespace Diagrammatist.Presentation.WPF.Core.Services.Formatting
{
    /// <summary>
    /// Represents a context for formatting figures in a diagram.
    /// </summary>
    public interface IFormattingContext
    {
        /// <summary>
        /// Moves the specified figure element to a new position in the diagram.
        /// </summary>
        /// <param name="figure"></param>
        /// <param name="newPosition"></param>
        void MoveElement(FigureModel figure, Point newPosition);
        /// <summary>
        /// Sets the size of the specified figure element.
        /// </summary>
        /// <param name="figure"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        void SetElementSize(FigureModel figure, double width, double height);
    }
}
