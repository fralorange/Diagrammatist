using Diagrammatist.Presentation.WPF.Core.Models.Figures.Special.Flowchart;
using Diagrammatist.Presentation.WPF.Simulator.Models.Node;
using System.Windows;
using System.Windows.Controls;

namespace Diagrammatist.Presentation.WPF.Simulator.Selectors
{
    /// <summary>
    /// A class that derives from <see cref="DataTemplateSelector"/>. Selects template by node type.
    /// </summary>
    public class SimulationScriptSelector : DataTemplateSelector
    {
        private DataTemplate _emptyTemplate = new();

        /// <summary>
        /// Gets or sets flowchart simulation node template.
        /// </summary>
        public DataTemplate? FlowchartSimulationNodeTemplate { get; set; }

        /// <summary>
        /// Gets or sets flowchart simulation node with file template.
        /// </summary>
        public DataTemplate? FlowchartSimulationNodeFileTemplate { get; set; }

        /// <inheritdoc/>
        public override DataTemplate? SelectTemplate(object item, DependencyObject container)
        {
            if (item is SimulationNodeBase { Figure: var figure })
            {
                return figure switch
                {
                    FlowchartFigureModel flowchart when flowchart.Subtype is FlowchartSubtypeModel.PredefinedProcess
                        => FlowchartSimulationNodeFileTemplate,

                    FlowchartFigureModel flowchart when flowchart.Subtype is FlowchartSubtypeModel.Process
                                                                or FlowchartSubtypeModel.InputOutput
                                                                or FlowchartSubtypeModel.Decision
                                                                or FlowchartSubtypeModel.Preparation
                        => FlowchartSimulationNodeTemplate,

                    _ => _emptyTemplate
                };
            }

            return null;
        }
    }
}
