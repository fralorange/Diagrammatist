using Diagrammatist.Presentation.WPF.Core.Models.Figures.Special.Container;

namespace Diagrammatist.Presentation.WPF.Core.Models.Figures.Special.Flowchart
{
    /// <summary>
    /// A flowchart figure class model. Derived class from <see cref="ContainerFigureModel"/>.
    /// </summary>
    /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="FigureContainer"]/*'/>
    public class FlowchartFigureModel : ContainerFigureModel
    {
        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="FigureFlowchartSubtype"]/*'/>
        public FlowchartSubtypeModel Subtype { get; set; }

        /// <inheritdoc/>
        public FlowchartFigureModel() { }

        public FlowchartFigureModel(FlowchartFigureModel source) : base(source)
        {
            Subtype = source.Subtype;
        }

        /// <inheritdoc/>
        public override FigureModel Clone()
        {
            return new FlowchartFigureModel(this);
        }

        public override void CopyPropertiesTo(FigureModel target)
        {
            base.CopyPropertiesTo(target);
        }
    }
}
