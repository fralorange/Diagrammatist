using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using DiagramApp.Contracts.Canvas;

namespace DiagramApp.Presentation.WPF.ViewModels
{
    /// <summary>
    /// A view model class for figures (toolbox) component.
    /// </summary>
    public sealed class FiguresViewModel : ObservableRecipient, IRecipient<PropertyChangedMessage<CanvasDto>>
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
