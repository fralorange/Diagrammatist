using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DiagramApp.Application.AppServices.Services;
using DiagramApp.Client.ViewModels.Wrappers;
using DiagramApp.Client.Views;
using DiagramApp.Domain.Canvas;
using DiagramApp.Domain.DiagramSettings;
using System.Collections.ObjectModel;

namespace DiagramApp.Client.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly IPopupService _popupService;
        private readonly IToolboxService _toolboxService;

        private int _canvasCounter = 1;
        public ObservableCollection<ObservableCanvas> Canvases { get; set; } = new();

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
        private async Task CreateCanvasAsync()
        {
            var result = await _popupService.ShowPopupAsync<NewDiagramPopupViewModel>(CancellationToken.None);
            if (result is DiagramSettings settings)
            {
                if (string.IsNullOrEmpty(settings.FileName))
                    settings.FileName = $"Безымянный{_canvasCounter++}";
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
                var result = await _popupService.ShowPopupAsync<ChangeDiagramSizePopupViewModel>(viewModel => viewModel.Settings = CurrentCanvas.Settings, CancellationToken.None);
                if (result is DiagramSettings settings)
                {
                    CurrentCanvas.UpdateSettings(settings);
                }
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

        [RelayCommand]
        private void DeleteItemFromCanvas(ObservableFigure figure)
        {
            CurrentCanvas!.Figures.Remove(figure);
        }

        [RelayCommand]
        private void SelectItemInCanvas(ObservableFigure figure)
        {
            CurrentCanvas!.SelectFigure(figure);
        }

        [RelayCommand]
        private void ResetItemInCanvas()
        {
            CurrentCanvas!.DeselectFigure();
        }

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
