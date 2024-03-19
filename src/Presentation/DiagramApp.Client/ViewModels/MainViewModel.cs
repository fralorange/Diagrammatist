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

        public ObservableCollection<ObservableCanvas> Canvases { get; set; } = new();

        [ObservableProperty]
        private ObservableCanvas? _currentCanvas;

        public MainViewModel(IPopupService popupService)
            => _popupService = popupService;

        [RelayCommand]
        private async Task CreateCanvasAsync()
        {
            var result = await _popupService.ShowPopupAsync<NewDiagramPopupViewModel>(CancellationToken.None);
            if (result is DiagramSettings settings)
            {
                if (string.IsNullOrEmpty(settings.FileName))
                    settings.FileName = $"Без имени {Canvases.Count + 1}";
                Canvas canvas = new(settings);
                ObservableCanvas observableCanvas = new(canvas);

                Canvases.Add(observableCanvas);
                CurrentCanvas = observableCanvas;
            }
        }

        [RelayCommand]
        private void SelectCanvas(ObservableCanvas selectedCanvas)
        {
            CurrentCanvas = selectedCanvas;
        }

        [RelayCommand]
        private void CloseCanvas(ObservableCanvas targetCanvas)
        {
            Canvases.Remove(targetCanvas);
            CurrentCanvas = null;
        }

        [RelayCommand]
        private void ZoomIn()
        {
            if (CurrentCanvas == null)
                return;

            CurrentCanvas.ZoomIn(0.1);
        }

        [RelayCommand]
        private void ZoomOut()
        {
            if (CurrentCanvas == null)
                return;

            CurrentCanvas.ZoomOut(0.1);
        }
    }
}
