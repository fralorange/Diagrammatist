using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging.Messages;
using CommunityToolkit.Mvvm.Messaging;
using DiagramApp.Contracts.Canvas;

namespace DiagramApp.Presentation.WPF.ViewModels
{
    /// <summary>
    /// Toolbar view model.
    /// </summary>
    public sealed class ToolbarViewModel : ObservableRecipient, IRecipient<PropertyChangedMessage<CanvasDto>>
    {
        private CanvasDto? _currentCanvas;

        private CanvasDto? CurrentCanvas
        {
            get => _currentCanvas;
            set => SetProperty(ref _currentCanvas, value);
        }

        /// <inheritdoc/>
        public void Receive(PropertyChangedMessage<CanvasDto> message)
        {
            if (message.Sender.GetType() == typeof(CanvasViewModel) &&
                message.PropertyName == nameof(CanvasViewModel.CurrentCanvas))
            {
                CurrentCanvas = message.NewValue;
            }
        }
    }
}
