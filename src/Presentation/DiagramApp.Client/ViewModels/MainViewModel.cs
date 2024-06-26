﻿using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DiagramApp.Application.AppServices.Helpers;
using DiagramApp.Application.AppServices.Services;
using DiagramApp.Client.ViewModels.Wrappers;
using DiagramApp.Domain.Canvas;
using DiagramApp.Domain.DiagramSettings;
using System.Collections.ObjectModel;
// TO-DO: Rethink some methods, combine some similar methods in one, move some methods in ObservableCanvas 
namespace DiagramApp.Client.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly IPopupService _popupService;
        private readonly IToolboxService _toolboxService;

        private int _canvasCounter = 0;
        public ObservableCollection<ObservableCanvas> Canvases { get; set; } = [];

        [ObservableProperty]
        private ObservableCanvas? _currentCanvas;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsCanvasNotNull))]
        private bool _isCanvasNull = true;
        public bool IsCanvasNotNull => !IsCanvasNull;

        [ObservableProperty]
        private ToolboxViewModel _toolboxViewModel;

        public MainViewModel(IPopupService popupService, IToolboxService toolboxService)
        {
            _popupService = popupService;
            _toolboxService = toolboxService;

            _toolboxViewModel = new(_toolboxService);
        }

        [RelayCommand]
        private async Task ViewProgramAboutAsync() => await _popupService.ShowPopupAsync<AboutPopupViewModel>(CancellationToken.None);

        [RelayCommand]
        private void Undo() => CurrentCanvas?.Undo();

        [RelayCommand]
        private void Redo() => CurrentCanvas?.Redo();

        [RelayCommand]
        private async Task CreateCanvasAsync()
        {
            var result = await _popupService.ShowPopupAsync<NewDiagramPopupViewModel>(CancellationToken.None);
            if (result is DiagramSettings settings)
            {
                if (string.IsNullOrEmpty(settings.FileName))
                    settings.FileName = $"Безымянный{++_canvasCounter}";
                Canvas canvas = new(settings);
                ObservableCanvas observableCanvas = new(canvas);

                Canvases.Add(observableCanvas);
                _ = SelectCanvasAsync(observableCanvas);
            }
        }

        [RelayCommand]
        private async Task EditCanvasAsync()
        {
            if (CurrentCanvas is not null)
            {
                var canvasSettings = CurrentCanvas.Settings;
                var currentSettings = new DiagramSettings()
                {
                    FileName = canvasSettings.FileName,
                    Background = canvasSettings.Background,
                    Type = canvasSettings.Type,
                    Width = canvasSettings.Width,
                    Height = canvasSettings.Height,
                };

                var result = await _popupService.ShowPopupAsync<ChangeDiagramSizePopupViewModel>(viewModel => viewModel.Settings = canvasSettings, CancellationToken.None);
                var action = new Action(() =>
                {
                    if (result is DiagramSettings settings)
                    {
                        CurrentCanvas.UpdateSettings(settings);
                    }
                });

                var undoAction = new Action(() =>
                {
                    CurrentCanvas.UpdateSettings(currentSettings);
                });

                UndoableCommandHelper.ExecuteAction(CurrentCanvas, action, undoAction);
            }
        }

        [RelayCommand]
        private async Task SelectCanvasAsync(ObservableCanvas selectedCanvas)
        {
            if (CurrentCanvas == selectedCanvas)
            {
                CurrentCanvas.IsSelected = false;
                CurrentCanvas = null;
                IsCanvasNull = true;
            }
            else
            {
                if (CurrentCanvas is not null)
                {
                    CurrentCanvas.IsSelected = false;
                }

                CurrentCanvas = null;
                await Task.Delay(20); // KLUDGE!!!!! // UI Updates in milliseconds
                CurrentCanvas = selectedCanvas;
                CurrentCanvas.IsSelected = true;
                IsCanvasNull = false;
            }
        }

        [RelayCommand]
        private void CloseCanvas(ObservableCanvas targetCanvas)
        {
            Canvases.Remove(targetCanvas);
            CurrentCanvas = null;
            IsCanvasNull = true;
        }
        //maybe try create some what of factory for this like:
        [RelayCommand]
        private void DeleteItemFromCanvas(ObservableFigure figure)
        {
            var currentAction = new Action(() => CurrentCanvas!.Figures.Remove(figure));
            var undoAction = new Action(() => CurrentCanvas!.Figures.Add(figure));
            UndoableCommandHelper.ExecuteAction(CurrentCanvas!, currentAction, undoAction);
        }

        [RelayCommand]
        private void SelectItemInCanvas(ObservableFigure figure)
        {
            CurrentCanvas!.SelectFigure(figure);
        }

        [RelayCommand]
        private void ResetItemInCanvas()
        {
            if (CurrentCanvas is not null && CurrentCanvas.SelectedFigure is not null)
                CurrentCanvas!.DeselectFigure();
        }
        // refactor this spot l8r 
        [RelayCommand]
        private void ZoomIn()
        {
            if (CurrentCanvas is null)
                return;

            CurrentCanvas.ZoomIn(0.1);
        }

        [RelayCommand]
        private void ZoomOut()
        {
            if (CurrentCanvas is null)
                return;

            CurrentCanvas.ZoomOut(0.1);
        }

        [RelayCommand]
        private void ZoomReset()
        {
            if (CurrentCanvas is null)
                return;

            CurrentCanvas.ZoomReset();
        }

        [RelayCommand]
        private void MoveCanvas(ScrolledEventArgs e)
        {
            if (CurrentCanvas is null)
                return;

            CurrentCanvas.MoveCanvas(e.ScrollX, e.ScrollY);
        }

        [RelayCommand]
        private void ChangeControls(string controlName)
        {
            if (CurrentCanvas is null)
                return;

            CurrentCanvas.ChangeControls(controlName);
        }

        [RelayCommand]
        private void Rotate(double degrees)
        {
            if (CurrentCanvas is null)
                return;

            CurrentCanvas.Rotation += degrees;
        }
    }
}
