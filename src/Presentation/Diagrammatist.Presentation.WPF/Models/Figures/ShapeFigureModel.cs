namespace Diagrammatist.Presentation.WPF.Models.Figures
{
    /// <summary>
    /// A shape figure model. Derived class from <see cref="FigureModel"/>.
    /// </summary>
    public partial class ShapeFigureModel : FigureModel
    {
        private double _width;

        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="FigureShapeWidth"]/*'/>
        public double Width
        {
            get => _width;
            set => SetProperty(ref _width, value);
        }

        private double _height;

        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="FigureShapeHeight"]/*'/>
        public double Height
        {
            get => _height;
            set => SetProperty(ref _height, value);
        }

        private bool _keepAspectRatio;

        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="FigureShapeKeepAspectRatio"]/*'/>
        public bool KeepAspectRatio
        {
            get => _keepAspectRatio;
            set => SetProperty(ref _keepAspectRatio, value);
        }

        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="FigureShapeData"]/*'/>
        public List<string> Data { get; set; } = [];

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
        }

        /// <inheritdoc/>
        public override FigureModel Clone()
        {
            return new ShapeFigureModel(this);
        }
    }
}
