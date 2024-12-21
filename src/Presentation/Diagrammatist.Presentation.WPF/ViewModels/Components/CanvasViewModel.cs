using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Diagrammatist.Application.AppServices.Canvas.Services;
using Diagrammatist.Presentation.WPF.Framework.Commands.Undoable.Helpers;
using Diagrammatist.Presentation.WPF.Framework.Commands.Undoable.Manager;
using Diagrammatist.Presentation.WPF.Framework.Controls.Args;
using Diagrammatist.Presentation.WPF.Framework.Messages;
using Diagrammatist.Presentation.WPF.Mappers.Canvas;
using Diagrammatist.Presentation.WPF.Models.Canvas;
using Diagrammatist.Presentation.WPF.Models.Figures;
using Diagrammatist.Presentation.WPF.ViewModels.Components.Constants.Flags;
using Diagrammatist.Presentation.WPF.ViewModels.Components.Enums.Modes;
using System.Collections.ObjectModel;
using System.IO;

namespace Diagrammatist.Presentation.WPF.ViewModels.Components
{
    /// <summary>
    /// A view model class for canvas component.
    /// </summary>
    public sealed partial class CanvasViewModel : ObservableRecipient
    {
        private readonly ITrackableCommandManager _trackableCommandManager;
        private readonly ICanvasSerializationService _canvasSerializationService;

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

        /// <summary>
        /// Gets or sets current canvas.
        /// </summary>
        /// <remarks>
        /// This property used to store current canvas values.
        /// </remarks>
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
        private bool _isGridVisible = true;

        public CanvasViewModel(ITrackableCommandManager trackableCommandManager,
                               ICanvasSerializationService canvasSerializationService)
        {
            _trackableCommandManager = trackableCommandManager;
            _canvasSerializationService = canvasSerializationService;

            _trackableCommandManager.StateChanged += OnStateChanged;

            IsActive = true;
        }

        #region Commands

        /// <summary>
        /// Reverts changes that was made by user.
        /// </summary>
        private void Undo()
        {
            _trackableCommandManager.Undo();
        }

        /// <summary>
        /// Repeats changes that was reverted by user.
        /// </summary>
        private void Redo()
        {
            _trackableCommandManager.Redo();
        }

        /// <summary>
        /// Zooms canvas in.
        /// </summary>
        private void ZoomIn()
        {
            if (RequestZoomIn is not null)
            {
                RequestZoomIn();
            }
        }

        /// <summary>
        /// Zooms canvas out.
        /// </summary>
        private void ZoomOut()
        {
            if (RequestZoomOut is not null)
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
            IsGridVisible = !IsGridVisible;
        }

        /// <summary>
        /// Saves current canvas as new file.
        /// </summary>
        private void SaveAs()
        {
            if (CurrentCanvas is null || RequestSaveAs is null)
            {
                return;
            }

            var filePath = RequestSaveAs(CurrentCanvas.Settings.FileName);

            if (!string.IsNullOrEmpty(filePath))
            {
                _canvasSerializationService.SaveCanvas(CurrentCanvas.ToDomain(), filePath);
                _trackableCommandManager.MarkSaved();

                FilePath = filePath;
            }

            var fileName = Path.GetFileNameWithoutExtension(filePath);

            if (!string.IsNullOrEmpty(filePath) && fileName != CurrentCanvas.Settings.FileName)
            {
                CurrentCanvas.Settings.FileName = fileName;
            }
        }

        /// <summary>
        /// Saves current canvas.
        /// </summary>
        private void Save()
        {
            if (CurrentCanvas is null)
            {
                return;
            }

            if (!File.Exists(FilePath))
            {
                SaveAs();
            }
            else
            {
                _canvasSerializationService.SaveCanvas(CurrentCanvas.ToDomain(), FilePath);
                _trackableCommandManager.MarkSaved();
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
        [RelayCommand]
        private void DeleteItem(FigureModel figure)
        {
            var command = DeleteItemHelper.CreateDeleteItemCommand(CurrentCanvas?.Figures, figure);

            _trackableCommandManager.Execute(command);
        }

        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Behaviors/Member[@name="BringForwardItem"]/*'/>
        [RelayCommand]
        private void BringForwardItem(FigureModel figure)
        {
            if (CurrentCanvas?.Figures is null)
                return;

            var command = ZIndexAdjustmentHelper.CreateZIndexAdjustmentCommand(figure, forward: true);

            _trackableCommandManager.Execute(command);
        }

        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Behaviors/Member[@name="SendBackwardItem"]/*'/>
        [RelayCommand]
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
                var command = CommonUndoableHelper.CreateUndoableCommand(
                    () =>
                    {
                        figure.PosX = e.NewPos.X;
                        figure.PosY = e.NewPos.Y;
                    },
                    () =>
                    {
                        figure.PosX = e.OldPos.X;
                        figure.PosY = e.OldPos.Y;
                    }
                );

                _trackableCommandManager.Execute(command);
            }
        }

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
                // Center canvas.
                if (RequestZoomReset is not null)
                {
                    RequestZoomReset();
                }
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
                    case CommandFlags.Save:
                        Save();
                        break;
                    case CommandFlags.SaveAs:
                        SaveAs();
                        break;
                }
            });
        }
    }
}
