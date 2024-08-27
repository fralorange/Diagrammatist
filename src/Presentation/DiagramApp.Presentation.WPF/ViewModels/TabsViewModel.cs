using CommunityToolkit.Mvvm.ComponentModel;
using DiagramApp.Contracts.Canvas;
using System.Collections.ObjectModel;

namespace DiagramApp.Presentation.WPF.ViewModels
{
    /// <summary>
    /// Canvas tabs view model.
    /// </summary>
    public sealed partial class TabsViewModel : ObservableRecipient
    {
        /// <summary>
        /// Collection of <see cref="CanvasDto"/>.
        /// </summary>
        public ObservableCollection<CanvasDto>? Canvases { get; set; }

        [ObservableProperty]
        [NotifyPropertyChangedRecipients]
        private CanvasDto? _selectedCanvas;
    }
}
