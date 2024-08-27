using CommunityToolkit.Mvvm.ComponentModel;
using DiagramApp.Contracts.Canvas;
using System.Collections.ObjectModel;

namespace DiagramApp.Presentation.WPF.ViewModels
{
    /// <summary>
    /// A view model class for canvas component.
    /// </summary>
    public sealed partial class TabsViewModel : ObservableRecipient
    {
        /// <summary>
        /// Gets or sets collection of <see cref="CanvasDto"/>.
        /// </summary>
        /// <remarks>
        /// This property used to store canvases in tabs UI.
        /// </remarks>
        public ObservableCollection<CanvasDto>? Canvases { get; set; }

        [ObservableProperty]
        [NotifyPropertyChangedRecipients]
        private CanvasDto? _selectedCanvas;
    }
}
