using Diagrammatist.Domain.Figures.Constants;

namespace Diagrammatist.Domain.Figures
{
    /// <summary>
    /// A shape figure class. Derived class from <see cref="Figure"/>.
    /// </summary>
    public class ShapeFigure : Figure
    {
        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="FigureShapeWidth"]/*'/>
        public double Width { get; set; } = FigureManipulationConstants.DefaultWidth;
        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="FigureShapeHeight"]/*'/>
        public double Height { get; set; } = FigureManipulationConstants.DefaultHeight;
        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="FigureShapeKeepAspectRatio"]/*'/>
        public bool KeepAspectRatio { get; set; } = ShapeFigureBoolConstants.DefaultAspectRatioParameter;
        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="FigureShapeData"]/*'/>
        public List<string> Data { get; set; } = [];
    }
}
