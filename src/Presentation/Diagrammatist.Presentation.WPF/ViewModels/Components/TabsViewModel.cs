using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Diagrammatist.Application.AppServices.Canvas.Services;
using Diagrammatist.Presentation.WPF.Core.Commands.Managers;
using Diagrammatist.Presentation.WPF.Core.Commands.Helpers.Undoable;
using Diagrammatist.Presentation.WPF.Core.Mappers.Canvas;
using Diagrammatist.Presentation.WPF.Core.Models.Canvas;
using Diagrammatist.Presentation.WPF.ViewModels.Components.Constants.Flags;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Threading;
using Size = System.Drawing.Size;
using Diagrammatist.Presentation.WPF.Core.Messaging.Messages;
using Diagrammatist.Presentation.WPF.Core.Messaging.RequestMessages;

namespace Diagrammatist.Presentation.WPF.ViewModels.Components
{
    /// <summary>
    /// A view model class for canvas component.
    /// </summary>
    public sealed partial class TabsViewModel : ObservableRecipient
    {
        private readonly ICanvasManipulationService _canvasManipulationService;
        private readonly ICanvasSerializationService _canvasSerializationService;
        private readonly ITrackableCommandManager _trackableCommandManager;

        /// <summary>
        /// Occurs when a request is made to open existing file.
        /// </summary>
        /// <remarks>
        /// This event is triggered when user initiates a open action from menu button and returns file path.
        /// </remarks>
        public event Func<string>? RequestOpen;
        /// <summary>
        /// Occurs when canvas can't be open.
        /// </summary>
        /// <remarks>
        /// This event is triggered when app can't open canvas (e.g. when canvas already open).
        /// </remarks>
        public event Action? OpenFailed;
        /// <summary>
        /// Occurs when canvas can't be closed yet.
        /// </summary>
        /// <remarks>
        /// This event is triggered when app can't close canvas (e.g. when changes haven't been saved).
        /// </remarks>
        public event Func<MessageBoxResult>? CloseFailed;

        /// <summary>
        /// Gets or sets collection of <see cref="CanvasModel"/>.
        /// </summary>
        /// <remarks>
        /// This property used to store canvases in tabs UI.
        /// </remarks>
        public ObservableCollection<CanvasModel> Canvases { get; } = [];

        private readonly Dictionary<CanvasModel, string> _canvasFilePathes = [];

        /// <summary>
        /// Gets or sets <see cref="CanvasModel"/>.
        /// </summary>
        /// <remarks>
        /// This property used to store and distribute selected <see cref="CanvasModel"/>.
        /// </remarks>
        [ObservableProperty]
        [NotifyPropertyChangedRecipients]
        private CanvasModel? _selectedCanvas;

        public TabsViewModel(ICanvasManipulationService canvasManipulationService,
                             ITrackableCommandManager trackableCommandManager,
                             ICanvasSerializationService canvasSerializationService)
        {
            _canvasManipulationService = canvasManipulationService;
            _trackableCommandManager = trackableCommandManager;
            _canvasSerializationService = canvasSerializationService;

            IsActive = true;
        }

        /// <summary>
        /// Adds and selects new canvas.
        /// </summary>
        /// <param name="canvas"></param>
        private void AddCanvas(CanvasModel canvas)
        {
            Canvases.Add(canvas);
            _canvasFilePathes.Add(canvas, string.Empty);

            SelectedCanvas = canvas;
        }

        /// <summary>
        /// Adds and selects new canvas with file path saved.
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="filePath"></param>
        private void AddCanvas(CanvasModel canvas, string filePath)
        {
            Canvases.Add(canvas);
            _canvasFilePathes.Add(canvas, filePath);

            SelectedCanvas = canvas;
        }

        /// <summary>
        /// Removes canvas from all buffers.
        /// </summary>
        /// <param name="canvas"></param>
        private void RemoveCanvas(CanvasModel canvas)
        {
            _canvasFilePathes.Remove(canvas);
            Canvases.Remove(canvas);
            _trackableCommandManager.DeleteContent(canvas);
        }

        /// <summary>
        /// Creates new <see cref="CanvasModel"/> and adds it to <see cref="Canvases"/>
        /// </summary>
        /// <param name="settings">Diagram settings.</param>
        private async Task CreateCanvas(SettingsModel settings)
        {
            var canvasDomain = await _canvasManipulationService.CreateCanvasAsync(settings.ToDomain());

            var canvas = canvasDomain.ToModel();

            AddCanvas(canvas);

            Messenger.Send(CommandFlags.ZoomReset);
        }

        /// <summary>
        /// Opens an existing canvas.
        /// </summary>
        private void OpenCanvas()
        {
            if (RequestOpen is null)
            {
                return;
            }

            var filePath = RequestOpen();

            if (string.IsNullOrEmpty(filePath))
            {
                return;
            }

            if (_canvasFilePathes.ContainsValue(filePath) && OpenFailed is not null)
            {
                OpenFailed();
                return;
            }

            if (_canvasSerializationService.LoadCanvas(filePath)?.ToModel() is { } loadedCanvas)
            {
                AddCanvas(loadedCanvas, filePath);
            }
        }

        /// <summary>
        /// Updates current canvas size.
        /// </summary>
        /// <param name="newSize">New size.</param>
        private void UpdateCanvasSize(Size newSize)
        {
            if (SelectedCanvas is not null)
            {
                var oldSize = new Size(SelectedCanvas.Settings.Width, SelectedCanvas.Settings.Height);
                var currentCanvas = SelectedCanvas.ToDomain();

                var command = CommonUndoableHelper.CreateUndoableCommand(
                    () =>
                    {
                        _canvasManipulationService.UpdateCanvas(currentCanvas, newSize);
                        SelectedCanvas.Settings = currentCanvas.Settings.ToModel();
                    },
                    () =>
                    {
                        _canvasManipulationService.UpdateCanvas(currentCanvas, oldSize);
                        SelectedCanvas.Settings = currentCanvas.Settings.ToModel();
                    }
                );

                _trackableCommandManager.Execute(command);
            }
        }

        /// <summary>
        /// Deletes existing <see cref="CanvasModel"/> from <see cref="Canvases"/>.
        /// </summary>
        /// <param name="target">Target canvas.</param>
        [RelayCommand]
        private void CloseCanvas(CanvasModel target)
        {
            if (CloseFailed is null)
            {
                return;
            }

            if (!target.HasChanges)
            {
                RemoveCanvas(target);
                return;
            }

            var closeResult = CloseFailed();

            if (closeResult == MessageBoxResult.Cancel)
            {
                return;
            }
            else if (closeResult == MessageBoxResult.Yes)
            {
                SelectedCanvas = target;

                if (!Messenger.Send(new SaveRequestMessage()))
                {
                    return;
                }
            }

            RemoveCanvas(target);
        }

        private void CloseCanvases()
        {
            if (Canvases.Count > 0)
            {
                foreach (var canvas in Canvases.ToList())
                {
                    CloseCanvas(canvas);
                }
            }
        }

        private void UpdateCanvasFilePath(string filePath)
        {
            if (SelectedCanvas is not null && _canvasFilePathes.ContainsKey(SelectedCanvas))
            {
                _canvasFilePathes[SelectedCanvas] = filePath;
            }
        }

        private bool SaveAllCanvases()
        {
            if (Canvases.Count == 0)
            {
                return false;
            }

            foreach (var canvas in Canvases.ToList())
            {
                SelectedCanvas = canvas;
                if (!Messenger.Send(new SaveRequestMessage()))
                    return false;
            }

            return true;
        }

        partial void OnSelectedCanvasChanged(CanvasModel? value)
        {
            if (value is not null && _canvasFilePathes.TryGetValue(value, out var canvasFilePath))
            {
                Messenger.Send(new CanvasFilePathMessage(canvasFilePath));
            }

            Messenger.Send(new Tuple<string, bool>(MenuFlags.HasCanvas, value is not null));

            _trackableCommandManager.UpdateContent(value);
        }

        protected override void OnActivated()
        {
            base.OnActivated();

            Messenger.Register<TabsViewModel, NewCanvasSettingsMessage>(this, (r, m) =>
            {
                Dispatcher.CurrentDispatcher.Invoke(async () =>
                {
                    await CreateCanvas(m.Value);
                });
            });

            Messenger.Register<TabsViewModel, UpdatedSizeMessage>(this, (r, m) =>
            {
                UpdateCanvasSize(m.Value);
            });

            Messenger.Register<TabsViewModel, UpdatedCanvasFilePathMessage>(this, (r, m) =>
            {
                UpdateCanvasFilePath(m.Value);
            });

            Messenger.Register<TabsViewModel, SaveAllRequestMessage>(this, (r, m) =>
            {
                m.Reply(SaveAllCanvases());
            });

            Messenger.Register<TabsViewModel, string>(this, (r, m) =>
            {
                switch (m)
                {
                    case CommandFlags.CloseCanvas when SelectedCanvas is not null:
                        CloseCanvas(SelectedCanvas);
                        break;
                    case CommandFlags.CloseCanvases:
                        CloseCanvases();
                        break;
                    case CommandFlags.Open:
                        OpenCanvas();
                        break;
                }
            });
        }
    }
}
