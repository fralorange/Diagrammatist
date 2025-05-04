using Diagrammatist.Presentation.WPF.Core.Models.Canvas;
using System.Windows;
using System.Windows.Controls;

namespace Diagrammatist.Presentation.WPF.Core.Interactions.Selectors
{
    /// <summary>
    /// A class that derives from <see cref="DataTemplateSelector"/>.
    /// This class is used to select a template for a diagram based on its type.
    /// </summary>
    public class DiagramTemplateSelector : DataTemplateSelector
    {
        /// <summary>
        /// Gets or sets the template for custom diagrams.
        /// </summary>
        public DataTemplate? CustomTemplate { get; set; }
        /// <summary>
        /// Gets or sets the template for flowchart diagrams.
        /// </summary>
        public DataTemplate? FlowchartTemplate { get; set; }

        /// <summary>
        /// Selects the appropriate template based on the type of diagram model.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="container"></param>
        /// <returns></returns>
        public override DataTemplate? SelectTemplate(object item, DependencyObject container)
        {
            return item switch
            {
                DiagramsModel.Custom => CustomTemplate,
                DiagramsModel.Flowchart => FlowchartTemplate,
                _ => null
            };
        }
    }
}
