using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DiagramApp.Client.ViewModels.Wrappers;
using DiagramApp.Domain.Canvas;
using DiagramApp.Domain.DiagramSettings;
using System.Collections.ObjectModel;

namespace DiagramApp.Client.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly IPopupService _popupService;

        private int _canvasCounter = 1;
        public ObservableCollection<ObservableCanvas> Canvases { get; set; } = new();

        [ObservableProperty]
        private ObservableCanvas? _currentCanvas;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsCanvasNotNull))]
        private bool _isCanvasNull = true;

        public bool IsCanvasNotNull => !IsCanvasNull;

        public MainViewModel(IPopupService popupService)
            => _popupService = popupService;

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
        private async Task SelectCanvasAsync(ObservableCanvas selectedCanvas)
        {
            if (CurrentCanvas == selectedCanvas)
            {
                CurrentCanvas = null;
                IsCanvasNull = true;
            }
            else
            {
                CurrentCanvas = null;
                await Task.Delay(20); // KLUDGE!!!!! // UI Updates in milliseconds
                CurrentCanvas = selectedCanvas;
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
    }
}
