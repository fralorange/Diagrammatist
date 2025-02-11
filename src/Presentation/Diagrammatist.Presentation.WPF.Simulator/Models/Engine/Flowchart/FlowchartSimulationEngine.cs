namespace Diagrammatist.Presentation.WPF.Simulator.Models.Engine.Flowchart
{
    /// <summary>
    /// A class that implements <see cref="ISimulationEngine"/>.
    /// </summary>
    public class FlowchartSimulationEngine : ISimulationEngine
    {
        public TimeSpan SimulationTime { get; set; }

        /// <summary>
        /// Initializes flowchart simulation engine.
        /// </summary>
        public FlowchartSimulationEngine()
        {

        }

        /// <inheritdoc/>
        public void Start()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void Stop()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void StepForward()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void StepBackward()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void Reset()
        {
            throw new NotImplementedException();
        }
    }
}
