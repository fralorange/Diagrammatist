using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using DiagramApp.Contracts.Figures;

namespace DiagramApp.Presentation.WPF.ViewModels
{
    /// <summary>
    /// Properties view model.
    /// </summary>
    public sealed class PropertiesViewModel : ObservableRecipient, IRecipient<PropertyChangedMessage<FigureDto>>
    {
        private FigureDto? _currentFigure;

        /// <summary>
        /// Current figure.
        /// </summary>
        public FigureDto? CurrentFigure
        {
            get => _currentFigure;
            private set => SetProperty(ref _currentFigure, value);
        }

        /// <inheritdoc/>
        public void Receive(PropertyChangedMessage<FigureDto> message)
        {
            if (message.Sender.GetType() == typeof(ObjectTreeViewModel) &&
                message.PropertyName == nameof(ObjectTreeViewModel.SelectedFigure))
            {
                CurrentFigure = message.NewValue;
            }
        }
    }
}
