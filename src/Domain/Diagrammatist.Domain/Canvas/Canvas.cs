using Diagrammatist.Domain.Canvas.Constants;
using Diagrammatist.Domain.Figures;

namespace Diagrammatist.Domain.Canvas
{
    /// <summary>
    /// Canvas class.
    /// </summary>
    public class Canvas
    {
        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="ImaginaryWidth"]/*'/>
        public int ImaginaryWidth { get; set; }
        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="ImaginaryHeight"]/*'/>
        public int ImaginaryHeight { get; set; }
        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="Settings"]/*'/>
        public required Settings Settings { get; set; }
        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="Zoom"]/*'/>
        public double Zoom { get; set; } = CanvasZoomConstants.DefaultZoom;
        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="Offset"]/*'/>
        public Offset Offset { get; set; } = new();
        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="Figures"]/*'/>
        public List<Figure> Figures { get; set; } = [];
        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="Connections"]/*'/>
        public List<Domain.Connection.Connection> Connections { get; set; } = [];
    }
}
