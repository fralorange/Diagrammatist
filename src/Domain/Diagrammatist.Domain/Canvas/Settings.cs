using Diagrammatist.Domain.Canvas.Constants;
using System.Drawing;

namespace Diagrammatist.Domain.Canvas
{
    /// <summary>
    /// Diagram settings.
    /// </summary>
    public class Settings
    {
        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="FileName"]/*'/>
        public string FileName { get; set; } = SettingsConstants.DefaultFileName;
        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="Width"]/*'/>
        public int Width { get; set; } = SettingsConstants.DefaultWidth;
        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="Height"]/*'/>
        public int Height { get; set; } = SettingsConstants.DefaultHeight;
        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="Background"]/*'/>
        public Color Background { get; set; } = SettingsConstants.DefaultBackground;
        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="DiagramType"]/*'/>
        public Diagrams Type { get; set; } = SettingsConstants.DefaultType;
    }
}
