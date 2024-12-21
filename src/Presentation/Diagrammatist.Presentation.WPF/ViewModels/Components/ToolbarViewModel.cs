using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Diagrammatist.Presentation.WPF.Models.Canvas;
using Diagrammatist.Presentation.WPF.ViewModels.Components.Enums.Modes;

namespace Diagrammatist.Presentation.WPF.ViewModels.Components
{
    /// <summary>
    /// A view model class for toolbar component.
    /// </summary>
    public sealed partial class ToolbarViewModel : ObservableRecipient, IRecipient<PropertyChangedMessage<CanvasModel?>>
    {
        private CanvasModel? _currentCanvas;
        private CanvasModel? CurrentCanvas
        {
            get => _currentCanvas;
            set => SetProperty(ref _currentCanvas, value);
        }

        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Behaviors/Member[@name="CurrentMouseMode"]/*'/>
        [ObservableProperty]
        [NotifyPropertyChangedRecipients]
        private MouseMode _currentMouseMode;

        public ToolbarViewModel()
        {
            IsActive = true;
        }

        /// <inheritdoc/>
        public void Receive(PropertyChangedMessage<CanvasModel?> message)
        {
            if (message.Sender.GetType() == typeof(CanvasViewModel) &&
                message.PropertyName == nameof(CanvasViewModel.CurrentCanvas))
            {
                CurrentCanvas = message.NewValue;
            }
        }

        [RelayCommand]
        private void ChangeMode(string mode)
        {
            CurrentMouseMode = (MouseMode)Enum.Parse(typeof(MouseMode), mode);
        }
    }
}
