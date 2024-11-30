using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Diagrammatist.Application.AppServices.Contexts.Figures.Services;
using Diagrammatist.Infrastructure.ComponentRegistrar.Contexts.Figures.Cloners;
using Diagrammatist.Contracts.Figures;
using System.Collections.ObjectModel;
using Diagrammatist.Presentation.WPF.Framework.Commands.Undoable.Manager;
using Diagrammatist.Presentation.WPF.Framework.Commands.Undoable.Helpers;

namespace Diagrammatist.Presentation.WPF.ViewModels.Components
{
    /// <summary>
    /// A view model class for figures (toolbox) component.
    /// </summary>
    public sealed partial class FiguresViewModel : ObservableRecipient, IRecipient<PropertyChangedMessage<ObservableCollection<FigureDto>?>>
    {
        private IFigureService _figureService;
        private IUndoableCommandManager _undoableCommandManager;

        [ObservableProperty]
        private ObservableCollection<FigureDto>? _canvasFigures;

        [ObservableProperty]
        private Dictionary<string, List<FigureDto>>? _figures;

        [ObservableProperty]
        private KeyValuePair<string, List<FigureDto>> _selectedCategory;

        [ObservableProperty]
        private FigureDto? _selectedFigure;

        /// <summary>
        /// Initializes a new figures view model.
        /// </summary>
        /// <param name="figureService">A figure service.</param>
        /// <param name="undoableCommandManager">A undoable command manager.</param>
        public FiguresViewModel(IFigureService figureService, IUndoableCommandManager undoableCommandManager)
        {
            _figureService = figureService;
            _undoableCommandManager = undoableCommandManager;

            IsActive = true;
        }

        /// <inheritdoc/>
        public void Receive(PropertyChangedMessage<ObservableCollection<FigureDto>?> message)
        {
            if (message.Sender.GetType() == typeof(CanvasViewModel) &&
                message.PropertyName == nameof(CanvasViewModel.Figures))
            {
                CanvasFigures = message.NewValue;
            }
        }

        /// <summary>
        /// Loads figures in <see cref="Figures"/> externally and asynchronously.
        /// </summary>
        public async Task LoadFiguresAsync()
        {
            Figures = await _figureService.GetAsync();
        }

        /// <summary>
        /// Adds new figure to canvas component.
        /// </summary>
        [RelayCommand]
        private void AddFigure()
        {
            if (CanvasFigures is not null && SelectedFigure is not null)
            {
                var figure = SelectedFigure.Clone();

                var command = CommonUndoableHelper.CreateUndoableCommand(
                    () => CanvasFigures.Add(figure),
                    () => CanvasFigures.Remove(figure)
                );

                _undoableCommandManager.Execute(command);
            }
            SelectedFigure = null;
        }
    }
}
