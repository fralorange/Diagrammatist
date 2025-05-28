using Diagrammatist.Presentation.WPF.Core.Shared.Enums;

namespace Diagrammatist.Presentation.WPF.Core.Shared.Records
{
    /// <summary>
    /// A record that holds the export settings for a diagram.
    /// </summary>
    /// <param name="ExportScenario">The export scenario to be used.</param>
    /// <param name="Ppi">The export PPI (Pixels Per Inch) setting.</param>
    /// <param name="ContentMargin">The margin around the content in pixels.</param>
    /// <param name="ExportTheme">The theme to be used for exporting the diagram.</param>
    public record ExportSettings(ExportScenario ExportScenario, int ContentMargin, int Ppi, ExportTheme ExportTheme);
}
