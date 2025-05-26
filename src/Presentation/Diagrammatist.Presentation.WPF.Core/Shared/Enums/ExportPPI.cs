namespace Diagrammatist.Presentation.WPF.Core.Shared.Enums
{
    /// <summary>
    /// Enumeration representing the export PPI (Pixels Per Inch) settings.
    /// </summary>
    public enum ExportPPI
    {
        /// <summary>
        /// Represents a PPI of 72, commonly used for screen display.
        /// </summary>
        PPI_72 = 72,
        /// <summary>
        /// Represents a PPI of 96, often used for web images.
        /// </summary>
        PPI_96 = 96,
        /// <summary>
        /// Represents a PPI of 150, suitable for high-quality prints.
        /// </summary>
        PPI_150 = 150,
        /// <summary>
        /// Represents a PPI of 300, typically used for professional printing.
        /// </summary>
        PPI_300 = 300,
        /// <summary>
        /// Represents a PPI of 600, used for very high-resolution prints.
        /// </summary>
        PPI_600 = 600,
        /// <summary>
        /// Represents a custom PPI value, allowing users to specify their own PPI.
        /// </summary>
        PPI_Custom = -1
    }
}
