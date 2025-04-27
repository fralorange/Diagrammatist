using CommunityToolkit.Mvvm.ComponentModel;
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
    public abstract partial class SimulationNode : ObservableObject
    {
        /// <summary>
        /// Decorated figure.
        /// </summary>
        public required FigureModel Figure { get; set; }

        /// <summary>
        /// Gets or sets lua script string value.
        /// </summary>
        /// <remarks>
        /// This property used to store current simulation node script.
        /// </remarks>
        public abstract string LuaScript { get; set; }

        /// <summary>
        /// Gets or sets optional external file path (e.g., nested dgmf).
        /// </summary>
        /// <remarks>
        /// This property used to add data source.
        /// </remarks>
        [ObservableProperty]
        private string? _externalFilePath;
    }
}
