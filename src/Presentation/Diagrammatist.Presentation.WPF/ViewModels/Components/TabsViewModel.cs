using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Diagrammatist.Application.AppServices.Document.Services;
using Diagrammatist.Presentation.WPF.Core.Managers.Command;
using Diagrammatist.Presentation.WPF.Core.Managers.Tabs;
using Diagrammatist.Presentation.WPF.Core.Mappers.Canvas;
using Diagrammatist.Presentation.WPF.Core.Mappers.Document;
using Diagrammatist.Presentation.WPF.Core.Messaging.Messages;
using Diagrammatist.Presentation.WPF.Core.Messaging.RequestMessages;
using Diagrammatist.Presentation.WPF.Core.Models.Canvas;
using Diagrammatist.Presentation.WPF.Core.Models.Document;
using Diagrammatist.Presentation.WPF.Core.Services.Canvas.Manipulation;
using Diagrammatist.Presentation.WPF.Core.Shared.Enums;
using Diagrammatist.Presentation.WPF.ViewModels.Components.Constants.Flags;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Threading;

namespace Diagrammatist.Presentation.WPF.ViewModels.Components
{
    /// <summary>
    /// A view model class for canvas component.
    /// </summary>
    public sealed partial class TabsViewModel : ObservableRecipient
    {
        private readonly ICanvasManipulationService _canvasManipulationService;
        private readonly IDocumentSerializationService _documentSerializationService;
        private readonly ITrackableCommandManager _trackableCommandManager;
        private readonly IDocumentTabsManager _tabsManager;

        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Behaviors/Member[@name="RequestOpen"]/*'/>
        public event Func<string>? RequestOpen;
        /// <summary>
        /// Occurs when a request is made to save current canvas as new file.
        /// </summary>
        /// <remarks>
        /// This event is triggered when user initiates a save as action from menu button and returns file path.
        /// </remarks>
        public event Func<string, string>? RequestSaveAs;
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
        public event Func<ConfirmationResult>? CloseFailed;

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
                             IDocumentTabsManager tabsManager,
                             IDocumentSerializationService documentSerializationService)
        {
            _canvasManipulationService = canvasManipulationService;
            _trackableCommandManager = trackableCommandManager;
            _tabsManager = tabsManager;
            _documentSerializationService = documentSerializationService;

            IsActive = true;
        }

        /// <summary>
        /// Adds and selects new document with file path (optional) saved.
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="filePath"></param>
        private void AddDocument(DocumentModel document, string filePath = "")
        {
            _tabsManager.Add(document, filePath);

            SelectedCanvas = document.Canvas;

            UpdateFlags();

            Messenger.Send(CommandFlags.ZoomReset);
        }

        /// <summary>
        /// Removes document from all buffers.
        /// </summary>
        /// <param name="canvas"></param>
        private void RemoveDocument(DocumentModel document)
        {
            _tabsManager.Remove(document);

            _trackableCommandManager.DeleteContent(document);

            UpdateFlags();
        }

        /// <summary>
        /// Creates new <see cref="CanvasModel"/> and adds it to <see cref="Canvases"/>
        /// </summary>
        /// <param name="settings">Diagram settings.</param>
        private async Task CreateCanvas(SettingsModel settings)
        {
            var canvas = await _canvasManipulationService.CreateCanvasAsync(settings);

            AddDocument(new DocumentModel { Canvas = canvas });
        }

        /// <summary>
        /// Opens existing document by file path and adds it to <see cref="Canvases"/> collection.
        /// </summary>
        /// <param name="filePath"></param>
        public void OpenDocument(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) return;

            if (_tabsManager.ContainsFilePath(filePath) && OpenFailed is not null)
            {
                OpenFailed();
                return;
            }

            if (_documentSerializationService.Load(filePath)?.ToModel() is { } loadedDocument)
            {
                AddDocument(loadedDocument, filePath);
            }
        }

        /// <summary>
        /// Opens an existing canvas.
        /// </summary>
        private void OpenDocument()
        {
            if (RequestOpen is null) return;

            var filePath = RequestOpen();

            OpenDocument(filePath);
        }

        /// <summary>
        /// Deletes existing <see cref="CanvasModel"/> from <see cref="Canvases"/>.
        /// </summary>
        /// <param name="target">Target canvas.</param>
        [RelayCommand]
        private void CloseDocument(CanvasModel? target)
        {
            if (target is null)
                return;

            if (CloseFailed is null) return;

            var doc = _tabsManager.Get(target)!;

            if (!target.HasChanges)
            {
                RemoveDocument(doc);
                return;
            }

            var closeResult = CloseFailed();

            if (closeResult == ConfirmationResult.Cancel) return;

            SelectedCanvas = target;

            if (closeResult == ConfirmationResult.Yes && !Messenger.Send(new SaveRequestMessage()))
            {
                return;
            }

            RemoveDocument(doc);
        }

        /// <summary>
        /// Occurs when tab is changed.
        /// </summary>
        [RelayCommand]
        private void TabChanged()
        {
            if (SelectedCanvas is null)
                return;

            Messenger.Send(new RestoreCanvasStateMessage((
                Convert.ToSingle(SelectedCanvas.Zoom),
                SelectedCanvas.Offset.X,
                SelectedCanvas.Offset.Y
            )));
        }

        private void CloseDocuments()
        {
            foreach (var canvas in Canvases.ToList())
            {
                CloseDocument(canvas);
            }
        }

        /// <summary>
        /// Saves current canvas as new file.
        /// summary>
        private bool SaveAs()
        {
            if (SelectedCanvas is null || RequestSaveAs == null)
                return false;

            string filePath = RequestSaveAs(SelectedCanvas.Settings.FileName);

            if (string.IsNullOrEmpty(filePath))
                return false;

            var doc = _tabsManager.Get(SelectedCanvas)!;

            string fileName = Path.GetFileNameWithoutExtension(filePath);
            if (fileName != SelectedCanvas.Settings.FileName)
                SelectedCanvas.Settings.FileName = fileName;

            _documentSerializationService.Save(doc.ToDomain(), filePath);
            _trackableCommandManager.MarkSaved();

            _tabsManager.UpdateFilePath(SelectedCanvas, filePath);

            return true;
        }

        /// <summary>
        /// Saves current canvas.
        /// </summary>
        private bool Save()
        {
            if (SelectedCanvas is null)
            {
                return false;
            }

            var filePath = _tabsManager.GetFilePath(SelectedCanvas);
            var doc = _tabsManager.Get(SelectedCanvas)!;

            if (!File.Exists(filePath))
            {
                return SaveAs();
            }
            else
            {
                _documentSerializationService.Save(doc.ToDomain(), filePath);
                _trackableCommandManager.MarkSaved();

                return true;
            }
        }

        private bool SaveAll()
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
            Messenger.Send(new Tuple<string, bool>(ActionFlags.HasCanvas, value is not null));
            Messenger.Send(new Tuple<string, bool>(ActionFlags.HasCustomCanvas, value?.Settings.Type is DiagramsModel.Custom));

            var doc = _tabsManager.Get(value);
            _trackableCommandManager.UpdateContent(doc);
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            // Create new canvas.
            Messenger.Register<TabsViewModel, NewCanvasSettingsMessage>(this, (r, m) =>
            {
                Dispatcher.CurrentDispatcher.Invoke(async () =>
                {
                    await CreateCanvas(m.Value);
                });
            });
            // Save all and return tesult.
            Messenger.Register<TabsViewModel, SaveAllRequestMessage>(this, (r, m) =>
            {
                m.Reply(SaveAll());
            });
            // Save and return result.
            Messenger.Register<TabsViewModel, SaveRequestMessage>(this, (r, m) =>
            {
                m.Reply(Save());
            });
            // Return current document.
            Messenger.Register<TabsViewModel, CurrentDocumentRequestMessage>(this, (r, m) =>
            {
                m.Reply(_tabsManager.Get(SelectedCanvas));
            });
            // Register commands.
            Messenger.Register<TabsViewModel, string>(this, (r, m) =>
            {
                switch (m)
                {
                    case CommandFlags.CloseCanvas: CloseDocument(SelectedCanvas); break;
                    case CommandFlags.CloseCanvases: CloseDocuments(); break;
                    case CommandFlags.Open: OpenDocument(); break;
                    case CommandFlags.SaveAs: SaveAs(); break;
                }
            });
        }
    }
}
