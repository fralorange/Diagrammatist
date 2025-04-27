using Diagrammatist.Domain.Figures.Magnetic;

namespace Diagrammatist.Domain.Connection
{
    /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="Connection"]/*'/>
    public class ConnectionData
    {
        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="SourceMagneticPoint"]/*'/>
        public MagneticPoint? SourceMagneticPoint { get; set; }
        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="DestinationMagneticPoint"]/*'/>
        public MagneticPoint? DestinationMagneticPoint { get; set; }
        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="Line"]/*'/>
        public required Guid LineId { get; set; }
    }
}
