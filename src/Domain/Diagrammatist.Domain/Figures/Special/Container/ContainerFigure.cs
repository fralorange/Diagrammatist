using Diagrammatist.Domain.Figures.Constants;
using System.Drawing;

namespace Diagrammatist.Domain.Figures.Special.Container
{
    /// <summary>
    /// A container figure class. Derived class from <see cref="ShapeFigure"/>.
    /// </summary>
    /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="FigureContainer"]/*'/>
    public class ContainerFigure : ShapeFigure
    {
        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="FigureTextContent"]/*'/>
        public string Text { get; set; } = FigureTextConstants.DefaultText;
        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="FigureTextColor"]/*'/>
        public Color TextColor { get; set; } = Color.Black;
        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="FigureTextFontSize"]/*'/>
        public double FontSize { get; set; } = TextFigureManipulationConstants.DefaultFontSize;
    }
}
