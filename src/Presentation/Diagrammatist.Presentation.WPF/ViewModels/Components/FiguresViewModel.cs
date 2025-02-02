﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Diagrammatist.Application.AppServices.Figures.Services;
using Diagrammatist.Presentation.WPF.Core.Commands.Helpers.Undoable;
using Diagrammatist.Presentation.WPF.Core.Commands.Managers;
using Diagrammatist.Presentation.WPF.Core.Foundation.Base.ObservableObject.Args;
using Diagrammatist.Presentation.WPF.Core.Mappers.Figures;
using Diagrammatist.Presentation.WPF.Core.Messaging.Messages;
using Diagrammatist.Presentation.WPF.Core.Models.Figures;
using System.Collections.ObjectModel;

namespace Diagrammatist.Presentation.WPF.ViewModels.Components
{
    /// <summary>
    /// A view model class for figures (toolbox) component.
    /// </summary>
    public sealed partial class FiguresViewModel : ObservableRecipient
    {
        private readonly IFigureService _figureService;
        private readonly ITrackableCommandManager _trackableCommandManager;

        /// <summary>
        /// Flag that determines whether figure changes in the moment or not.
        /// </summary>
        private bool _figureChanges;

        /// <summary>
        /// Gets or sets <see cref="ObservableCollection{T}"/> of <see cref="FigureModel"/>.
        /// </summary>
        /// <remarks>
        /// This property used to store figures that placed on canvas.
        /// </remarks>
        [ObservableProperty]
        private ObservableCollection<FigureModel>? _canvasFigures;

        /// <summary>
        /// Gets or sets dictionary with string keys and <see cref="List{T}"/> of <see cref="FigureModel"/> pairs.
        /// </summary>
        /// <remarks>
        /// This property used to store toolbox figures
        /// </remarks>
        [ObservableProperty]
        private Dictionary<string, List<FigureModel>>? _figures;

        /// <summary>
        /// Gets or sets key value pair of string to <see cref="List{T}"/> of <see cref="FigureModel"/>.
        /// </summary>
        /// <remarks>
        /// This property used to show filtered <see cref="_figures"/> by category.
        /// </remarks>
        [ObservableProperty]
        private KeyValuePair<string, List<FigureModel>> _selectedCategory;

        /// <summary>
        /// Gets or sets <see cref="FigureModel"/> selected figure.
        /// </summary>
        /// <remarks>
        /// This property used to get selected figure from toolbox.
        /// </remarks>
        [ObservableProperty]
        private FigureModel? _selectedFigure;

        /// <summary>
        /// Initializes a new figures view model.
        /// </summary>
        /// <param name="figureService">A figure service.</param>
        /// <param name="trackableCommandManager">A command manager.</param>
        public FiguresViewModel(IFigureService figureService, ITrackableCommandManager trackableCommandManager)
        {
            _figureService = figureService;
            _trackableCommandManager = trackableCommandManager;

            IsActive = true;
        }

        /// <summary>
        /// Loads figures in <see cref="Figures"/> externally and asynchronously.
        /// </summary>
        public async Task LoadFiguresAsync()
        {
            var figureDomains = await _figureService.GetAsync();

            var figureModels = figureDomains.ToDictionary(
                pair => pair.Key,
                pair => pair.Value.Select(figure =>
                {
                    var model = figure.ToModel();
                    var color = (System.Windows.Media.Color)App.Current.Resources["AppBackground"];
                    model.BackgroundColor = System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);

                    return model;
                }).ToList()
            );

            Figures = figureModels;
        }

        private void UpdateFigureColors()
        {
            if (Figures is null)
                return;

            var newColor = (System.Windows.Media.Color)App.Current.Resources["AppBackground"];
            var updatedColor = System.Drawing.Color.FromArgb(newColor.A, newColor.R, newColor.G, newColor.B);

            foreach (var category in Figures.Values)
            {
                foreach (var figure in category)
                {
                    figure.BackgroundColor = updatedColor;
                }
            }
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

                figure.ExtendedPropertyChanged += OnPropertyChanged;

                var command = CommonUndoableHelper.CreateUndoableCommand(
                    () => CanvasFigures.Add(figure),
                    () => CanvasFigures.Remove(figure)
                );

                _trackableCommandManager.Execute(command);
            }
            SelectedFigure = null;
        }

        private void OnPropertyChanged(object? sender, ExtendedPropertyChangedEventArgs e)
        {
            if (sender is FigureModel figure && e.PropertyName is not null && !_figureChanges)
            {
                var command = CommonUndoableHelper.CreateUndoableCommand(
                    () => SetFigureProperty(figure, e.PropertyName, e.NewValue),
                    () => SetFigureProperty(figure, e.PropertyName, e.OldValue)
                );

                _trackableCommandManager.Execute(command);
            }
        }

        private void SetFigureProperty(FigureModel figure, string propertyName, object? value)
        {
            _figureChanges = true;
            try
            {
                var property = figure.GetType().GetProperty(propertyName);

                property?.SetValue(figure, value);
            }
            finally
            {
                _figureChanges = false;
            }
        }

        /// <inheritdoc/>
        protected override void OnActivated()
        {
            base.OnActivated();

            Messenger.Register<FiguresViewModel, ThemeChangedMessage>(this, (r, m) => UpdateFigureColors());

            Messenger.Register<FiguresViewModel, PropertyChangedMessage<ObservableCollection<FigureModel>?>>(this, (r, m) =>
            {
                if (m.Sender.GetType() == typeof(CanvasViewModel) &&
                    m.PropertyName == nameof(CanvasViewModel.CurrentCanvas.Figures))
                {
                    CanvasFigures = m.NewValue;
                }
            });
        }
    }
}
