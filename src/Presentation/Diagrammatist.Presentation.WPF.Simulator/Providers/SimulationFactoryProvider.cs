using Diagrammatist.Presentation.WPF.Core.Models.Canvas;
using Diagrammatist.Presentation.WPF.Simulator.Factories.Flowchart;
using Diagrammatist.Presentation.WPF.Simulator.Interfaces;

namespace Diagrammatist.Presentation.WPF.Simulator.Providers
{
    /// <summary>
    /// A class that provides required factory.
    /// </summary>
    public static class SimulationFactoryProvider
    {
        /// <summary>
        /// Gets factory by diagram type.
        /// </summary>
        /// <param name="modelType"></param>
        /// <returns></returns>
        public static ISimulationFactory GetFactory(DiagramsModel modelType)
        {
            return modelType switch
            {
                DiagramsModel.Flowchart => new FlowchartSimulationFactory(),
                _ => throw new ArgumentException("Unsupported diagram type."),
            };
        }
    }
}
