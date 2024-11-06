using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using DiagramApp.Contracts.Settings;
using DiagramApp.Presentation.WPF.Framework.Commands.Helpers;
using DiagramApp.Presentation.WPF.Framework.Messages;
using DiagramApp.Presentation.WPF.ViewModels.Components.Consts.Flags;
using DiagramApp.Presentation.WPF.ViewModels.Dialogs;
using MvvmDialogs;

namespace DiagramApp.Presentation.WPF.ViewModels
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

        public event Action? OnRequestClose;

        public MainViewModel(IDialogService dialogService)
        {
            _dialogService = dialogService;
        }

        #region Menu

        #region File
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

        [RelayCommand]
        private void MenuClose()
        {
            Messenger.Send(MessengerFlags.CloseCanvas);
        }

        [RelayCommand]
        private void MenuCloseAll()
        {
            Messenger.Send(MessengerFlags.CloseCanvases);
        }

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
        [RelayCommand]
        private void MenuUndo()
        {
            Messenger.Send(MessengerFlags.Undo);
        }

        [RelayCommand]
        private void MenuRedo()
        {
            Messenger.Send(MessengerFlags.Redo);
        }
        #endregion
        #region Canvas

        [RelayCommand]
        private void MenuChangeSize()
        {
            var currentCanvas = Messenger.Send<CurrentCanvasRequestMessage>().Response;

            if (currentCanvas is null)
                return;

            var settings = new DiagramSettingsDto
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

        [RelayCommand]
        private void MenuZoomIn()
        {
            Messenger.Send(MessengerFlags.ZoomIn);
        }

        [RelayCommand]
        private void MenuZoomOut()
        {
            Messenger.Send(MessengerFlags.ZoomOut);
        }

        [RelayCommand]
        private void MenuZoomReset()
        {
            Messenger.Send(MessengerFlags.ZoomReset);
        }

        [RelayCommand]
        private void MenuEnableGrid()
        {
            Messenger.Send(MessengerFlags.EnableGrid);
        }

        #endregion
        #region Help

        [RelayCommand]
        private void MenuHelp()
        {
            var helpUrl = App.Current.Resources["HelpUrl"] as string;

            UrlHelper.OpenUrl(helpUrl);
        }

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