using Diagrammatist.Domain.Figures;

namespace Diagrammatist.Domain.Canvas
{
    /// <summary>
    /// A class that represents diagram drawing context.
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
        public double Zoom { get; set; }
        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="Offset"]/*'/>
        public Offset Offset { get; set; } = new();
        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="Figures"]/*'/>
        public List<Figure> Figures { get; set; } = [];
        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="Connections"]/*'/>
        public List<Connection.ConnectionData> Connections { get; set; } = [];
    }
}
