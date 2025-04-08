using Diagrammatist.Presentation.WPF.Core.Models.Figures;
using Diagrammatist.Presentation.WPF.Core.Models.Figures.Magnetic;

namespace Diagrammatist.Presentation.WPF.Core.Models.Connection
{
    /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="Connection"]/*'/>
    public class ConnectionModel
    {
        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="SourceMagneticPoint"]/*'/>
        public MagneticPointModel? SourceMagneticPoint { get; set; }
        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="DestinationMagneticPoint"]/*'/>
        public MagneticPointModel? DestinationMagneticPoint { get; set; }
        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="Line"]/*'/>
        public required LineFigureModel Line { get; set; }
    }
}
