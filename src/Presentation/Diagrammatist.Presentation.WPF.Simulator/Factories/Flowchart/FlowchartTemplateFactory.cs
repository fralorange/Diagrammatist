using Diagrammatist.Presentation.WPF.Core.Models.Figures.Special.Flowchart;
using Diagrammatist.Presentation.WPF.Simulator.Interfaces;
using Diagrammatist.Presentation.WPF.Simulator.Models.Node;
using System.Windows;

namespace Diagrammatist.Presentation.WPF.Simulator.Factories.Flowchart
{
    /// <summary>
    /// A class that implements <see cref="ITemplateFactory"/>. Provides with template creation operations for flowchart nodes.
    /// </summary>
    public class FlowchartTemplateFactory : ITemplateFactory
    {
        private readonly DataTemplate _emptyTemplate = new();
        /// <summary>
        /// Gets or sets flowchart simulation node template.
        /// </summary>
        public DataTemplate? FlowchartSimulationNodeTemplate { get; }
        /// <summary>
        /// Gets or sets flowchart simulation node with file template.
        /// </summary>
        public DataTemplate? FlowchartSimulationFileNodeTemplate { get; }

        /// <summary>
        /// Initializes factory.
        /// </summary>
        /// <param name="nodeTemplate">Flowchart simulation node template.</param>
        /// <param name="fileNodeTemplate">Flowchart simulation node with file template.</param>
        public FlowchartTemplateFactory(DataTemplate? nodeTemplate, DataTemplate? fileNodeTemplate)
        {
            FlowchartSimulationNodeTemplate = nodeTemplate;
            FlowchartSimulationFileNodeTemplate = fileNodeTemplate;
        }

        /// <inheritdoc/>
        public DataTemplate CreateTemplate(SimulationNode node)
        {
            if (node.Figure is FlowchartFigureModel flowchart)
            {
                return flowchart.Subtype switch
                {
                    FlowchartSubtypeModel.PredefinedProcess => FlowchartSimulationFileNodeTemplate ?? _emptyTemplate,

                    FlowchartSubtypeModel.Process
                        or FlowchartSubtypeModel.InputOutput
                        or FlowchartSubtypeModel.Decision
                        or FlowchartSubtypeModel.Preparation
                        => FlowchartSimulationNodeTemplate ?? _emptyTemplate,

                    _ => _emptyTemplate
                };
            }

            return _emptyTemplate;
        }
    }
}
