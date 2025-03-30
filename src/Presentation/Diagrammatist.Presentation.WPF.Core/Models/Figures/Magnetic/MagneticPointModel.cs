using System.Windows;

namespace Diagrammatist.Presentation.WPF.Core.Models.Figures.Magnetic
{
    /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="MagneticPoint"]/*'/>
    public class MagneticPointModel
    {
        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="Position"]/*'/>
        public Point Position { get; set; }
        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="Owner"]/*'/>
        public required ShapeFigureModel Owner { get ; set; }
    }
}
