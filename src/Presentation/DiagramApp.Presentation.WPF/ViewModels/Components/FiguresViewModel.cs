using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using DiagramApp.Application.AppServices.Contexts.Figures.Services;
using DiagramApp.Contracts.Canvas;

namespace DiagramApp.Presentation.WPF.ViewModels.Components
{
    /// <summary>
    /// A view model class for figures (toolbox) component.
    /// </summary>
    public sealed class FiguresViewModel : ObservableRecipient, IRecipient<PropertyChangedMessage<CanvasDto>>
    {
        private IFigureService _figureService;

        private CanvasDto? _currentCanvas;
        private CanvasDto? CurrentCanvas
        {
            get => _currentCanvas;
            set => SetProperty(ref _currentCanvas, value);
        }

        /// <summary>
        /// Initializes a new figures view model.
        /// </summary>
        /// <param name="figureService">A figure service.</param>
        public FiguresViewModel(IFigureService figureService)
        {
            _figureService = figureService;
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
