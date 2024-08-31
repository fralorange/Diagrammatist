using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using DiagramApp.Application.AppServices.Contexts.Canvas.Services;
using DiagramApp.Contracts.Canvas;
using DiagramApp.Contracts.Settings;
using DiagramApp.Presentation.WPF.Messages;
using System.Collections.ObjectModel;
using System.Windows.Threading;

namespace DiagramApp.Presentation.WPF.ViewModels
{
    /// <summary>
    /// A view model class for canvas component.
    /// </summary>
    public sealed partial class TabsViewModel : ObservableRecipient, IRecipient<NewCanvasSettingsMessage>
    {
        private readonly ICanvasManipulationService _canvasManipulationService;

        /// <summary>
        /// Gets or sets collection of <see cref="CanvasDto"/>.
        /// </summary>
        /// <remarks>
        /// This property used to store canvases in tabs UI.
        /// </remarks>
        public ObservableCollection<CanvasDto> Canvases { get; } = [];

        [ObservableProperty]
        [NotifyPropertyChangedRecipients]
        private CanvasDto? _selectedCanvas;

        public TabsViewModel(ICanvasManipulationService canvasManipulationService)
        {
            _canvasManipulationService = canvasManipulationService;

            IsActive = true;
        }

        /// <summary>
        /// Creates new <see cref="CanvasDto"/> and adds it to <see cref="Canvases"/>
        /// </summary>
        /// <param name="settings">Diagram settings.</param>
        private async Task CreateCanvas(DiagramSettingsDto settings)
        {
            var canvas = await _canvasManipulationService.CreateCanvasAsync(settings);

            Canvases.Add(canvas);
            SelectedCanvas = canvas;
        }

        /// <inheritdoc/>
        public void Receive(NewCanvasSettingsMessage message)
        {
            Dispatcher.CurrentDispatcher.Invoke(async () =>
            {
                await CreateCanvas(message.Value);
            });
        }
    }
}
