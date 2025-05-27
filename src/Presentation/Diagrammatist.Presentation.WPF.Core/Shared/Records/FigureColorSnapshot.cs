using Diagrammatist.Presentation.WPF.Core.Models.Figures;
using System.Windows.Media;

namespace Diagrammatist.Presentation.WPF.Core.Shared.Records
{
    /// <summary>
    /// Represents a snapshot of a figure's color properties.
    /// </summary>
    /// <param name="Figure"></param>
    /// <param name="BackgroundColor"></param>
    /// <param name="TextColor"></param>
    /// <param name="ThemeColor"></param>
    public record FigureColorSnapshot(FigureModel Figure, Color BackgroundColor, Color? TextColor, Color? ThemeColor);
}
