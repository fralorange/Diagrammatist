using Diagrammatist.Presentation.WPF.Core.Models.Connection;
using Diagrammatist.Presentation.WPF.Core.Models.Figures;
using Diagrammatist.Presentation.WPF.Simulator.Models.Node;
using System.Windows;
using System.Windows.Controls;

namespace Diagrammatist.Presentation.WPF.Simulator.Selectors
{
    /// <summary>
    /// A class that derives from <see cref="StyleSelector"/>. Selects style by simulation item type.
    /// </summary>
    public class SimulationItemStyleSelector : StyleSelector
    {
        /// <summary>
        /// Gets or sets simulation node style.
        /// </summary>
        public Style? SimulationNodeStyle { get; set; }

        /// <summary>
        /// Gets or sets simulation connection style.
        /// </summary>
        public Style? SimulationConnectionStyle { get; set; }

        /// <summary>
        /// Gets or sets simulation annotation style.
        /// </summary>
        public Style? SimulationAnnotationStyle { get; set; }

        /// <inheritdoc/>
        public override Style? SelectStyle(object item, DependencyObject container)
        {
            return item switch
            {
                SimulationNode => SimulationNodeStyle,
                ConnectionModel => SimulationConnectionStyle,
                TextFigureModel => SimulationAnnotationStyle,
                _ => null
            };
        }
    }
}
