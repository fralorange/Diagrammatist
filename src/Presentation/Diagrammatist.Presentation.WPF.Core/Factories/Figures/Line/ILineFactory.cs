using Diagrammatist.Presentation.WPF.Core.Models.Connection;
using Diagrammatist.Presentation.WPF.Core.Models.Figures;
using Diagrammatist.Presentation.WPF.Core.Models.Figures.Magnetic;

namespace Diagrammatist.Presentation.WPF.Core.Factories.Figures.Line
{
    /// <summary>
    /// A factory interface for creating line figures.
    /// </summary>
    public interface ILineFactory
    {
        /// <summary>
        /// Creates a line figure.
        /// </summary>
        /// <param name="template"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="connections"></param>
        /// <returns></returns>
        LineFigureModel CreateLine(LineFigureModel template,
                                   MagneticPointModel? start,
                                   MagneticPointModel? end,
                                   ICollection<ConnectionModel> connections);
    }
}
