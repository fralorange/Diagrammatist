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
        /// Gets or sets simulation node that caused the error.
        /// </summary>
        public SimulationNode? Node { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimulationErrorEventArgs"/> class.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="node"></param>
        public SimulationErrorEventArgs(string message, SimulationNode? node = null)
        {
            Message = message;
            Node = node;
        }
    }
}
