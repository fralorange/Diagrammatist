using Diagrammatist.Domain.Figures.Constants;
using System.Drawing;

namespace Diagrammatist.Domain.Figures
{
    /// <summary>
    /// A line figure class. Derived class from <see cref="Figure"/>.
    /// </summary>
    public class LineFigure : Figure
    {
        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="FigureLinePoints"]/*'/>
        public List<Point> Points { get; set; } = [];
        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="FigureLineThickness"]/*'/>
        public double Thickness { get; set; } = LineFigureManipulationConstants.DefaultThickness;
        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="FigureLineIsDashed"]/*'/>
        public bool IsDashed { get; set; } = LineFigureBoolConstants.DefaultDashedParameter;
        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="FigureLineHasArrow"]/*'/>
        public bool HasArrow { get; set; } = LineFigureBoolConstants.DefaultArrowParameter;
    }
}
