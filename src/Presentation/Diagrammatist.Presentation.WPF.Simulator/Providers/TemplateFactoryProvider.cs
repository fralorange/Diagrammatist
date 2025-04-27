using Diagrammatist.Presentation.WPF.Simulator.Factories.Flowchart;
using Diagrammatist.Presentation.WPF.Simulator.Interfaces;
using Diagrammatist.Presentation.WPF.Simulator.Models.Node;
using Diagrammatist.Presentation.WPF.Simulator.Models.Node.Flowchart;
using System.Windows;

namespace Diagrammatist.Presentation.WPF.Simulator.Providers
{
    /// <summary>
    /// A class that provides required template factory.
    /// </summary>
    public static class TemplateFactoryProvider
    {
        /// <summary>
        /// Gets factory by simulation node base type.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="nodeTemplate"></param>
        /// <param name="fileNodeTemplate"></param>
        /// <returns></returns>
        public static ITemplateFactory GetFactory(SimulationNode node, DataTemplate? nodeTemplate, DataTemplate? fileNodeTemplate)
        {
            return node switch
            {
                FlowchartSimulationNode => new FlowchartTemplateFactory(nodeTemplate, fileNodeTemplate),
                _ => throw new ArgumentException("Unsupported node type."),
            };
        }
    }
}
