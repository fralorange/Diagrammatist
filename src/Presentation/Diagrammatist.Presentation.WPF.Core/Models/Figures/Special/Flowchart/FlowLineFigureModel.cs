using System.Collections.Specialized;
using System.Windows;

namespace Diagrammatist.Presentation.WPF.Core.Models.Figures.Special.Flowchart
{
    /// <summary>
    /// A class that represents a flowchart special line figure.
    /// </summary>
    public class FlowLineFigureModel : LineFigureModel
    {
        /// <summary>
        /// Gets or sets the flowchart condition.
        /// </summary>
        public FlowConditionModel Condition { get; set; } = FlowConditionModel.None;

        private string _label = string.Empty;

        /// <summary>
        /// Gets or sets the label of the flowchart figure.
        /// </summary>
        public string Label
        {
            get => _label;
            set => SetProperty(ref _label, value);
        }

        private double _labelFontSize = 12;

        /// <summary>
        /// Gets or sets the font size of the label.
        /// </summary>
        public double LabelFontSize
        {
            get => _labelFontSize;
            set => SetProperty(ref _labelFontSize, value);
        }

        /// <summary>
        /// Gets the position of the label based on the first two points of the line.
        /// </summary>
        public Point LabelPosition 
        { 
            get
            {
                if (Points is null || Points.Count < 2)
                    return new Point(0, 0);

                var p0 = Points[0];
                var p1 = Points[1];

                var midX = (p0.X + p1.X) / 2 - PosX;
                var midY = (p0.Y + p1.Y) / 2 - PosY - 20;

                return new Point(midX, midY);
            }
        }

        /// <summary>
        /// Default initializer.
        /// </summary>
        public FlowLineFigureModel()
        {
            Initialize();
        }

        /// <summary>
        /// Clones all properties and initializes new instance.
        /// </summary>
        /// <param name="source"></param>
        public FlowLineFigureModel(FlowLineFigureModel source) 
            : base(source)
        {
            Condition = source.Condition;
            Label = source.Label;
            LabelFontSize = source.LabelFontSize;

            Initialize();
        }

        /// <summary>
        /// Clones all properties and initializes new instance from <see cref="LineFigureModel"/>.
        /// </summary>
        /// <param name="lineFigure"></param>
        public FlowLineFigureModel(LineFigureModel lineFigure) 
            : base(lineFigure) 
        {
            Initialize();
        }


        /// <inheritdoc/>
        public override FigureModel Clone()
        {
            return new FlowLineFigureModel(this);
        }

        private void Initialize()
        {
            if (Points is INotifyCollectionChanged col)
                col.CollectionChanged += (_, __) =>
                    OnPropertyChanged(nameof(LabelPosition));
        }
    }
}
