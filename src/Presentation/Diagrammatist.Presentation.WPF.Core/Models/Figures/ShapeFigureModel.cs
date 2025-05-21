using CommunityToolkit.Mvvm.ComponentModel;
using Diagrammatist.Presentation.WPF.Core.Helpers;
using Diagrammatist.Presentation.WPF.Core.Models.Figures.Interfaces;
using Diagrammatist.Presentation.WPF.Core.Models.Figures.Magnetic;
using System.Windows.Media;

namespace Diagrammatist.Presentation.WPF.Core.Models.Figures
{
    /// <summary>
    /// A shape figure model. Derived class from <see cref="FigureModel"/>.
    /// </summary>
    public partial class ShapeFigureModel : FigureModel, IMagneticSupport
    {
        private double _width;

        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="FigureShapeWidth"]/*'/>
        public double Width
        {
            get => _width;
            set
            {
                if (SetProperty(ref _width, value))
                {
                    UpdateMagneticPoints();
                }
            }
        }

        private double _height;

        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="FigureShapeHeight"]/*'/>
        public double Height
        {
            get => _height;
            set
            {
                if (SetProperty(ref _height, value))
                {
                    UpdateMagneticPoints();
                }
            }
        }

        private bool _keepAspectRatio;

        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="FigureShapeKeepAspectRatio"]/*'/>
        public bool KeepAspectRatio
        {
            get => _keepAspectRatio;
            set
            {
                if (SetProperty(ref _keepAspectRatio, value))
                {
                    UpdateMagneticPoints();
                }
            }
        }

        /// <inheritdoc/>
        public override double Rotation 
        { 
            get => base.Rotation; 
            set 
            {
                if (SetProperty(ref _rotation, value))
                {
                    UpdateMagneticPoints();
                }
            } 
        }

        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="FigureShapeData"]/*'/>
        public List<string> Data { get; set; } = [];

        /// <inheritdoc cref="IMagneticSupport.MagneticPoints"/>
        [ObservableProperty]
        private List<MagneticPointModel> _magneticPoints = [];

        /// <summary>
        /// Default initializer.
        /// </summary>
        public ShapeFigureModel() { }

        /// <summary>
        /// Clones all properties and initializes new instance.
        /// </summary>
        /// <param name="source">Source model.</param>
        public ShapeFigureModel(ShapeFigureModel source) : base(source)
        {
            Width = source.Width;
            Height = source.Height;
            KeepAspectRatio = source.KeepAspectRatio;
            Data = new(source.Data);

            UpdateMagneticPoints();
        }

        /// <inheritdoc/>
        public override FigureModel Clone()
        {
            return new ShapeFigureModel(this);
        }

        /// <inheritdoc/>
        public override void CopyPropertiesTo(FigureModel target)
        {
            base.CopyPropertiesTo(target);

            if (target is ShapeFigureModel shapeTarget)
            {
                shapeTarget.Width = Width;
                shapeTarget.Height = Height;
                shapeTarget.KeepAspectRatio = KeepAspectRatio;
            }
        }

        /// <inheritdoc/>
        public void UpdateMagneticPoints()
        {
            if (Data.Any(string.IsNullOrEmpty))
                return;

            MagneticPoints.Clear();

            var geometryGroup = new GeometryGroup();

            foreach (var pathData in Data)
            {
                var geometry = Geometry.Parse(pathData);
                geometryGroup.Children.Add(geometry);
            }

            var points = FigureSnapHelper.GetMagneticPoints(geometryGroup, Width, Height, KeepAspectRatio, Rotation);

            foreach (var point in points)
            {
                MagneticPoints.Add(new() { Position = point, Owner = this });
            }

            OnPropertyChanged(nameof(MagneticPoints));
        }
    }
}
