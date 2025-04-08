using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Diagrammatist.Application.AppServices.Canvas.Services;
using Diagrammatist.Presentation.WPF.Core.Commands.Helpers.General;
using Diagrammatist.Presentation.WPF.Core.Commands.Helpers.Undoable;
using Diagrammatist.Presentation.WPF.Core.Commands.Managers;
using Diagrammatist.Presentation.WPF.Core.Controls.Args;
using Diagrammatist.Presentation.WPF.Core.Services.Connection;
using Diagrammatist.Presentation.WPF.Core.Mappers.Canvas;
using Diagrammatist.Presentation.WPF.Core.Messaging.Messages;
using Diagrammatist.Presentation.WPF.Core.Messaging.RequestMessages;
using Diagrammatist.Presentation.WPF.Core.Models.Canvas;
using Diagrammatist.Presentation.WPF.Core.Models.Connection;
using Diagrammatist.Presentation.WPF.Core.Models.Figures;
using Diagrammatist.Presentation.WPF.Core.Services.Clipboard;
using Diagrammatist.Presentation.WPF.ViewModels.Components.Constants.Flags;
using Diagrammatist.Presentation.WPF.ViewModels.Components.Enums.Modes;
using System.Collections.ObjectModel;
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
        private readonly ICanvasSerializationService _canvasSerializationService;
        private readonly IClipboardService<FigureModel> _clipboardManager;
        private readonly IConnectionService _connectionService;

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

        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Behaviors/Member[@name="ViewModelFigures"]/*'/>
        /// <remarks>
        /// This property used to send figures as message to other components that require it.
        /// </remarks>
        [ObservableProperty]
        [NotifyPropertyChangedRecipients]
        private ObservableCollection<FigureModel>? _figures;

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
                               ICanvasSerializationService canvasSerializationService,
                               IClipboardService<FigureModel> clipboardManager,
                               IConnectionService connectionManager)
        {
            _trackableCommandManager = trackableCommandManager;
            _canvasSerializationService = canvasSerializationService;
            _clipboardManager = clipboardManager;
            _connectionService = connectionManager;

            _trackableCommandManager.StateChanged += OnStateChanged;

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

            _canvasSerializationService.SaveCanvas(CurrentCanvas.ToDomain(), filePath);
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
                _canvasSerializationService.SaveCanvas(CurrentCanvas.ToDomain(), FilePath);
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
            if (CurrentCanvas?.Figures is null)
                return;

            var command = DeleteItemHelper.CreateDeleteItemCommand(CurrentCanvas.Figures, figure, _connectionService, CurrentCanvas.Connections);

            _trackableCommandManager.Execute(command);
        }

        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Behaviors/Member[@name="Paste"]/*'/>
        [RelayCommand(CanExecute = nameof(MenuIsNotBlocked))]
        private void Paste(object position)
        {
            if (CurrentCanvas is not null && _clipboardManager.PasteFromClipboard() is { } pastedFigure && position is Point destination)
            {
                var command = PasteHelper.CreatePasteCommand(
                    CurrentCanvas.Figures,
                    pastedFigure,
                    () => SelectedFigure,
                    figure => SelectedFigure = figure,
                    (figure, x, y) =>
                    {
                        figure.PosX = x;
                        figure.PosY = y;
                    },
                    new(destination.X, destination.Y));

                _trackableCommandManager.Execute(command);
            }
        }

        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Behaviors/Member[@name="Copy"]/*'/>
        [RelayCommand(CanExecute = nameof(MenuIsNotBlocked))]
        private void Copy()
        {
            if (SelectedFigure is not null && CurrentCanvas is not null)
            {
                CopyHelper.Copy(_clipboardManager, SelectedFigure);
            }
        }

        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Behaviors/Member[@name="Cut"]/*'/>
        [RelayCommand(CanExecute = nameof(MenuIsNotBlocked))]
        private void Cut()
        {
            if (SelectedFigure is not null && CurrentCanvas is not null)
            {
                var command = CutHelper.CreateCutCommand(
                    _clipboardManager,
                    CurrentCanvas!.Figures,
                    () => SelectedFigure,
                    figure => SelectedFigure = figure);

                _trackableCommandManager.Execute(command);
            }
        }

        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Behaviors/Member[@name="Duplicate"]/*'/>
        [RelayCommand(CanExecute = nameof(MenuIsNotBlocked))]
        private void Duplicate()
        {
            if (SelectedFigure is not null && CurrentCanvas is not null)
            {
                var command = DuplicateHelper.CreateDuplicateCommand(
                    CurrentCanvas!.Figures,
                    () => SelectedFigure,
                    figure => SelectedFigure = figure,
                    figure => figure.Clone());

                _trackableCommandManager.Execute(command);
            }
        }

        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Behaviors/Member[@name="BringForwardItem"]/*'/>
        [RelayCommand(CanExecute = nameof(MenuIsNotBlocked))]
        private void BringForwardItem(FigureModel figure)
        {
            if (CurrentCanvas?.Figures is null)
                return;

            var command = ZIndexAdjustmentHelper.CreateZIndexAdjustmentCommand(figure, forward: true);

            _trackableCommandManager.Execute(command);
        }

        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Behaviors/Member[@name="SendBackwardItem"]/*'/>
        [RelayCommand(CanExecute = nameof(MenuIsNotBlocked))]
        private void SendBackwardItem(FigureModel figure)
        {
            if (CurrentCanvas?.Figures is null)
                return;

            var command = ZIndexAdjustmentHelper.CreateZIndexAdjustmentCommand(figure, forward: false);

            _trackableCommandManager.Execute(command);
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
            if (e.DataContext is FigureModel figure)
            {
                var lineUpdater = UpdateLineIfExists(figure);

                var command = CommonUndoableHelper.CreateUndoableCommand(
                    () =>
                    {
                        UpdatePositions(figure, e.NewPos);
                        ValidateConnections(e.DataContext, e.NewPos, e.OldPos);
                        lineUpdater?.Invoke(false);
                    },
                    () =>
                    {
                        UpdatePositions(figure, e.OldPos);
                        ValidateConnections(e.DataContext, e.OldPos, e.NewPos);
                        lineUpdater?.Invoke(true);
                    }
                );

                _trackableCommandManager.Execute(command);
            }
        }

        #region ItemPositionChange sub operations

        private void UpdatePositions(FigureModel figure, Point newPos)
        {
            figure.PosX = newPos.X;
            figure.PosY = newPos.Y;
        }

        private void UpdateConnections(ConnectionModel connection, Point newPos, bool isSource)
        {
            if (isSource)
            {
                connection.SourceMagneticPoint!.Position = newPos;
                connection.Line.Points[0] = newPos;
            }
            else
            {
                connection.DestinationMagneticPoint!.Position = newPos;
                connection.Line.Points[^1] = newPos;
            }
        }

        private void ValidateConnections(object? dataContext, Point newPos, Point oldPos)
        {
            if (dataContext is ShapeFigureModel shapeFigure && Connections is not null)
            {
                var connections = _connectionService.GetConnections(Connections, shapeFigure);

                foreach (var connection in connections)
                {
                    var source = connection.SourceMagneticPoint;
                    var dest = connection.DestinationMagneticPoint;

                    var deltaX = newPos.X - oldPos.X;
                    var deltaY = newPos.Y - oldPos.Y;

                    var isSource = source?.Owner == shapeFigure;

                    var currentPoint = isSource ? source!.Position : dest!.Position;
                    var nextPos = new Point(currentPoint.X + deltaX, currentPoint.Y + deltaY);

                    UpdateConnections(connection, nextPos, isSource);
                }
            }
        }

        private Action<bool>? UpdateLineIfExists(FigureModel figure)
        {
            if (figure is LineFigureModel lineFigure && CurrentCanvas?.Connections is { } connections && _connectionService.GetConnection(connections, lineFigure) is { } connection)
            {
                return new Action<bool>((revert) =>
                {
                    if (revert)
                        _connectionService.AddConnection(connections, connection);
                    else
                        _connectionService.RemoveConnection(connections, connection);
                });
            }

            return null;
        }

        #endregion

        #endregion

        private void OnStateChanged(object? sender, EventArgs e)
        {
            if (CurrentCanvas is not null)
            {
                CurrentCanvas.HasChanges = _trackableCommandManager.HasChanges;
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
                    case MenuFlags.IsBlocked:
                        IsBlocked = m.Item2;
                        break;
                }
            });
            // Register menu commands.
            Messenger.Register<CanvasViewModel, string>(this, (r, m) =>
            {
                switch (m)
                {
                    case CommandFlags.Undo:
                        Undo();
                        break;
                    case CommandFlags.Redo:
                        Redo();
                        break;
                    case CommandFlags.ZoomIn:
                        ZoomIn();
                        break;
                    case CommandFlags.ZoomOut:
                        ZoomOut();
                        break;
                    case CommandFlags.ZoomReset:
                        ZoomReset();
                        break;
                    case CommandFlags.EnableGrid:
                        EnableGrid();
                        break;
                    case CommandFlags.Export:
                        Export();
                        break;
                    case CommandFlags.SaveAs:
                        SaveAs();
                        break;
                }
            });
        }
    }
}
