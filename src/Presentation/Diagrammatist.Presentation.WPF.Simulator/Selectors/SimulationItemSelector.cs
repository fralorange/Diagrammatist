using Diagrammatist.Presentation.WPF.Core.Models.Connection;
using Diagrammatist.Presentation.WPF.Simulator.Models.Node;
using System.Windows;
using System.Windows.Controls;

namespace Diagrammatist.Presentation.WPF.Simulator.Selectors
{
    /// <summary>
    /// A class that derives from <see cref="DataTemplateSelector"/>. Selects template by simulation item type.
    /// </summary>
    public class SimulationItemSelector : DataTemplateSelector
    {
        /// <summary>
        /// Gets or sets simulation node template.
        /// </summary>
        public DataTemplate? SimulationNodeTemplate { get; set; }

        /// <summary>
        /// Gets or sets simulation connection template.
        /// </summary>
        public DataTemplate? SimulationConnectionTemplate { get; set; }

        /// <inheritdoc/>
        public override DataTemplate? SelectTemplate(object item, DependencyObject container)
        {
            return item switch
            {
                SimulationNodeBase => SimulationNodeTemplate,
                ConnectionModel => SimulationConnectionTemplate,
                _ => null,
            };
        }
    }
}
