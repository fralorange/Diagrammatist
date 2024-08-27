using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using DiagramApp.Contracts.Canvas;

namespace DiagramApp.Presentation.WPF.ViewModels
{
    /// <summary>
    /// Canvas view model.
    /// </summary>
    public sealed class CanvasViewModel : ObservableRecipient, IRecipient<PropertyChangedMessage<CanvasDto>>
    {
        private CanvasDto? _currentCanvas;

        /// <summary>
        /// Current canvas.
        /// </summary>
        public CanvasDto? CurrentCanvas
        {
            get => _currentCanvas;
            private set => SetProperty(ref _currentCanvas, value, true);
        }

        /// <inheritdoc/>
        public void Receive(PropertyChangedMessage<CanvasDto> message)
        {
            if (message.Sender.GetType() == typeof(TabsViewModel) &&
                message.PropertyName == nameof(TabsViewModel.SelectedCanvas))
            {
                CurrentCanvas = message.NewValue;
            }
        }
    }
}
