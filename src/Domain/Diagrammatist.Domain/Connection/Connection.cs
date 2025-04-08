using Diagrammatist.Domain.Figures;
using Diagrammatist.Domain.Figures.Magnetic;

namespace Diagrammatist.Domain.Connection
{
    /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="Connection"]/*'/>
    public class Connection
    {
        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="SourceMagneticPoint"]/*'/>
        public MagneticPoint? SourceMagneticPoint { get; set; }
        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="DestinationMagneticPoint"]/*'/>
        public MagneticPoint? DestinationMagneticPoint { get; set; }
        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="Line"]/*'/>
        public required LineFigure Line { get; set; }
    }
}
