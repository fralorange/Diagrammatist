using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Diagrammatist.Contracts.Figures;

namespace Diagrammatist.Presentation.WPF.ViewModels.Components
{
    /// <summary>
    /// A view model class for properties component.
    /// </summary>
    public sealed class PropertiesViewModel : ObservableRecipient, IRecipient<PropertyChangedMessage<FigureDto?>>
    {
        private FigureDto? _currentFigure;

        /// <summary>
        /// Gets current figure.
        /// </summary>
        /// <remarks>
        /// This property used to display current figure in UI.
        /// </remarks>
        public FigureDto? CurrentFigure
        {
            get => _currentFigure;
            private set => SetProperty(ref _currentFigure, value);
        }

        public PropertiesViewModel()
        {
            IsActive = true;
        }

        /// <inheritdoc/>
        public void Receive(PropertyChangedMessage<FigureDto?> message)
        {
            if (message.Sender.GetType() == typeof(ObjectTreeViewModel) &&
                message.PropertyName == nameof(ObjectTreeViewModel.SelectedFigure))
            {
                CurrentFigure = message.NewValue;
            }
        }
    }
}
