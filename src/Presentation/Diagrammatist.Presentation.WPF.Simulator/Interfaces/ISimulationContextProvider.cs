using Diagrammatist.Presentation.WPF.Simulator.Models.Context;

namespace Diagrammatist.Presentation.WPF.Simulator.Interfaces
{
    /// <summary>
    /// An interface that defines with simulation context provider operations.
    /// </summary>
    public interface ISimulationContextProvider
    {
        /// <summary>
        /// Loads simulation context from document file.
        /// </summary>
        /// <param name="path">A file path.</param>
        /// <returns><see cref="SimulationContext"/> extracted from document, if it exists.</returns>
        SimulationContext? Load(string path);
    }
}
