using Diagrammatist.Presentation.WPF.Core.Models.Figures;
using Diagrammatist.Presentation.WPF.Core.Models.Figures.Special.Container;
using Diagrammatist.Presentation.WPF.Core.Models.Figures.Special.Flowchart;
using System.Windows;
using System.Windows.Controls;

namespace Diagrammatist.Presentation.WPF.Core.Interactions.Selectors
{
    /// <summary>
    /// A class that derives from <see cref="DataTemplateSelector"/>. Selects template based on figure's type.
    /// </summary>
    public class FigureItemTemplateSelector : DataTemplateSelector
    {
        /// <summary>
        /// Gets or sets shape figure template.
        /// </summary>
        public DataTemplate? ShapeFigureTemplate { get; set; }
        /// <summary>
        /// Gets or sets line figure template.
        /// </summary>
        public DataTemplate? LineFigureTemplate { get; set; }
        /// <summary>
        /// Gets or sets text figure template.
        /// </summary>
        public DataTemplate? TextFigureTemplate { get; set; }
        /// <summary>
        /// Gets or sets container figure template.
        /// </summary>
        public DataTemplate? ContainerFigureTemplate { get; set; }
        /// <summary>
        /// Gets or sets flowchart figure template.
        /// </summary>
        public DataTemplate? FlowchartFigureTemplate { get; set; }

        /// <inheritdoc/>
        public override DataTemplate? SelectTemplate(object item, DependencyObject container)
        {
            return item switch
            {
                FlowchartFigureModel => FlowchartFigureTemplate,
                ContainerFigureModel => ContainerFigureTemplate,
                ShapeFigureModel => ShapeFigureTemplate,
                LineFigureModel => LineFigureTemplate,
                TextFigureModel => TextFigureTemplate,
                _ => null
            };
        }
    }
}
