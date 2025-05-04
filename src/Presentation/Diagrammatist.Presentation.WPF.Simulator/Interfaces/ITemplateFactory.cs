using Diagrammatist.Presentation.WPF.Simulator.Models.Node;
using System.Windows;

namespace Diagrammatist.Presentation.WPF.Simulator.Interfaces
{
    /// <summary>
    /// An interface that provides template creation operations.
    /// </summary>
    public interface ITemplateFactory
    {
        /// <summary>
        /// Creates template for simulation node base.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        DataTemplate CreateTemplate(SimulationNode node);
    }
}
