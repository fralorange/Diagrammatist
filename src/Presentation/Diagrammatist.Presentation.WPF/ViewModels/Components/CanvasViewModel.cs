using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Diagrammatist.Application.AppServices.Document.Services;
using Diagrammatist.Presentation.WPF.Core.Controls.Args;
using Diagrammatist.Presentation.WPF.Core.Facades.Canvas;
using Diagrammatist.Presentation.WPF.Core.Helpers;
using Diagrammatist.Presentation.WPF.Core.Managers.Command;
using Diagrammatist.Presentation.WPF.Core.Messaging.Messages;
using Diagrammatist.Presentation.WPF.Core.Messaging.RequestMessages;
using Diagrammatist.Presentation.WPF.Core.Models.Canvas;
using Diagrammatist.Presentation.WPF.Core.Models.Connection;
using Diagrammatist.Presentation.WPF.Core.Models.Figures;
using Diagrammatist.Presentation.WPF.Core.Services.Settings;
using Diagrammatist.Presentation.WPF.Core.Shared.Enums;
using Diagrammatist.Presentation.WPF.Core.Shared.Records;
using Diagrammatist.Presentation.WPF.ViewModels.Components.Constants.Flags;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Media;

namespace Diagrammatist.Presentation.WPF.ViewModels.Components
{
    /// <summary>
    /// A view model class for canvas component.
    /// </summary>
    public sealed partial class CanvasViewModel : ObservableRecipient
    {
        private readonly ITrackableCommandManager _trackableCommandManager;
        private readonly ICanvasServiceFacade _canvasServiceFacade;
        private readonly IDocumentSerializationService _documentSerializationService;
        private readonly IUserSettingsService _userSettingsService;

        /// <summary>
        /// Occurs when a request is made to zoom current window in.
        /// </summary>
        /// <remarks>
        /// This event is triggered when user initiates a zoom in action from menu button.
        /// </remarks>
        public event Action? RequestZoomIn;
        /// <summary>
        /// Occurs when a request is made to zoom current window out.
        /// </summary>
        /// <remarks>
        /// This event is triggered when user initiates a zoom out action from menu button.
        /// </remarks>
        public event Action? RequestZoomOut;
        /// <summary>
        /// Occurs when a request is made to reset current window zoom.
        /// </summary>
        /// <remarks>
        /// This event is triggered when user initiates a zoom reset action from menu button.
        /// </remarks>
        public event Action? RequestZoomReset;
        /// <summary>
        /// Occurs when a requiest is made to export current canvas as bitmap.
        /// </summary>
        /// <remarks>
        /// This event is triggered when user initiates a export action from menu button.
        /// </remarks>
        public event Action<ExportSettings>? RequestExport;
        /// <summary>
        /// Occurs when a request is made to get visible area of the canvas.
        /// </summary>
        /// <remarks> 
        /// This event is triggered when user initiates a visible area request by adding new figure.
        /// </remarks>
        public event Func<Rect>? RequestVisibleArea;
        /// <summary>
        /// Occurs when a request is made to scroll to the specified figure on the canvas.
        /// </summary>
        /// <remarks> 
        /// This event is triggered when user adds a figure from canvas non-visible area.
        /// </remarks>
        public event Action<FigureModel>? RequestScrollToFigure;
        /// <summary>
        /// Occurs when a request is made to restore canvas state (zoom and offsets).
        /// </summary>
        /// <remarks> 
        /// This event is triggered when user initiates a restore action from menu button.
        /// </remarks>
        public event Action<(float zoom, double hOffset, double vOffset)> RequestRestoreState;

        private CanvasModel? _currentCanvas;

        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Behaviors/Member[@name="CurrentCanvas"]/*'/>
        public CanvasModel? CurrentCanvas
        {
            get => _currentCanvas;
            private set => SetProperty(ref _currentCanvas, value, true);
        }

        private MouseMode _currentMouseMode;

        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Behaviors/Member[@name="CurrentMouseMode"]/*'/>
        public MouseMode CurrentMouseMode
        {
            get => _currentMouseMode;
            private set => SetProperty(ref _currentMouseMode, value);
        }

        private ObservableCollection<FigureModel>? _figures;

        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Behaviors/Member[@name="ViewModelFigures"]/*'/>
        /// <remarks>
        /// This property used to send figures as message to other components that require it.
        /// </remarks>
        public ObservableCollection<FigureModel>? Figures
        {
            get => _figures;
            private set
            {
                if (_figures is not null)
                {
                    _figures.CollectionChanged -= FiguresCollectionChanged;
                }

                SetProperty(ref _figures, value, broadcast: true);

                if (_figures is not null)
                {
                    _figures.CollectionChanged += FiguresCollectionChanged;
                }
            }
        }

        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Behaviors/Member[@name="ViewModelConnections"]/*'/>
        /// <remarks>
        /// This property used to send connections as message to other components that require it.
        /// </remarks>
        [ObservableProperty]
        [NotifyPropertyChangedRecipients]
        private ObservableCollection<ConnectionModel>? _connections;

        /// <summary>
        /// Gets or sets selected figure.
        /// </summary>
        /// <remarks>
        /// This property used to store selected figure that placed on the canvas.
        /// </remarks>
        [ObservableProperty]
        [NotifyPropertyChangedRecipients]
        private FigureModel? _selectedFigure;

        /// <summary>
        /// Gets or sets grid visible flag.
        /// </summary>
        /// <remarks>
        /// This property used to determine whether grid is visible for user or not.
        /// </remarks>
        [ObservableProperty]
        private bool _isGridVisible;

        /// <summary>
        /// Gets or sets snap to grid option.
        /// </summary>
        /// <remarks>
        /// This property used to determine whether snap to grid is enabled or not.
        /// </remarks>
        [ObservableProperty]
        private bool _isGridSnapEnabled;

        /// <summary>
        /// Gets or sets alt disable grid snap option.
        /// </summary>
        /// <remarks>
        /// This property used to determine whether alt disable grid snap is enabled or not.
        /// </remarks>
        [ObservableProperty]
        private bool _isAltGridSnapEnabled;

        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Behaviors/Member[@name="IsBlocked"]/*'/>
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotBlocked))]
        [NotifyCanExecuteChangedFor(
            nameof(CopyCommand),
            nameof(CutCommand),
            nameof(DuplicateCommand),
            nameof(BringForwardItemCommand),
            nameof(SendBackwardItemCommand),
            nameof(DeleteItemCommand),
            nameof(PasteCommand))]
        private bool _isBlocked;

        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Behaviors/Member[@name="IsNotBlocked"]/*'/>
        public bool IsNotBlocked => !IsBlocked;

        public CanvasViewModel(ITrackableCommandManager trackableCommandManager,
                               ICanvasServiceFacade canvasServiceFacade,
                               IDocumentSerializationService documentSerializationService,
                               IUserSettingsService userSettingsService)
        {
            _trackableCommandManager = trackableCommandManager;
            _canvasServiceFacade = canvasServiceFacade;
            _documentSerializationService = documentSerializationService;
            _userSettingsService = userSettingsService;

            _trackableCommandManager.StateChanged += (_, _) =>
            {
                if (CurrentCanvas != null)
                    CurrentCanvas.HasChanges = _trackableCommandManager.HasChanges;
            };

            IsGridVisible = _userSettingsService.Get<bool>("GridVisible");
            IsGridSnapEnabled = _userSettingsService.Get<bool>("SnapToGrid");
            IsAltGridSnapEnabled = _userSettingsService.Get<bool>("AltGridSnap");

            IsActive = true;
        }

        #region Commands Can Execute
        private bool MenuIsNotBlocked()
        {
            return IsNotBlocked;
        }
        #endregion

        #region Commands

        /// <summary>
        /// Reverts changes that was made by user.
        /// </summary>
        private void Undo()
        {
            if (CurrentCanvas is null)
                return;

            _trackableCommandManager.Undo();
        }

        /// <summary>
        /// Repeats changes that was reverted by user.
        /// </summary>
        private void Redo()
        {
            if (CurrentCanvas is null)
                return;

            _trackableCommandManager.Redo();
        }

        /// <summary>
        /// Zooms canvas in.
        /// </summary>
        private void ZoomIn()
        {
            if (RequestZoomIn is not null && CurrentCanvas is not null)
            {
                RequestZoomIn();
            }
        }

        /// <summary>
        /// Zooms canvas out.
        /// </summary>
        private void ZoomOut()
        {
            if (RequestZoomOut is not null && CurrentCanvas is not null)
            {
                RequestZoomOut();
            }
        }

        /// <summary>
        /// Resets current zoom.
        /// </summary>
        private void ZoomReset()
        {
            if (RequestZoomReset is not null)
            {
                RequestZoomReset();
            }
        }

        /// <summary>
        /// Enables or disables grid visual.
        /// </summary>
        private void EnableGrid()
        {
            if (CurrentCanvas is null)
                return;

            IsGridVisible = !IsGridVisible;
            // Save client-prefs.
            _userSettingsService.Set("GridVisible", IsGridVisible);
            _userSettingsService.Save();
        }

        /// <summary>
        /// Updates current canvas size.
        /// </summary>
        /// <param name="newSize">New size.</param>
        private void UpdateCanvasSize(Size newSize)
        {
            if (CurrentCanvas is not null)
            {
                var oldSize = new Size(CurrentCanvas.Settings.Width, CurrentCanvas.Settings.Height);

                var command = CommonUndoableHelper.CreateUndoableCommand(
                    () =>
                    {
                        _canvasServiceFacade.Manipulation.UpdateCanvas(CurrentCanvas, newSize);
                        ZoomReset();
                    },
                    () =>
                    {
                        _canvasServiceFacade.Manipulation.UpdateCanvas(CurrentCanvas, oldSize);
                        ZoomReset();
                    }
                );

                _trackableCommandManager.Execute(command);
            }
        }

        /// <summary>
        /// Updates current canvas background.
        /// </summary>
        /// <param name="newBG"></param>
        private void UpdateCanvasBackground(Color newBG)
        {
            if (CurrentCanvas is not null)
            {
                var oldBG = CurrentCanvas.Settings.Background;

                var command = CommonUndoableHelper.CreateUndoableCommand(
                    () => _canvasServiceFacade.Manipulation.UpdateCanvas(CurrentCanvas, newBG),
                    () => _canvasServiceFacade.Manipulation.UpdateCanvas(CurrentCanvas, oldBG));

                _trackableCommandManager.Execute(command);
            }
        }

        /// <summary>
        /// Updates current diagram (canvas) type.
        /// </summary>
        /// <param name="newDiagramType"></param>
        private void UpdateCanvasDiagramType(DiagramsModel newDiagramType)
        {
            if (CurrentCanvas is not null)
            {
                var oldDiagramType = CurrentCanvas.Settings.Type;
                var currentDoc = Messenger.Send<CurrentDocumentRequestMessage>().Response;
                var payloads = currentDoc?.Payloads.ToDictionary();

                var command = CommonUndoableHelper.CreateUndoableCommand(
                    () =>
                    {
                        _canvasServiceFacade.Manipulation.UpdateCanvas(CurrentCanvas, newDiagramType);
                        if (payloads is not null)
                        {
                            currentDoc!.SetPayloads([]);
                        }
                        Messenger.Send<Tuple<string, bool>>(new(ActionFlags.HasCustomCanvas, newDiagramType is DiagramsModel.Custom));
                    },
                    () =>
                    {
                        _canvasServiceFacade.Manipulation.UpdateCanvas(CurrentCanvas, oldDiagramType);
                        if (payloads is not null)
                        {
                            currentDoc!.SetPayloads(payloads);
                        }
                        Messenger.Send<Tuple<string, bool>>(new(ActionFlags.HasCustomCanvas, oldDiagramType is DiagramsModel.Custom));
                    });

                _trackableCommandManager.Execute(command);
            }
        }

        /// <summary>
        /// Exports current canvas. 
        /// </summary>
        private void Export(ExportSettings settings)
        {
            if (CurrentCanvas is not null && RequestExport is not null)
            {
                RequestExport(settings);
            }
        }

        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Behaviors/Member[@name="DeleteItem"]/*'/>
        [RelayCommand(CanExecute = nameof(MenuIsNotBlocked))]
        private void DeleteItem(FigureModel figure)
        {
            if (Figures is null || Connections is null)
                return;

            _canvasServiceFacade.FigureManipulation.Delete(figure, Figures, Connections);
        }

        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Behaviors/Member[@name="Paste"]/*'/>
        [RelayCommand(CanExecute = nameof(MenuIsNotBlocked))]
        private void Paste(object position)
        {
            if (Figures is not null && position is Point destination)
            {
                _canvasServiceFacade.FigureManipulation.Paste(Figures, destination);
            }
        }

        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Behaviors/Member[@name="Copy"]/*'/>
        [RelayCommand(CanExecute = nameof(MenuIsNotBlocked))]
        private void Copy()
        {
            if (SelectedFigure is not null)
            {
                _canvasServiceFacade.FigureManipulation.Copy(SelectedFigure);
            }
        }

        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Behaviors/Member[@name="Cut"]/*'/>
        [RelayCommand(CanExecute = nameof(MenuIsNotBlocked))]
        private void Cut()
        {
            if (SelectedFigure is not null && Figures is not null)
            {
                _canvasServiceFacade.FigureManipulation.Cut(SelectedFigure, Figures);
            }
        }

        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Behaviors/Member[@name="Duplicate"]/*'/>
        [RelayCommand(CanExecute = nameof(MenuIsNotBlocked))]
        private void Duplicate()
        {
            if (SelectedFigure is not null && Figures is not null)
            {
                _canvasServiceFacade.FigureManipulation.Duplicate(SelectedFigure, Figures);
            }
        }

        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Behaviors/Member[@name="BringForwardItem"]/*'/>
        [RelayCommand(CanExecute = nameof(MenuIsNotBlocked))]
        private void BringForwardItem(FigureModel figure)
        {
            _canvasServiceFacade.FigureManipulation.BringForward(figure);
        }

        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Behaviors/Member[@name="SendBackwardItem"]/*'/>
        [RelayCommand(CanExecute = nameof(MenuIsNotBlocked))]
        private void SendBackwardItem(FigureModel figure)
        {
            _canvasServiceFacade.FigureManipulation.SendBackward(figure);
        }

        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Behaviors/Member[@name="CopyStyle"]/*'/>
        [RelayCommand(CanExecute = nameof(MenuIsNotBlocked))]
        private void CopyStyle()
        {
            if (SelectedFigure is not null)
            {
                _canvasServiceFacade.FigureManipulation.CopyStyle(SelectedFigure);
            }
        }

        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Behaviors/Member[@name="PasteStyle"]/*'/>
        [RelayCommand(CanExecute = nameof(MenuIsNotBlocked))]
        private void PasteStyle()
        {
            if (SelectedFigure is not null)
            {
                _canvasServiceFacade.FigureManipulation.PasteStyle(SelectedFigure);
            }
        }

        /// <summary>
        /// Processes item position changing event.
        /// </summary>
        /// <param name="e">Position changed event arguments.</param>
        [RelayCommand]
        private void ItemPositionChanging(PositionChangingEventArgs e)
        {
            if (e.DataContext is FigureModel figure && Connections is not null)
            {
                _canvasServiceFacade.Interaction.MoveFigureVisuals(figure, e.OldPos, e.NewPos, Connections);
            }
        }

        /// <summary>
        /// Archives different positions of an figure object.
        /// </summary>
        /// <remarks>
        /// This commands occurs when item position change and used to implement undo/redo event command.
        /// </remarks>
        /// <param name="e">Position changed event arguments.</param>
        [RelayCommand]
        private void ItemPositionChanged(PositionChangedEventArgs e)
        {
            if (e.DataContext is FigureModel figure && Connections is not null)
            {
                _canvasServiceFacade.Interaction.MoveFigure(figure, e.InitialPos, e.OldPos, e.NewPos, Connections);
            }
        }

        #endregion

        // Always select new figures.
        private void FiguresCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action is NotifyCollectionChangedAction.Add && e.NewItems is { Count: > 0 })
            {
                SelectedFigure = e.NewItems[^1] as FigureModel;
            }
        }

        /// <inheritdoc/>
        protected override void OnActivated()
        {
            base.OnActivated();
            // Initialize new canvas.
            Messenger.Register<CanvasViewModel, PropertyChangedMessage<CanvasModel?>>(this, (r, m) =>
            {
                CurrentCanvas = m.NewValue;

                Figures = CurrentCanvas?.Figures;

                Connections = CurrentCanvas?.Connections;
            });
            // Change canvas size.
            Messenger.Register<CanvasViewModel, UpdatedSizeMessage>(this, (r, m) =>
            {
                UpdateCanvasSize(m.Value);
            });
            // Change canvas background.
            Messenger.Register<CanvasViewModel, UpdatedBackgroundMessage>(this, (r, m) =>
            {
                UpdateCanvasBackground(m.Value);
            });
            // Change canvas diagram type.
            Messenger.Register<CanvasViewModel, UpdatedTypeMessage>(this, (r, m) =>
            {
                UpdateCanvasDiagramType(m.Value);
            });
            // Change mouse mode.
            Messenger.Register<CanvasViewModel, PropertyChangedMessage<MouseMode>>(this, (r, m) =>
            {
                CurrentMouseMode = m.NewValue;
            });
            // Change selected figure from object tree.
            Messenger.Register<CanvasViewModel, PropertyChangedMessage<FigureModel?>>(this, (r, m) =>
            {
                SelectedFigure = m.NewValue;
            });
            // Scroll to figure.
            Messenger.Register<CanvasViewModel, ScrollToFigureMessage>(this, (r, m) =>
            {
                RequestScrollToFigure?.Invoke(m.Value);
            });
            // Export current canvas.
            Messenger.Register<CanvasViewModel, ExportSettingsMessage>(this, (r, m) =>
            {
                Export(m.Value);
            });
            // Restore canvas state.
            Messenger.Register<CanvasViewModel, RestoreCanvasStateMessage>(this, (r, m) =>
            {
                if (m.Value is { } value)
                {
                    RequestRestoreState?.Invoke(value);
                }
            });
            // Answer to current canvas request.
            Messenger.Register<CanvasViewModel, CurrentCanvasRequestMessage>(this, (r, m) =>
            {
                m.Reply(r.CurrentCanvas);
            });
            // Answer to visible area request.
            Messenger.Register<CanvasViewModel, VisibleAreaRequestMessage>(this, (r, m) =>
            {
                var visibleRect = RequestVisibleArea?.Invoke() ?? Rect.Empty;
                m.Reply(visibleRect);
            });
            // Register action flags.
            Messenger.Register<CanvasViewModel, Tuple<string, bool>>(this, (r, m) =>
            {
                switch (m.Item1)
                {
                    case ActionFlags.IsBlocked: IsBlocked = m.Item2; break;
                    case ActionFlags.IsGridSnapEnabled: IsGridSnapEnabled = m.Item2; break;
                    case ActionFlags.IsAltGridSnapEnabled: IsAltGridSnapEnabled = m.Item2; break;
                }
            });
            // Register menu commands.
            Messenger.Register<CanvasViewModel, string>(this, (r, m) =>
            {
                switch (m)
                {
                    case CommandFlags.Undo: Undo(); break;
                    case CommandFlags.Redo: Redo(); break;
                    case CommandFlags.ZoomIn: ZoomIn(); break;
                    case CommandFlags.ZoomOut: ZoomOut(); break;
                    case CommandFlags.ZoomReset: ZoomReset(); break;
                    case CommandFlags.EnableGrid: EnableGrid(); break;
                }
            });
        }
    }
}
