using CommunityToolkit.Mvvm.ComponentModel;
using System.Drawing;

namespace Diagrammatist.Presentation.WPF.Models.Figures
{
    /// <summary>
    /// A line figure model. Derived class from <see cref="FigureModel"/>.
    /// </summary>
    public partial class LineFigureModel : FigureModel
    {
        /// <summary>
        /// Gets or sets collection of points.
        /// </summary>
        /// <remarks>
        /// This property used to draw line by points.
        /// </remarks>
        [ObservableProperty]
        private List<Point> _points;

        /// <summary>
        /// Gets or sets line thickness.
        /// </summary>
        /// <remarks>
        /// This property used to configure line thickness.
        /// </remarks>
        [ObservableProperty]
        private double _thickness;

        /// <summary>
        /// Gets or sets line dash condition.
        /// </summary>
        /// <remarks>
        /// This property indicates whether the line is dashed.
        /// </remarks>
        [ObservableProperty]
        private bool _isDashed;

        /// <summary>
        /// Gets or sets line arrow condition.
        /// </summary>
        /// <remarks>
        /// This property indicates whether the line has arrow on last point.
        /// </remarks>
        [ObservableProperty]
        private bool _hasArrow;

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
