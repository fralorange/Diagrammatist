using Diagrammatist.Presentation.WPF.Core.Models.Connection;
using Diagrammatist.Presentation.WPF.Core.Models.Figures;
using Diagrammatist.Presentation.WPF.Core.Models.Figures.Magnetic;
using Diagrammatist.Presentation.WPF.Core.Models.Figures.Special.Flowchart;
using Diagrammatist.Presentation.WPF.Core.Services.Connection;

namespace Diagrammatist.Presentation.WPF.Core.Factories.Figures.Line
{
    /// <summary>
    /// A class that implements <see cref="ILineFactory"/>.
    /// This class is responsible for creating line figures.
    /// </summary>
    public class LineFactory : ILineFactory
    {
        private readonly IConnectionService _connectionService;

        /// <summary>
        /// Initializes a new instance of the <see cref="LineFactory"/> class.
        /// </summary>
        /// <param name="connectionService"></param>
        public LineFactory(IConnectionService connectionService)
        {
            _connectionService = connectionService;
        }

        /// <inheritdoc/>
        public LineFigureModel CreateLine(LineFigureModel template,
                                          MagneticPointModel? start,
                                          MagneticPointModel? end,
                                          ICollection<ConnectionModel> connections)
        {
            if (start?.Owner is FlowchartFigureModel owner
            && (owner.Subtype == FlowchartSubtypeModel.Decision || owner.Subtype == FlowchartSubtypeModel.Preparation))
            {
                var outgoingCount = _connectionService
                    .GetConnections(connections, owner)
                    .Count(c => c.SourceMagneticPoint?.Owner == owner);

                var flowLine = new FlowLineFigureModel(template)
                {
                    Condition = outgoingCount == 0
                        ? FlowConditionModel.True
                        : FlowConditionModel.False,
                };
                flowLine.Label = flowLine.Condition.ToString();
                return flowLine;
            }

            return (LineFigureModel)template.Clone();
        }
    }
}
