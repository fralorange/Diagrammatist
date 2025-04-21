using Diagrammatist.Presentation.WPF.Core.Models.Figures.Special.Flowchart;
using Diagrammatist.Presentation.WPF.Simulator.Models.Node;
using Diagrammatist.Presentation.WPF.Simulator.Providers;
using System.Windows;
using System.Windows.Controls;

namespace Diagrammatist.Presentation.WPF.Simulator.Selectors
{
    /// <summary>
    /// A class that derives from <see cref="DataTemplateSelector"/>. Selects template by node type.
    /// </summary>
    public class SimulationScriptSelector : DataTemplateSelector
    {
        /// <summary>
        /// Gets or sets flowchart simulation node template.
        /// </summary>
        public DataTemplate? SimulationNodeTemplate { get; set; }

        /// <summary>
        /// Gets or sets flowchart simulation node with file template.
        /// </summary>
        public DataTemplate? SimulationFileNodeTemplate { get; set; }

        /// <inheritdoc/>
        public override DataTemplate? SelectTemplate(object item, DependencyObject container)
        {
            if (item is SimulationNode node)
            {
                var factory = TemplateFactoryProvider.GetFactory(node, SimulationNodeTemplate, SimulationFileNodeTemplate);
                return factory.CreateTemplate(node);
            }

            return null;
        }
    }
}
