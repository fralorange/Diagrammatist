namespace Diagrammatist.Presentation.WPF.Simulator.Models.Engine
{
    /// <summary>
    /// An interface that defines simulation engine operations.
    /// </summary>
    public interface ISimulationEngine
    {
        /// <summary>
        /// Gets or sets time interval between simulation steps.
        /// </summary>
        TimeSpan SimulationTime { get; set; }
        /// <summary>
        /// Starts simulation.
        /// </summary>
        void Start();
        /// <summary>
        /// Stops simulation.
        /// </summary>
        void Stop();
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
