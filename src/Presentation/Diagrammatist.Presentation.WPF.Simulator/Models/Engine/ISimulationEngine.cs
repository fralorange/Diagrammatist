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
        /// Initializes simulation engine.
        /// </summary>
        void Initialize();
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
