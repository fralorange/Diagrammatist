namespace Diagrammatist.Domain.Figures.Special.Flowchart
{
    /// <summary>
    /// A class that represents a flowchart special line figure.
    /// </summary>
    public class FlowLineFigure : LineFigure
    {
        /// <summary>
        /// Gets or sets the flowchart condition.
        /// </summary>
        public FlowCondition Condition { get; set; }

        /// <summary>
        /// Gets or sets the label of the flowchart figure.
        /// </summary>
        public string Label { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the font size of the label.
        /// </summary>
        public double LabelFontSize { get; set; }

        /// <summary>
        /// Gets or sets the position by X of the label based on the first two points of the line.
        /// </summary>
        public double LabelPositionX { get; set; }

        /// <summary>
        /// Gets or sets the position by Y of the label based on the first two points of the line.
        /// </summary>
        public double LabelPositionY { get; set; }
    }
}
