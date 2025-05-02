using Diagrammatist.Presentation.WPF.Simulator.Models.Node;

namespace Diagrammatist.Presentation.WPF.Simulator.Models.Engine.Args
{
    /// <summary>
    /// A class that derived from <see cref="EventArgs"/>. This class defines simulation error event arguments.
    /// </summary>
    public class SimulationErrorEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets error message.
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimulationErrorEventArgs"/> class.
        /// </summary>
        /// <param name="message"></param>
        public SimulationErrorEventArgs(string message)
        {
            Message = message;
        }
    }
}
