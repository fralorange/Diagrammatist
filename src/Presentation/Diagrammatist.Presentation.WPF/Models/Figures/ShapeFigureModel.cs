using CommunityToolkit.Mvvm.ComponentModel;

namespace Diagrammatist.Presentation.WPF.Models.Figures
{
    /// <summary>
    /// A shape figure model. Derived class from <see cref="FigureModel"/>.
    /// </summary>
    public partial class ShapeFigureModel : FigureModel
    {
        private double _width;

        /// <summary>
        /// Gets or sets width.
        /// </summary>
        /// <remarks>
        /// This property used to store figure width.
        /// </remarks>
        public double Width
        {
            get => _width;
            set => SetProperty(ref _width, value);
        }

        private double _height;

        /// <summary>
        /// Gets or sets height.
        /// </summary>
        /// <remarks>
        /// This property used to store figure height.
        /// </remarks>
        public double Height
        {
            get => _height;
            set => SetProperty(ref _height, value);
        }

        private bool _keepAspectRatio;

        /// <summary>
        /// Gets or sets aspect ratio keep parameter.
        /// </summary>
        /// <remarks>
        /// This property is used to determine whether or not to keep the aspect ratio.
        /// </remarks>
        public bool KeepAspectRatio
        {
            get => _keepAspectRatio;
            set => SetProperty(ref _keepAspectRatio, value);
        }

        /// <summary>
        /// Gets or sets collection of data.
        /// </summary>
        /// <remarks>
        /// This property used to store data.
        /// </remarks>
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
