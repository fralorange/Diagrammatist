using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Diagrammatist.Application.AppServices.Canvas.Services;
using Diagrammatist.Presentation.WPF.Core.Helpers;
using Diagrammatist.Presentation.WPF.Core.Managers.Command;
using Diagrammatist.Presentation.WPF.Core.Managers.Tabs;
using Diagrammatist.Presentation.WPF.Core.Mappers.Canvas;
using Diagrammatist.Presentation.WPF.Core.Messaging.Messages;
using Diagrammatist.Presentation.WPF.Core.Messaging.RequestMessages;
using Diagrammatist.Presentation.WPF.Core.Models.Canvas;
using Diagrammatist.Presentation.WPF.ViewModels.Components.Constants.Flags;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Threading;
using Size = System.Drawing.Size;

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
        private readonly ICanvasTabsManager _tabsManager;

        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Behaviors/Member[@name="RequestOpen"]/*'/>
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
        public ObservableCollection<CanvasModel> Canvases => _tabsManager.Canvases;

        /// <summary>
        /// Gets or sets 'has canvases' flag.
        /// </summary>
        /// <remarks>
        /// This property used to determine whether content has any canvases or not.
        /// </remarks>
        [ObservableProperty]
        private bool _hasCanvases;

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
                             ICanvasSerializationService canvasSerializationService,
                             ICanvasTabsManager tabsManager)
        {
            _canvasManipulationService = canvasManipulationService;
            _trackableCommandManager = trackableCommandManager;
            _canvasSerializationService = canvasSerializationService;
            _tabsManager = tabsManager;

            IsActive = true;
        }

        /// <summary>
        /// Adds and selects new canvas with file path (optional) saved.
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="filePath"></param>
        private void AddCanvas(CanvasModel canvas, string filePath = "")
        {
            _tabsManager.Add(canvas, filePath);

            SelectedCanvas = canvas;

            UpdateFlags();
        }

        /// <summary>
        /// Removes canvas from all buffers.
        /// </summary>
        /// <param name="canvas"></param>
        private void RemoveCanvas(CanvasModel canvas)
        {
            _tabsManager.Remove(canvas);

            _trackableCommandManager.DeleteContent(canvas);

            UpdateFlags();
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
            if (RequestOpen is null) return;

            var filePath = RequestOpen();

            if (string.IsNullOrEmpty(filePath)) return;

            if (_tabsManager.ContainsFilePath(filePath) && OpenFailed is not null)
            {
                OpenFailed();
                return;
            }

            if (_canvasSerializationService.LoadCanvas(filePath)?.ToModel() is { } loadedCanvas)
            {
                AddCanvas(loadedCanvas, filePath);

                Messenger.Send(CommandFlags.ZoomReset);
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
        /// Updates current canvas file path.
        /// </summary>
        /// <param name="filePath">New file path.</param>
        private void UpdateCanvasFilePath(string filePath)
        {
            if (SelectedCanvas is not null)
            {
                _tabsManager.UpdateFilePath(SelectedCanvas, filePath);
            }
        }

        /// <summary>
        /// Deletes existing <see cref="CanvasModel"/> from <see cref="Canvases"/>.
        /// </summary>
        /// <param name="target">Target canvas.</param>
        [RelayCommand]
        private void CloseCanvas(CanvasModel target)
        {
            if (CloseFailed is null) return;

            if (!target.HasChanges)
            {
                RemoveCanvas(target);
                return;
            }

            var closeResult = CloseFailed();

            if (closeResult == MessageBoxResult.Cancel) return;

            SelectedCanvas = target;

            if (closeResult == MessageBoxResult.Yes && !Messenger.Send(new SaveRequestMessage()))
            {
                return;
            }

            RemoveCanvas(target);
        }

        private void CloseCanvases()
        {
            foreach (var canvas in Canvases.ToList())
            {
                CloseCanvas(canvas);
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

        private void UpdateFlags()
        {
            HasCanvases = Canvases.Count > 0;
        }

        partial void OnSelectedCanvasChanged(CanvasModel? value)
        {
            if (value is not null)
            {
                var path = _tabsManager.GetFilePath(value);
                if (path is not null)
                {
                    Messenger.Send(new CanvasFilePathMessage(path));
                }
            }

            Messenger.Send(new Tuple<string, bool>(MenuFlags.HasCanvas, value is not null));
            Messenger.Send(new Tuple<string, bool>(MenuFlags.HasCustomCanvas, value?.Settings.Type is DiagramsModel.Custom));

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
