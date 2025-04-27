using Diagrammatist.Presentation.WPF.Simulator.Models.Node;

namespace Diagrammatist.Presentation.WPF.Simulator.Models.Engine
{
    /// <summary>
    /// An interface that defines simulation engine operations.
    /// </summary>
    public interface ISimulationEngine
    {
        /// <summary>
        /// An event that occurs when current node changes.
        /// </summary>
        event EventHandler<SimulationNode?> CurrentNodeChanged;
        /// <summary>
        /// Gets simulation engine condition whether simulation was completed or not.
        /// </summary>
        /// <remarks>
        /// In terms of 'completed' it is assumed that the last node of the graph has been reached.
        /// </remarks>
        bool IsCompleted { get; }
        /// <summary>
        /// Initializes simulation engine.
        /// </summary>
        void Initialize();
        /// <summary>
        /// Iterates over simulation, until it is completed.
        /// </summary>
        /// <param name="args">An arguments.</param>
        /// <returns>An <c>object</c>, if process returns something and is valid, otherwise <c>null</c>.</returns>
        object[]? Simulate(params object[] args);
        /// <summary>
        /// Takes the simulation one step forward.
        /// </summary>
        void StepForward();
        /// <summary>
        /// Takes the simulation one step backward.
        /// </summary>
        void StepBackward();
        /// <summary>
        /// Resets simulation.
        /// </summary>
        void Reset();
    }
}
