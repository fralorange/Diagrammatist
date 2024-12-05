using CommunityToolkit.Mvvm.ComponentModel;

namespace Diagrammatist.Presentation.WPF.Models.Figures
{
    /// <summary>
    /// A shape figure model. Derived class from <see cref="FigureModel"/>.
    /// </summary>
    public partial class ShapeFigureModel : FigureModel
    {
        /// <summary>
        /// Gets or sets width.
        /// </summary>
        /// <remarks>
        /// This property used to store figure width.
        /// </remarks>
        [ObservableProperty]
        private double _width;

        /// <summary>
        /// Gets or sets height.
        /// </summary>
        /// <remarks>
        /// This property used to store figure height.
        /// </remarks>
        [ObservableProperty]
        private double _height;
        /// <summary>
        /// Gets or sets aspect ratio keep parameter.
        /// </summary>
        /// <remarks>
        /// This property is used to determine whether or not to keep the aspect ratio.
        /// </remarks>
        [ObservableProperty]
        private bool _keepAspectRatio;
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
