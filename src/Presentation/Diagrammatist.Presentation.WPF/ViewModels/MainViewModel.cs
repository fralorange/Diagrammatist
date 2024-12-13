using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Diagrammatist.Presentation.WPF.Framework.Commands.Helpers;
using Diagrammatist.Presentation.WPF.Framework.Messages;
using Diagrammatist.Presentation.WPF.Models.Canvas;
using Diagrammatist.Presentation.WPF.ViewModels.Components.Constants.Flags;
using Diagrammatist.Presentation.WPF.ViewModels.Dialogs;
using MvvmDialogs;

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

        /// <summary>
        /// Occurs when a request is made to close the current canvas.
        /// </summary>
        /// <remarks>
        /// This event is triggered when user initiates a close action from menu button.
        /// </remarks>
        public event Action? OnRequestClose;

        public MainViewModel(IDialogService dialogService)
        {
            _dialogService = dialogService;
        }

        #region Menu

        #region File

        /// <summary>
        /// Creates a new canvas through dialog window.
        /// </summary>
        [RelayCommand]
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
        /// Closes current canvas.
        /// </summary>
        [RelayCommand]
        private void MenuClose()
        {
            Messenger.Send(MessengerFlags.CloseCanvas);
        }

        /// <summary>
        /// Closes all canvases.
        /// </summary>
        [RelayCommand]
        private void MenuCloseAll()
        {
            Messenger.Send(MessengerFlags.CloseCanvases);
        }

        [RelayCommand]
        private void MenuExport()
        {
            Messenger.Send(MessengerFlags.Export);
        }

        /// <summary>
        /// Exits from program.
        /// </summary>
        [RelayCommand]
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
        /// Cancels user last actions.
        /// </summary>
        [RelayCommand]
        private void MenuUndo()
        {
            Messenger.Send(MessengerFlags.Undo);
        }

        /// <summary>
        /// Repeats user last actions if they were undid.
        /// </summary>
        [RelayCommand]
        private void MenuRedo()
        {
            Messenger.Send(MessengerFlags.Redo);
        }
        #endregion
        #region Canvas

        /// <summary>
        /// Changes size of the current selected canvas through dialog window.
        /// </summary>
        [RelayCommand]
        private void MenuChangeSize()
        {
            var currentCanvas = Messenger.Send<CurrentCanvasRequestMessage>().Response;

            if (currentCanvas is null)
                return;

            var settings = new SettingsModel
            {
                Width = currentCanvas.Settings.Width,
                Height = currentCanvas.Settings.Height,
                FileName = currentCanvas.Settings.FileName,
                Background = currentCanvas.Settings.Background,
                Type = currentCanvas.Settings.Type,
            };

            var dialogViewModel = new ChangeCanvasSizeDialogViewModel(settings);

            var success = _dialogService.ShowDialog(this, dialogViewModel);
            if (success == true)
            {
                var newSettings = dialogViewModel.Settings!;

                Messenger.Send<UpdatedSettingsMessage>(new(newSettings));
            }
        }

        #endregion
        #region View

        /// <summary>
        /// Zooms in to dynamic center.
        /// </summary>
        [RelayCommand]
        private void MenuZoomIn()
        {
            Messenger.Send(MessengerFlags.ZoomIn);
        }

        /// <summary>
        /// Zoms out from dynamic center.
        /// </summary>
        [RelayCommand]
        private void MenuZoomOut()
        {
            Messenger.Send(MessengerFlags.ZoomOut);
        }

        /// <summary>
        /// Resets current zoom to default.
        /// </summary>
        [RelayCommand]
        private void MenuZoomReset()
        {
            Messenger.Send(MessengerFlags.ZoomReset);
        }

        /// <summary>
        /// Enables or disables grid visual.
        /// </summary>
        [RelayCommand]
        private void MenuEnableGrid()
        {
            Messenger.Send(MessengerFlags.EnableGrid);
        }

        #endregion
        #region Help

        /// <summary>
        /// Opens app wiki. 
        /// </summary>
        [RelayCommand]
        private void MenuHelp()
        {
            var helpUrl = App.Current.Resources["HelpUrl"] as string;

            UrlHelper.OpenUrl(helpUrl);
        }

        /// <summary>
        /// Opens dialog 'about' window.
        /// </summary>
        [RelayCommand]
        private void MenuAbout()
        {
            var dialogViewModel = new AboutAppDialogViewModel();

            _dialogService.ShowDialog(this, dialogViewModel);
        }

        #endregion

        #endregion
    }
}