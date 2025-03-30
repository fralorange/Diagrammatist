using Diagrammatist.Presentation.WPF.Core.Models.Figures;
using Diagrammatist.Presentation.WPF.Core.Models.Figures.Magnetic;

namespace Diagrammatist.Presentation.WPF.Core.Models.Connection
{
    /// <summary>
    /// A class that describes connection between shape figure types.
    /// </summary>
    public class ConnectionModel
    {
        public MagneticPointModel? SourceMagneticPoint { get; set; }
        /// <summary>
        /// Gets or sets destination magnetic point that line is connected to, if it exists.
        /// </summary>
        public MagneticPointModel? DestinationMagneticPoint { get; set; }
        /// <summary>
        /// Gets or sets line that connects figure if they exists.
        /// </summary>
        public required LineFigureModel Line { get; set; }
    }
}
