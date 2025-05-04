using CommunityToolkit.Mvvm.Input;
using System.Windows.Media;

namespace Diagrammatist.Presentation.WPF.ViewModels.Components
{
    /// <summary>
    /// A view model class for specific action items.
    /// </summary>
    public class ActionItemViewModel
    {
        /// <summary>
        /// Tooltip text for the action item.
        /// </summary>
        public string Tooltip { get; }
        /// <summary>
        /// Command to execute when the action item is clicked.
        /// </summary>
        public IRelayCommand Command { get; }
        /// <summary>
        /// Icon data for the action item, represented as a Geometry object.
        /// </summary>
        public Geometry IconData { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionItemViewModel"/> class with the specified tooltip, command, and icon data.
        /// </summary>
        /// <param name="tooltip"></param>
        /// <param name="command"></param>
        /// <param name="iconData"></param>
        public ActionItemViewModel(string tooltip, IRelayCommand command, Geometry iconData)
        {
            Tooltip = tooltip;
            Command = command;
            IconData = iconData;
        }
    }
}
