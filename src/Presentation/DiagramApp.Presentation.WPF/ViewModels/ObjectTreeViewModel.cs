using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using DiagramApp.Contracts.Figures;
using System.Collections.ObjectModel;

namespace DiagramApp.Presentation.WPF.ViewModels
{
    /// <summary>
    /// A view model class for object tree (explorer) component.
    /// </summary>
    public sealed partial class ObjectTreeViewModel : ObservableRecipient, IRecipient<PropertyChangedMessage<object>>
    {
        private ObservableCollection<FigureDto>? _figures;

        /// <summary>
        /// Gets collection of <see cref="FigureDto"/>
        /// </summary>
        /// <remarks>
        /// This property used to display figures in UI.
        /// </remarks>
        public ObservableCollection<FigureDto>? Figures
        {
            get => _figures;
            private set => SetProperty(ref  _figures, value);
        }

        private FigureDto? _selectedFigure;

        /// <summary>
        /// Gets selected figure from collection <see cref="Figures"/>
        /// </summary>
        /// <remarks>
        /// This property used to store selected figure.
        /// </remarks>
        public FigureDto? SelectedFigure
        {
            get => _selectedFigure;
            private set => SetProperty(ref _selectedFigure, value, true);
        }

        /// <inheritdoc/>
        public void Receive(PropertyChangedMessage<object> message)
        {
            if (message.Sender.GetType() != typeof(CanvasViewModel)) return;

            switch (message.PropertyName)
            {
                case nameof(CanvasViewModel.Figures):
                    Figures = message.NewValue as ObservableCollection<FigureDto>;
                    break;
                case nameof(CanvasViewModel.SelectedFigure):
                    SelectedFigure = message.NewValue as FigureDto;
                    break;
            }
        }
    }
}
