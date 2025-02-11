using Diagrammatist.Presentation.WPF.Core.Models.Figures;

namespace Diagrammatist.Presentation.WPF.Simulator.Models.Node
{
    /// <summary>
    /// A class that derives from <see cref="FigureModel"/>. This class defines simulation node properties.
    /// </summary>
    /// <remarks>
    /// This class used as decorator of <see cref="FigureModel"/> and adds 
    /// additional functionality for the simulation engine so it can work properly.
    /// </remarks>
    public abstract class SimulationNodeBase
    {
        /// <summary>
        /// Decorated figure.
        /// </summary>
        public FigureModel? Figure { get; init; }

        /// <summary>
        /// Gets or sets lua script string value.
        /// </summary>
        /// <remarks>
        /// This property used to store current simulation node script.
        /// </remarks>
        public abstract string LuaScript { get; set; }
    }
}
