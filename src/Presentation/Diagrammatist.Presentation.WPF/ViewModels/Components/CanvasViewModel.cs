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

namespace Diagrammatist.Presentation.WPF.ViewModels.Components
{
    /// <summary>
    /// A view model class for canvas component.
    /// </summary>
    public sealed partial class CanvasViewModel : ObservableRecipient
    {
        private readonly IUndoableCommandManager _undoableCommandManager;
        private readonly ICanvasSerializationService _canvasSerializationService;

        /// <summary>
        /// Occurs when a request is made to zoom current window in.
        /// </summary>
        /// <remarks>
        /// This event is triggered when user initiates a zoom in action from menu button.
        /// </remarks>
        public event Action? OnRequestZoomIn;
        /// <summary>
        /// Occurs when a request is made to zoom current window out.
        /// </summary>
        /// <remarks>
        /// This event is triggered when user initiates a zoom out action from menu button.
        /// </remarks>
        public event Action? OnRequestZoomOut;
        /// <summary>
        /// Occurs when a request is made to reset current window zoom.
        /// </summary>
        /// <remarks>
        /// This event is triggered when user initiates a zoom reset action from menu button.
        /// </remarks>
        public event Action? OnRequestZoomReset;
        /// <summary>
        /// Occurs when a request is made to save current canvas as new file.
        /// </summary>
        /// <remarks>
        /// This event is triggered when user initiates a save as action from menu button and returns file path.
        /// </remarks>
        public event Func<string>? OnRequestSaveAs;
        /// <summary>
        /// Occurs when a requiest is made to export current canvas as bitmap.
        /// </summary>
        /// <remarks>
        /// This event is triggered when user initiates a export action from menu button.
        /// </remarks>
        public event Action? OnRequestExport;

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

        /// <summary>
        /// Gets or sets current mouse mode.
        /// </summary>
        /// <remarks>
        /// This property used to configure current mouse mode.
        /// </remarks>
        public MouseMode CurrentMouseMode
        {
            get => _currentMouseMode;
            private set => SetProperty(ref _currentMouseMode, value);
        }

        /// <summary>
        /// Gets or sets collection of <see cref="FigureModel"/>.
        /// </summary>
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

        public CanvasViewModel(IUndoableCommandManager undoableCommandManager,
                               ICanvasSerializationService canvasSerializationService)
        {
            _undoableCommandManager = undoableCommandManager;
            _canvasSerializationService = canvasSerializationService;

            IsActive = true;
        }

        #region Commands

        /// <summary>
        /// Reverts changes that was made by user.
        /// </summary>
        private void Undo()
        {
            _undoableCommandManager.Undo();
        }

        /// <summary>
        /// Repeats changes that was reverted by user.
        /// </summary>
        private void Redo()
        {
            _undoableCommandManager.Redo();
        }

        /// <summary>
        /// Zooms canvas in.
        /// </summary>
        private void ZoomIn()
        {
            if (OnRequestZoomIn is not null)
            {
                OnRequestZoomIn();
            }
        }

        /// <summary>
        /// Zooms canvas out.
        /// </summary>
        private void ZoomOut()
        {
            if (OnRequestZoomOut is not null)
            {
                OnRequestZoomOut();
            }
        }

        /// <summary>
        /// Resets current zoom.
        /// </summary>
        private void ZoomReset()
        {
            if (OnRequestZoomReset is not null)
            {
                OnRequestZoomReset();
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
            if (CurrentCanvas is null || OnRequestSaveAs is null)
            {
                return; 
            }

            string filePath = OnRequestSaveAs();

            if (!string.IsNullOrEmpty(filePath))
            {
                _canvasSerializationService.SaveCanvas(CurrentCanvas.ToDomain(), filePath);
            }
        }

        /// <summary>
        /// Exports current canvas. 
        /// </summary>
        private void Export()
        {
            if (CurrentCanvas is not null && OnRequestExport is not null)
            {
                OnRequestExport();
            }
        }

        /// <summary>
        /// Deletes item from canvas.
        /// </summary>
        /// <param name="figure">Target figure.</param>
        [RelayCommand]
        private void DeleteItem(FigureModel figure)
        {
            var command = DeleteItemHelper.CreateDeleteItemCommand(CurrentCanvas?.Figures, figure);

            _undoableCommandManager.Execute(command);
        }

        /// <summary>
        /// Brings item forward on canvas.
        /// </summary>
        /// <param name="figure">Target figure.</param>
        [RelayCommand]
        private void BringForwardItem(FigureModel figure)
        {
            if (CurrentCanvas?.Figures is null)
                return;

            var command = ZIndexAdjustmentHelper.CreateZIndexAdjustmentCommand(figure, forward: true);

            _undoableCommandManager.Execute(command);
        }

        /// <summary>
        /// Sends item backward on canvas.
        /// </summary>
        /// <param name="figure">Target figure.</param>
        [RelayCommand]
        private void SendBackwardItem(FigureModel figure)
        {
            if (CurrentCanvas?.Figures is null)
                return;

            var command = ZIndexAdjustmentHelper.CreateZIndexAdjustmentCommand(figure, forward: false);

            _undoableCommandManager.Execute(command);
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

                _undoableCommandManager.Execute(command);
            }
        }

        #endregion

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
                if (OnRequestZoomReset is not null)
                {
                    OnRequestZoomReset();
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
            // Answer request fro canvas.
            Messenger.Register<CanvasViewModel, CurrentCanvasRequestMessage>(this, (r, m) =>
            {
                m.Reply(r.CurrentCanvas);
            });
            // Register menu commands.
            Messenger.Register<CanvasViewModel, string>(this, (r, m) =>
            {
                switch (m)
                {
                    case MessengerFlags.Undo:
                        Undo();
                        break;
                    case MessengerFlags.Redo:
                        Redo();
                        break;
                    case MessengerFlags.ZoomIn:
                        ZoomIn();
                        break;
                    case MessengerFlags.ZoomOut:
                        ZoomOut();
                        break;
                    case MessengerFlags.ZoomReset:
                        ZoomReset();
                        break;
                    case MessengerFlags.EnableGrid:
                        EnableGrid();
                        break;
                    case MessengerFlags.Export:
                        Export();
                        break;
                    case MessengerFlags.Save:
                        break;
                    case MessengerFlags.SaveAs:
                        SaveAs();
                        break;
                }
            });
        }
    }
}
