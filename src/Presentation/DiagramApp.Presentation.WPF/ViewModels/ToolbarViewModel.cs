using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging.Messages;
using CommunityToolkit.Mvvm.Messaging;
using DiagramApp.Contracts.Canvas;
using DiagramApp.Presentation.WPF.ViewModels.Enums.Modes;

namespace DiagramApp.Presentation.WPF.ViewModels
{
    /// <summary>
    /// A view model class for toolbar component.
    /// </summary>
    public sealed partial class ToolbarViewModel : ObservableRecipient, IRecipient<PropertyChangedMessage<CanvasDto>>
    {
        private CanvasDto? _currentCanvas;
        private CanvasDto? CurrentCanvas
        {
            get => _currentCanvas;
            set => SetProperty(ref _currentCanvas, value);
        }

        /// <summary>
        /// Gets or sets current mouse mode.
        /// </summary>
        /// <remarks>
        /// This property used to configure current mouse mode. <br/>
        /// For details see <see cref="MouseMode"/>
        /// </remarks>
        [ObservableProperty]
        [NotifyPropertyChangedRecipients]
        private MouseMode _currentMouseMode;

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
