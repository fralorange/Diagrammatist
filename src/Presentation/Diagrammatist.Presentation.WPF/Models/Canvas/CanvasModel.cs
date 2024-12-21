using CommunityToolkit.Mvvm.ComponentModel;
using Diagrammatist.Presentation.WPF.Models.Figures;
using System.Collections.ObjectModel;

namespace Diagrammatist.Presentation.WPF.Models.Canvas
{
    /// <summary>
    /// A canvas model.
    /// </summary>
    public partial class CanvasModel : ObservableObject
    {
        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="ImaginaryWidth"]/*'/>
        [ObservableProperty]
        private int _imaginaryWidth;
        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="ImaginaryHeight"]/*'/>
        [ObservableProperty]
        private int _imaginaryHeight;
        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="Settings"]/*'/>
        [ObservableProperty]
        private SettingsModel _settings;
        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="Zoom"]/*'/>
        [ObservableProperty]
        private double _zoom;
        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="Offset"]/*'/>
        [ObservableProperty]
        private OffsetModel _offset;
        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="Figures"]/*'/>
        public ObservableCollection<FigureModel> Figures { get; set; } = [];
        /// <summary>
        /// Gets or sets 'has changes' flag.
        /// </summary>
        /// <remarks>
        /// This property used to determine whether canvas has changes or not.
        /// </remarks>
        [ObservableProperty]
        private bool _hasChanges;
    }
}
