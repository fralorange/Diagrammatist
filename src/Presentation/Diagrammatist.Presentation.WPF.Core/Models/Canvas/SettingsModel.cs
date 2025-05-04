using CommunityToolkit.Mvvm.ComponentModel;
using Diagrammatist.Presentation.WPF.Core.Models.Canvas.Constants;
using System.Windows.Media;

namespace Diagrammatist.Presentation.WPF.Core.Models.Canvas
{
    /// <summary>
    /// Diagram settings.
    /// </summary>
    public partial class SettingsModel : ObservableObject
    {
        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="FileName"]/*'/>
        [ObservableProperty]
        private string _fileName = SettingsConstants.DefaultFileName;
        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="Width"]/*'/>
        [ObservableProperty]
        private double _width = SettingsConstants.DefaultWidth;
        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="Height"]/*'/>
        [ObservableProperty]
        private double _height = SettingsConstants.DefaultHeight;
        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="Background"]/*'/>
        [ObservableProperty]
        private Color _background;
        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="DiagramType"]/*'/>
        [ObservableProperty]
        private DiagramsModel _type = SettingsConstants.DefaultType;
    }
}
