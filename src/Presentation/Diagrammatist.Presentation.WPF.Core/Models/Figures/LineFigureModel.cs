using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows;

namespace Diagrammatist.Presentation.WPF.Core.Models.Figures
{
    /// <summary>
    /// A line figure model. Derived class from <see cref="FigureModel"/>.
    /// </summary>
    public partial class LineFigureModel : FigureModel
    {
        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="FigureLinePoints"]/*'/>
        [ObservableProperty]
        private ObservableCollection<Point> _points;

        private double _thickness;

        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="FigureLineThickness"]/*'/>
        public double Thickness
        {
            get => _thickness;
            set => SetProperty(ref _thickness, value);
        }

        private bool _isDashed;

        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="FigureLineIsDashed"]/*'/>
        public bool IsDashed
        {
            get => _isDashed;
            set => SetProperty(ref _isDashed, value);
        }

        private bool _hasArrow;

        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="FigureLineHasArrow"]/*'/>
        public bool HasArrow
        {
            get => _hasArrow;
            set => SetProperty(ref _hasArrow, value);
        }

        /// <summary>
        /// Default initializer.
        /// </summary>
#pragma warning disable CS8618
        public LineFigureModel() { }
#pragma warning restore CS8618 

        /// <summary>
        /// Clones all properties and initializes new instance.
        /// </summary>
        /// <param name="source">Source model.</param>
        public LineFigureModel(LineFigureModel source) : base(source)
        {
            Points = new(source.Points);
            Thickness = source.Thickness;
            IsDashed = source.IsDashed;
            HasArrow = source.HasArrow;
        }

        /// <inheritdoc/>
        public override FigureModel Clone()
        {
            return new LineFigureModel(this);
        }
    }
}
