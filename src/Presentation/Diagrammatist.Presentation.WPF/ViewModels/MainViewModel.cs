using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Diagrammatist.Presentation.WPF.Core.Commands.Helpers.General;
using Diagrammatist.Presentation.WPF.Core.Commands.Managers;
using Diagrammatist.Presentation.WPF.Core.Messaging.Messages;
using Diagrammatist.Presentation.WPF.Core.Messaging.RequestMessages;
using Diagrammatist.Presentation.WPF.Simulator.ViewModels;
using Diagrammatist.Presentation.WPF.ViewModels.Components.Constants.Flags;
using Diagrammatist.Presentation.WPF.ViewModels.Dialogs;
using MvvmDialogs;
using System.Configuration;

namespace Diagrammatist.Presentation.WPF.ViewModels
{
    /// <summary>
    /// A view model class for main window.
    /// </summary>
    /// <remarks>
    /// This view model used to process menu and system commands.
    /// <para>
    /// Also used as mediator between menu options and view models.
    /// </para>
    /// </remarks>
    public sealed partial class MainViewModel : ObservableRecipient
    {
        private readonly IDialogService _dialogService;
        private readonly ITrackableCommandManager _trackableCommandManager;

        /// <summary>
        /// Occurs when a request is made to close the current canvas.
        /// </summary>
        /// <remarks>
        /// This event is triggered when user initiates a close action from menu button.
        /// </remarks>
        public event Action? OnRequestClose;

        #region MenuFlags
        /// <inheritdoc cref="ITrackableCommandManager.HasGlobalChanges"/>
        public bool HasGlobalChangesFlag => _trackableCommandManager.HasGlobalChanges;
        /// <inheritdoc cref="ITrackableCommandManager.HasChanges"/>
        public bool HasChangesFlag => _trackableCommandManager.HasChanges;
        /// <inheritdoc cref="IUndoableCommandManager.CanUndo"/>
        public bool HasUndoFlag => _trackableCommandManager.CanUndo;
        /// <inheritdoc cref="IUndoableCommandManager.CanRedo"/>
        public bool HasRedoFlag => _trackableCommandManager.CanRedo;
        /// <summary>
        /// Gets or sets 'has canvas' flag.
        /// </summary>
        /// <remarks>
        /// This property used to determine whether app has a canvas on the screen right now or not.
        /// </remarks>
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(
            nameof(MenuCloseCommand),
            nameof(MenuCloseAllCommand),
            nameof(MenuSaveAsCommand),
            nameof(MenuExportCommand),
            nameof(MenuZoomInCommand),
            nameof(MenuZoomOutCommand),
            nameof(MenuZoomResetCommand),
            nameof(MenuEnableGridCommand),
            nameof(MenuSimulatorCommand),
            nameof(MenuChangeSizeCommand))]
        private bool _hasCanvasFlag;
        /// <summary>
        /// Gets or sets 'has custom canvas' flag.
        /// </summary>
        /// <remarks>
        /// This property used to determine whether app current canvas is custom or not.
        /// </remarks>
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(MenuSimulatorCommand))]
        private bool _hasCustomCanvasFlag;
        /// <summary>
        /// Gets 'has grid flag' from client-prefs (be default: True)
        /// </summary>
        /// <remarks>
        /// This property used to show checkmarks in front of grid menu item, for visual sake..
        /// </remarks>
        [ObservableProperty]
        private bool _hasGridFlag = Properties.Settings.Default.GridVisible;
        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Behaviors/Member[@name="IsBlocked"]/*'/>
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotBlocked))]
        [NotifyCanExecuteChangedFor(
            nameof(MenuNewCommand), 
            nameof(MenuOpenCommand),
            nameof(MenuCloseCommand),
            nameof(MenuCloseAllCommand),
            nameof(MenuSaveCommand),
            nameof(MenuSaveAsCommand),
            nameof(MenuSaveAllCommand),
            nameof(MenuExportCommand),
            nameof(MenuExitCommand),
            nameof(MenuUndoCommand),
            nameof(MenuRedoCommand),
            nameof(MenuZoomInCommand),
            nameof(MenuZoomOutCommand),
            nameof(MenuZoomResetCommand),
            nameof(MenuSimulatorCommand),
            nameof(MenuChangeSizeCommand))]
        private bool _isBlocked;
        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Behaviors/Member[@name="IsNotBlocked"]/*'/>
        public bool IsNotBlocked => !IsBlocked;
        #endregion

        public MainViewModel(IDialogService dialogService, ITrackableCommandManager trackableCommandManager)
        {
            _dialogService = dialogService;
            _trackableCommandManager = trackableCommandManager;

            ConfigureEvents();

            IsActive = true;
        }

        private void ConfigureEvents()
        {
            _trackableCommandManager.StateChanged += OnStateChanged;
            _trackableCommandManager.OperationPerformed += OnOperationPerformed;

            Properties.Settings.Default.SettingChanging += OnSettingsChanged;
        }

        #region Menu

        private bool MenuCanExecute()
        {
            return IsNotBlocked;
        }

        private bool CanvasCanExecute()
        {
            return HasCanvasFlag;
        }

        private bool MenuWithCanvasCanExecute()
        {
            return MenuCanExecute() && CanvasCanExecute();
        }

        private bool MenuWithNotCustomCanvasCanExecute()
        {
            return MenuCanExecute() && CanvasCanExecute() && !HasCustomCanvasFlag;
        }

        private bool MenuWithCanvasChangesCanExecute()
        {
            return MenuCanExecute() && HasChangesFlag;
        }

        private bool MenuWithGlobalChangesCanExecute()
        {
            return MenuCanExecute() && HasGlobalChangesFlag;
        }

        private bool MenuWithUndoCanExecute()
        {
            return MenuCanExecute() && HasUndoFlag;
        }

        private bool MenuWithRedoCanExecute()
        {
            return MenuCanExecute() && HasRedoFlag;
        }

        #region File

        /// <summary>
        /// Creates a new canvas through dialog window from menu button.
        /// </summary>
        [RelayCommand(CanExecute = nameof(MenuCanExecute))]
        private void MenuNew()
        {
            var dialogViewModel = new AddCanvasDialogViewModel();

            var success = _dialogService.ShowDialog(this, dialogViewModel);
            if (success == true)
            {
                var settings = dialogViewModel.Settings;

                Messenger.Send<NewCanvasSettingsMessage>(new(settings!));
            }
        }

        /// <summary>
        /// Opens an existing canvas from menu button.
        /// </summary>
        [RelayCommand(CanExecute = nameof(MenuCanExecute))]
        private void MenuOpen()
        {
            Messenger.Send(CommandFlags.Open);
        }

        /// <summary>
        /// Closes current canvas from menu button.
        /// </summary>
        [RelayCommand(CanExecute = nameof(MenuWithCanvasCanExecute))]
        private void MenuClose()
        {
            Messenger.Send(CommandFlags.CloseCanvas);
        }

        /// <summary>
        /// Closes all canvases from menu button.
        /// </summary>
        [RelayCommand(CanExecute = nameof(MenuWithCanvasCanExecute))]
        private void MenuCloseAll()
        {
            Messenger.Send(CommandFlags.CloseCanvases);
        }

        /// <summary>
        /// Saves current canvas as new file from menu button.
        /// </summary>
        [RelayCommand(CanExecute = nameof(MenuWithCanvasCanExecute))]
        private void MenuSaveAs()
        {
            Messenger.Send(CommandFlags.SaveAs);
        }

        /// <summary>
        /// Saves all canvases from menu button.
        /// </summary>
        [RelayCommand(CanExecute = nameof(MenuWithGlobalChangesCanExecute))]
        private void MenuSaveAll()
        {
            SaveAll();
        }

        /// <summary>
        /// Saves all canvases from indirect call.
        /// </summary>
        /// <returns>Saving operation result.</returns>
        public bool SaveAll()
        {
            return Messenger.Send(new SaveAllRequestMessage());
        }

        /// <summary>
        /// Saves current canvas from menu button.
        /// </summary>
        [RelayCommand(CanExecute = nameof(MenuWithCanvasChangesCanExecute))]
        private void MenuSave()
        {
            Messenger.Send(new SaveRequestMessage());
        }

        /// <summary>
        /// Exports current canvas as bitmap file from menu button.
        /// </summary>
        [RelayCommand(CanExecute = nameof(MenuWithCanvasCanExecute))]
        private void MenuExport()
        {
            Messenger.Send(CommandFlags.Export);
        }

        /// <summary>
        /// Exits from program from menu button.
        /// </summary>
        [RelayCommand(CanExecute = nameof(MenuCanExecute))]
        private void MenuExit()
        {
            if (OnRequestClose is not null)
            {
                OnRequestClose();
            }
        }

        #endregion
        #region Edit

        /// <summary>
        /// Cancels user last actions from menu button.
        /// </summary>
        [RelayCommand(CanExecute = nameof(MenuWithUndoCanExecute))]
        private void MenuUndo()
        {
            Messenger.Send(CommandFlags.Undo);
        }

        /// <summary>
        /// Repeats user last actions if they were undid from menu button.
        /// </summary>
        [RelayCommand(CanExecute = nameof(MenuWithRedoCanExecute))]
        private void MenuRedo()
        {
            Messenger.Send(CommandFlags.Redo);
        }
        #endregion
        #region View

        /// <summary>
        /// Zooms in to dynamic center from menu button.
        /// </summary>
        [RelayCommand(CanExecute = nameof(MenuWithCanvasCanExecute))]
        private void MenuZoomIn()
        {
            Messenger.Send(CommandFlags.ZoomIn);
        }

        /// <summary>
        /// Zoms out from dynamic center from menu button.
        /// </summary>
        [RelayCommand(CanExecute = nameof(MenuWithCanvasCanExecute))]
        private void MenuZoomOut()
        {
            Messenger.Send(CommandFlags.ZoomOut);
        }

        /// <summary>
        /// Resets current zoom to default from menu button.
        /// </summary>
        [RelayCommand(CanExecute = nameof(MenuWithCanvasCanExecute))]
        private void MenuZoomReset()
        {
            Messenger.Send(CommandFlags.ZoomReset);
        }

        /// <summary>
        /// Enables or disables grid visual from menu button.
        /// </summary>
        [RelayCommand(CanExecute = nameof(CanvasCanExecute))]
        private void MenuEnableGrid()
        {
            Messenger.Send(CommandFlags.EnableGrid);
        }

        #endregion
        #region Canvas

        /// <summary>
        /// Changes size of the current selected canvas through dialog window from menu button.
        /// </summary>
        [RelayCommand(CanExecute = nameof(MenuWithCanvasCanExecute))]
        private void MenuChangeSize()
        {
            var currentCanvas = Messenger.Send<CurrentCanvasRequestMessage>().Response;

            if (currentCanvas is null)
                return;

            var dialogViewModel = new ChangeCanvasSizeDialogViewModel(currentCanvas.Settings.Width, currentCanvas.Settings.Height);

            if (_dialogService.ShowDialog(this, dialogViewModel) == true && dialogViewModel.Size is { } size)
            {
                Messenger.Send<UpdatedSizeMessage>(new(size));
            }
        }

        #endregion
        #region Tools
        /// <summary>
        /// Opens dialog 'simulator' window from menu button.
        /// </summary>
        [RelayCommand(CanExecute = nameof(MenuWithNotCustomCanvasCanExecute))]
        private void MenuSimulator()
        {
            var dialogViewModel = new SimulatorWindowViewModel();

            _dialogService.ShowDialog(this, dialogViewModel);
        }

        /// <summary>
        /// Opens dialog 'preferences' window from menu button.
        /// </summary>
        [RelayCommand]
        private void MenuPreferences()
        {
            var dialogViewModel = new SettingsDialogViewModel();

            _dialogService.ShowDialog(this, dialogViewModel);
        }
        #endregion
        #region Help

        /// <summary>
        /// Opens app wiki from menu button. 
        /// </summary>
        [RelayCommand]
        private void MenuHelp()
        {
            var helpUrl = App.Current.Resources["HelpUrl"] as string;

            UrlHelper.OpenUrl(helpUrl);
        }

        /// <summary>
        /// Opens dialog 'about' window from menu button.
        /// </summary>
        [RelayCommand]
        private void MenuAbout()
        {
            var dialogViewModel = new AboutAppDialogViewModel();

            _dialogService.ShowDialog(this, dialogViewModel);
        }

        #endregion

        #endregion

        private void OnStateChanged(object? sender, EventArgs e)
        {
            OnPropertyChanged(nameof(HasGlobalChangesFlag));
            OnPropertyChanged(nameof(HasChangesFlag));

            MenuSaveAllCommand.NotifyCanExecuteChanged();
            MenuSaveCommand.NotifyCanExecuteChanged();
        }

        private void OnOperationPerformed(object? sender, EventArgs e)
        {
            OnPropertyChanged(nameof(HasUndoFlag));
            OnPropertyChanged(nameof(HasRedoFlag));

            MenuUndoCommand.NotifyCanExecuteChanged();
            MenuRedoCommand.NotifyCanExecuteChanged();
        }

        private void OnSettingsChanged(object? sender, SettingChangingEventArgs e)
        {
            if (e.SettingName == nameof(Properties.Settings.Default.GridVisible) && e.NewValue is bool gridVisible)
            {
                HasGridFlag = gridVisible;
            }
        }

        protected override void OnActivated()
        {
            base.OnActivated();

            // Configures menuflag - bool values.
            Messenger.Register<MainViewModel, Tuple<string, bool>>(this, (r, m) =>
            {
                switch (m.Item1)
                {
                    case MenuFlags.HasCanvas:
                        HasCanvasFlag = m.Item2;
                        break;
                    case MenuFlags.HasCustomCanvas:
                        HasCustomCanvasFlag = m.Item2;
                        break;
                    case MenuFlags.IsBlocked:
                        IsBlocked = m.Item2;
                        break;
                }
            });
        }
    }
}