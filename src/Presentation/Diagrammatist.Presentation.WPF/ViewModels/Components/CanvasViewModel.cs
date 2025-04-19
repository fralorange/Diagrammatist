using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Diagrammatist.Presentation.WPF.Core.Controls.Args;
using Diagrammatist.Presentation.WPF.Core.Facades.Canvas;
using Diagrammatist.Presentation.WPF.Core.Managers.Command;
using Diagrammatist.Presentation.WPF.Core.Mappers.Canvas;
using Diagrammatist.Presentation.WPF.Core.Messaging.Messages;
using Diagrammatist.Presentation.WPF.Core.Messaging.RequestMessages;
using Diagrammatist.Presentation.WPF.Core.Models.Canvas;
using Diagrammatist.Presentation.WPF.Core.Models.Connection;
using Diagrammatist.Presentation.WPF.Core.Models.Figures;
using Diagrammatist.Presentation.WPF.ViewModels.Components.Constants.Flags;
using Diagrammatist.Presentation.WPF.ViewModels.Components.Enums.Modes;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Windows;

namespace Diagrammatist.Presentation.WPF.ViewModels.Components
{
    /// <summary>
    /// A view model class for canvas component.
    /// </summary>
    public sealed partial class CanvasViewModel : ObservableRecipient
    {
        private readonly ITrackableCommandManager _trackableCommandManager;
        private readonly ICanvasServiceFacade _canvasServiceFacade;

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
        /// Occurs when a request is made to save current canvas as new file.
        /// </summary>
        /// <remarks>
        /// This event is triggered when user initiates a save as action from menu button and returns file path.
        /// </remarks>
        public event Func<string, string>? RequestSaveAs;
        /// <summary>
        /// Occurs when a requiest is made to export current canvas as bitmap.
        /// </summary>
        /// <remarks>
        /// This event is triggered when user initiates a export action from menu button.
        /// </remarks>
        public event Action? RequestExport;

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

        /// <summary>
        /// Gets or sets current file path to the current canvas.
        /// </summary>
        /// <remarks>
        /// This property used to configure save options.
        /// </remarks>
        [ObservableProperty]
        private string _filePath = string.Empty;

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
        private bool _isGridVisible = Properties.Settings.Default.GridVisible;

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
                               ICanvasServiceFacade canvasServiceFacade)
        {
            _trackableCommandManager = trackableCommandManager;
            _canvasServiceFacade = canvasServiceFacade;

            _trackableCommandManager.StateChanged += (_, _) =>
            {
                if (CurrentCanvas != null)
                    CurrentCanvas.HasChanges = _trackableCommandManager.HasChanges;
            };

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
            Properties.Settings.Default.GridVisible = IsGridVisible;
            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// Saves current canvas as new file.
        /// summary>
        private bool SaveAs()
        {
            if (CurrentCanvas == null || RequestSaveAs == null)
                return false;

            string filePath = RequestSaveAs(CurrentCanvas.Settings.FileName);

            if (string.IsNullOrEmpty(filePath))
                return false;

            _canvasServiceFacade.Serialization.SaveCanvas(CurrentCanvas.ToDomain(), filePath);
            _trackableCommandManager.MarkSaved();

            FilePath = filePath;
            Messenger.Send(new UpdatedCanvasFilePathMessage(filePath));

            string fileName = Path.GetFileNameWithoutExtension(filePath);
            if (fileName != CurrentCanvas.Settings.FileName)
                CurrentCanvas.Settings.FileName = fileName;

            return true;
        }

        /// <summary>
        /// Saves current canvas.
        /// </summary>
        private bool Save()
        {
            if (CurrentCanvas is null)
            {
                return false;
            }

            if (!File.Exists(FilePath))
            {
                return SaveAs();
            }
            else
            {
                _canvasServiceFacade.Serialization.SaveCanvas(CurrentCanvas.ToDomain(), FilePath);
                _trackableCommandManager.MarkSaved();

                return true;
            }
        }

        /// <summary>
        /// Exports current canvas. 
        /// </summary>
        private void Export()
        {
            if (CurrentCanvas is not null && RequestExport is not null)
            {
                RequestExport();
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

        /// <summary>
        /// Archives different positions of an figure object.
        /// </summary>
        /// <remarks>
        /// This commands occurs when item position change and used to implement undo/redo event command.
        /// </remarks>
        /// <param name="e">Position changed event arguments.</param>
        [RelayCommand]
        private void ItemPositionChange(PositionChangedEventArgs e)
        {
            if (e.DataContext is FigureModel figure && Connections is not null)
            {
                _canvasServiceFacade.Interaction.MoveFigure(figure, e.OldPos, e.NewPos, Connections);
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
            // Get file path associated with current canvas.
            Messenger.Register<CanvasViewModel, CanvasFilePathMessage>(this, (r, m) =>
            {
                FilePath = m.Value;
            });
            // Answer request from canvas.
            Messenger.Register<CanvasViewModel, CurrentCanvasRequestMessage>(this, (r, m) =>
            {
                m.Reply(r.CurrentCanvas);
            });
            // Save and return result.
            Messenger.Register<CanvasViewModel, SaveRequestMessage>(this, (r, m) =>
            {
                m.Reply(Save());
            });
            // Register menu flags.
            Messenger.Register<CanvasViewModel, Tuple<string, bool>>(this, (r, m) =>
            {
                switch (m.Item1)
                {
                    case MenuFlags.IsBlocked: IsBlocked = m.Item2; break;
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
                    case CommandFlags.Export: Export(); break;
                    case CommandFlags.SaveAs: SaveAs(); break;
                }
            });
        }
    }
}
