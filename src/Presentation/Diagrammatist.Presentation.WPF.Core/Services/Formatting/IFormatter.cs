using Diagrammatist.Presentation.WPF.Core.Models.Figures;

namespace Diagrammatist.Presentation.WPF.Core.Services.Formatting
{
    /// <summary>
    /// Interface for formatting figures in a diagram.
    /// </summary>
    public interface IFormatter
    {
        /// <summary>
        /// Gets the name of the formatter.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Formats the given figures using the specified formatting context.
        /// </summary>
        /// <param name="figures"></param>
        /// <param name="formattingContext"></param>
        void Format(IEnumerable<FigureModel> figures, IFormattingContext formattingContext);
    }
}
