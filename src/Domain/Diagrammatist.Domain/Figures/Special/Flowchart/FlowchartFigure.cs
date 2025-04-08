using Diagrammatist.Domain.Figures.Special.Container;

namespace Diagrammatist.Domain.Figures.Special.Flowchart
{
    /// <summary>
    /// A flowchart figure class. Derived class from <see cref="ContainerFigure"/>.
    /// </summary>
    /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="FigureFlowchart"]/*'/>
    public class FlowchartFigure : ContainerFigure
    {
        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="FigureFlowchartSubtype"]/*'/>
        public FlowchartSubtype Subtype { get; set; }
    }
}
