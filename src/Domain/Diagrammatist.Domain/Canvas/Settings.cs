using System.Drawing;

namespace Diagrammatist.Domain.Canvas
{
    /// <summary>
    /// Diagram settings.
    /// </summary>
    public class Settings
    {
        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="FileName"]/*'/>
        public string FileName { get; set; } = string.Empty;
        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="Width"]/*'/>
        public double Width { get; set; } 
        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="Height"]/*'/>
        public double Height { get; set; } 
        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="Background"]/*'/>
        public Color Background { get; set; } 
        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="DiagramType"]/*'/>
        public Diagrams Type { get; set; } 
    }
}
