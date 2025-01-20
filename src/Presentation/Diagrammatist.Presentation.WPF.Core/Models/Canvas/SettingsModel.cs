using CommunityToolkit.Mvvm.ComponentModel;
using System.Drawing;

namespace Diagrammatist.Presentation.WPF.Core.Models.Canvas
{
    /// <summary>
    /// Diagram settings.
    /// </summary>
    public partial class SettingsModel : ObservableObject
    {
        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="FileName"]/*'/>
        [ObservableProperty]
        private string _fileName = string.Empty;
        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="Width"]/*'/>
        [ObservableProperty]
        private int _width;
        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="Height"]/*'/>
        [ObservableProperty]
        private int _height;
        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="Background"]/*'/>
        [ObservableProperty]
        private Color _background;
        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="DiagramType"]/*'/>
        public DiagramsModel Type { get; set; }
    }
}
