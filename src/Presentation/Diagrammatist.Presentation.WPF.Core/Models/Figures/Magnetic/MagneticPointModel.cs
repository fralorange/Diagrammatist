using System.Windows;

namespace Diagrammatist.Presentation.WPF.Core.Models.Figures.Magnetic
{
    /// <summary>
    /// A class that describes magnetic point.
    /// </summary>
    public class MagneticPointModel
    {
        /// <summary>
        /// Gets or sets current magnetic point position.
        /// </summary>
        public Point Position { get; set; }
        /// <summary>
        /// Gets or sets current magnetic point owner.
        /// </summary>
        public required ShapeFigureModel Owner { get ; set; }
    }
}
