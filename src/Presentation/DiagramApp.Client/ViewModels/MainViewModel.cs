using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DiagramApp.Application.AppServices.Helpers;
using DiagramApp.Application.AppServices.Services.Clipboard;
using DiagramApp.Application.AppServices.Services.File;
using DiagramApp.Application.AppServices.Services.Toolbox;
using DiagramApp.Client.Mappers.Canvas;
using DiagramApp.Client.Mappers.Figure;
using DiagramApp.Client.Platforms.Windows.Handlers;
using DiagramApp.Client.ViewModels.Wrappers;
using DiagramApp.Domain.Canvas;
using DiagramApp.Domain.DiagramSettings;
using LocalizationResourceManager.Maui;
using System.Collections.ObjectModel;
// TO-DO: Rethink some methods, combine some similar methods in one, move some methods in ObservableCanvas 
namespace DiagramApp.Client.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly IPopupService _popupService;
        private readonly IToolboxService _toolboxService;
        private readonly IFileService _fileService;
        private readonly IClipboardService _clipboardService;
        private readonly ILocalizationResourceManager _localizationResourceManager;
        private readonly ICanvasMapper _canvasMapper;
        private readonly IFigureMapper _figureMapper;

        public string? CurrentLanguage => _localizationResourceManager?.CurrentCulture.TwoLetterISOLanguageName;

        [ObservableProperty]
        private string _currentTheme = App.Current!.UserAppTheme.ToString();

        [ObservableProperty]
        private int _canvasCounter = 0;
        public ObservableCollection<ObservableCanvas> Canvases { get; set; } = [];

        public bool IsCanvasesEmpty => Canvases.Count > 0;

        [ObservableProperty]
        private ObservableCanvas? _currentCanvas;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsCanvasNotNull))]
        private bool _isCanvasNull = true;
        public bool IsCanvasNotNull => !IsCanvasNull;

        [ObservableProperty]
        private ToolboxViewModel _toolboxViewModel;

        public bool IsClipboardNotEmpty => Clipboard.Default.HasText;

        public MainViewModel(
            IPopupService popupService,
            IToolboxService toolboxService,
            IFileService fileService,
            ILocalizationResourceManager localizationResourceManager,
            ICanvasMapper canvasMapper,
            IClipboardService clipboardService,
            IFigureMapper figureMapper)
        {
            _popupService = popupService;
            _toolboxService = toolboxService;
            _fileService = fileService;
            _localizationResourceManager = localizationResourceManager;
            _canvasMapper = canvasMapper;
            _clipboardService = clipboardService;
            _figureMapper = figureMapper;

            _toolboxViewModel = new(_toolboxService, localizationResourceManager);
        }

        [RelayCommand]
        private async Task SaveAsAsync()
        {
            if (CurrentCanvas is null)
                return;

            var data = _fileService.Save(_canvasMapper.ToDto(CurrentCanvas));
            var stream = new MemoryStream(data);

            var result = await FileSaver.Default.SaveAsync($"{CurrentCanvas.Settings.FileName}.dgmt", stream);

            if (result.IsSuccessful)
            {
                CurrentCanvas.FileLocation = result.FilePath;

                await SaveChangesAsync(result.FilePath);
            }
        }

        [RelayCommand]
        private async Task SaveAsync()
        {
            if (CurrentCanvas is null)
                return;

            if (string.IsNullOrEmpty(CurrentCanvas.FileLocation))
            {
                await SaveAsAsync();
                return;
            }

            await SaveChangesAsync(CurrentCanvas.FileLocation);
        }

        private async Task SaveChangesAsync(string path)
        {
            var data = _fileService.Save(_canvasMapper.ToDto(CurrentCanvas!));
            await File.WriteAllBytesAsync(path, data);

            CurrentCanvas!.Save();
        }

        [RelayCommand]
        private async Task LoadAsync()
        {
            var file = await FilePicker.Default.PickAsync(new PickOptions
            {
                FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.WinUI, new[] { ".dgmt" } },
                })
            });

            if (file is null)
                return;

            var loadedFile = _fileService.Load(file!.FullPath);
            var canvas = _canvasMapper.FromDto(loadedFile!);

            AddToCanvases(canvas);
            CanvasCounter++;

            await SelectCanvasAsync(canvas);
        }

        [RelayCommand]
        private async Task ExitAsync()
        {
            if (CurrentCanvas is { CanSave: true })
            {
                var result = await WindowClosingEventHandler.DisplayMessage(_localizationResourceManager);

                if (!result) return;
            }

            App.Current!.Quit();
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
                    settings.FileName = $"{_localizationResourceManager["Unnamed"]}{++CanvasCounter}";
                Canvas canvas = new(settings);
                ObservableCanvas observableCanvas = new(canvas);

                AddToCanvases(observableCanvas);
                await SelectCanvasAsync(observableCanvas);
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
        private async Task CloseCanvasAsync(ObservableCanvas targetCanvas)
        {
            if (targetCanvas is { CanSave: true })
            {
                var result = await WindowClosingEventHandler.DisplayMessage(_localizationResourceManager);

                if (!result) return;
            }

            RemoveFromCanvases(targetCanvas);
            CurrentCanvas = null;
            IsCanvasNull = true;
        }

        [RelayCommand]
        private async Task CloseAllCanvasesAsync()
        {
            foreach (var canvas in Canvases.ToList())
            {
                await CloseCanvasAsync(canvas);
            }
        }

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

        [RelayCommand]
        private void BringItemForward(ObservableFigure figure)
        {
            figure.ZIndex++;
        }

        [RelayCommand]
        private void SendItemBackward(ObservableFigure figure)
        {
            if (figure.ZIndex != 1)
                figure.ZIndex--;
        }
        // TO-DO: Consider moving this methods (Copy, Cut, Paste and Duplicate) to ObservableCanvas
        [RelayCommand]
        private async Task CopyItemAsync(ObservableFigure? targetFigure = null)
        {
            if (CurrentCanvas is not null)
            {
                var figure = targetFigure ?? CurrentCanvas.SelectedFigure;
                if (figure is null)
                    return;

                var dto = _figureMapper.ToDto(figure);
                var clipboardString = _clipboardService.ToClipboardString(dto);

                await Clipboard.Default.SetTextAsync(clipboardString);
                OnPropertyChanged(nameof(IsClipboardNotEmpty));
            }
        }

        [RelayCommand]
        private async Task CutItemAsync(ObservableFigure? targetFigure = null)
        {
            if (CurrentCanvas is not null)
            {
                var figure = targetFigure ?? CurrentCanvas.SelectedFigure;
                if (figure is null)
                    return;

                await CopyItemAsync(figure);
                DeleteItemFromCanvas(figure);
            }
        }

        [RelayCommand]
        private async Task PasteItemAsync()
        {
            if (CurrentCanvas is null)
                return;

            var clipboardString = await Clipboard.Default.GetTextAsync() ?? string.Empty;
            var dto = _clipboardService.ToObjectFromClipboard(clipboardString);

            if (dto is null)
                return;

            var model = _figureMapper.FromDto(dto);

            var action = new Action(() => CurrentCanvas.Figures.Add(model));
            var undoAction = new Action(() => CurrentCanvas.Figures.Remove(model));
            UndoableCommandHelper.ExecuteAction(CurrentCanvas, action, undoAction);
        }

        [RelayCommand]
        private void DuplicateItem(ObservableFigure? targetFigure = null)
        {
            if (CurrentCanvas is not null)
            {
                var figure = targetFigure ?? CurrentCanvas.SelectedFigure;
                if (figure is null)
                    return;

                var newFigure = (ObservableFigure)figure.Clone();
                newFigure.IsSelected = false;

                var action = new Action(() => CurrentCanvas.Figures.Add(newFigure));
                var undoAction = new Action(() => CurrentCanvas.Figures.Remove(newFigure));
                UndoableCommandHelper.ExecuteAction(CurrentCanvas, action, undoAction);

                CurrentCanvas.SelectFigure(newFigure);
            }
        }

        [RelayCommand]
        private async Task ChangeLanguageAsync(string parameter)
        {
            _localizationResourceManager.CurrentCulture = new System.Globalization.CultureInfo(parameter);
            await ToolboxViewModel.LoadToolboxCommand.ExecuteAsync(null);
            OnPropertyChanged(nameof(CurrentLanguage));
        }

        [RelayCommand]
        private void ChangeTheme(string parameter)
        {
            var requestedTheme = (AppTheme)Enum.Parse(typeof(AppTheme), parameter);
            App.Current!.UserAppTheme = requestedTheme;
            CurrentTheme = parameter;

            Preferences.Set("AppTheme", parameter);
        }

        [RelayCommand]
        private void ChangeGridVisibility()
        {
            if (CurrentCanvas is null)
                return;

            CurrentCanvas.ChangeGridVisibility();
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

        private void AddToCanvases(ObservableCanvas canvas)
        {
            Canvases.Add(canvas);
            OnPropertyChanged(nameof(IsCanvasesEmpty));
        }

        private void RemoveFromCanvases(ObservableCanvas canvas)
        {
            Canvases.Remove(canvas);
            OnPropertyChanged(nameof(IsCanvasesEmpty));
        }
    }
}
