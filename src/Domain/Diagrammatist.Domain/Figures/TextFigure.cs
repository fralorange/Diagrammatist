using Diagrammatist.Domain.Figures.Constants;
using System.Drawing;

namespace Diagrammatist.Domain.Figures
{
    /// <summary>
    /// A text figure class. Derived class from <see cref="Figure"/>.
    /// </summary>
    public class TextFigure : Figure
    {
        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="FigureTextContent"]/*'/>
        public string Text { get; set; } = FigureTextConstants.DefaultText;
        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="FigureTextColor"]/*'/>
        public Color TextColor { get; set; } = Color.Black;
        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="FigureTextFontSize"]/*'/>
        public double FontSize { get; set; } = TextFigureManipulationConstants.DefaultFontSize;
        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="FigureTextHasOutline"]/*'/>
        public bool HasOutline { get; set; } = TextFigureBoolConstants.DefaultOutlineParameter;
        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="FigureTextHasBackground"]/*'/>
        public bool HasBackground { get; set; } = TextFigureBoolConstants.DefaultBackgroundParameter;
    }
}
