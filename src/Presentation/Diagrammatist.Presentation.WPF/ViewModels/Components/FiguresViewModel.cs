using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Diagrammatist.Application.AppServices.Figures.Services;
using Diagrammatist.Presentation.WPF.Framework.Commands.Undoable.Helpers;
using Diagrammatist.Presentation.WPF.Framework.Commands.Undoable.Manager;
using Diagrammatist.Presentation.WPF.Mappers.Figures;
using Diagrammatist.Presentation.WPF.Models.Figures;
using System.Collections.ObjectModel;

namespace Diagrammatist.Presentation.WPF.ViewModels.Components
{
    /// <summary>
    /// A view model class for figures (toolbox) component.
    /// </summary>
    public sealed partial class FiguresViewModel : ObservableRecipient, IRecipient<PropertyChangedMessage<ObservableCollection<FigureModel>?>>
    {
        private IFigureService _figureService;
        private IUndoableCommandManager _undoableCommandManager;

        [ObservableProperty]
        private ObservableCollection<FigureModel>? _canvasFigures;

        [ObservableProperty]
        private Dictionary<string, List<FigureModel>>? _figures;

        [ObservableProperty]
        private KeyValuePair<string, List<FigureModel>> _selectedCategory;

        [ObservableProperty]
        private FigureModel? _selectedFigure;

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
        public void Receive(PropertyChangedMessage<ObservableCollection<FigureModel>?> message)
        {
            if (message.Sender.GetType() == typeof(CanvasViewModel) &&
                message.PropertyName == nameof(CanvasViewModel.CurrentCanvas.Figures))
            {
                CanvasFigures = message.NewValue;
            }
        }

        /// <summary>
        /// Loads figures in <see cref="Figures"/> externally and asynchronously.
        /// </summary>
        public async Task LoadFiguresAsync()
        {
            var figureDomains = await _figureService.GetAsync();

            var figureModels = figureDomains.ToDictionary(
                pair => pair.Key, 
                pair => pair.Value.Select(figure => figure.ToModel()).ToList() 
            );

            Figures = figureModels;
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
