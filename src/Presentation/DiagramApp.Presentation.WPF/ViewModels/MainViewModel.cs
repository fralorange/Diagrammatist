using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using DiagramApp.Presentation.WPF.Messages;
using DiagramApp.Presentation.WPF.ViewModels.Dialogs;
using MvvmDialogs;
using System.Windows.Input;

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

        public MainViewModel(IDialogService dialogService)
        {
            _dialogService = dialogService;
        }

        #region Menu

        #region File
        [RelayCommand]
        public void MenuNew()
        {
            var dialogViewModel = new AddCanvasDialogViewModel();

            var success = _dialogService.ShowDialog(this, dialogViewModel);
            if (success == true)
            {
                var settings = dialogViewModel.Settings;

                Messenger.Send<NewCanvasSettingsMessage>(new(settings!));
            }
        }

        #endregion
        #region Edit
        [RelayCommand]
        public void MenuUndo()
        {

        }

        [RelayCommand]
        public void MenuRedo()
        {

        }
        #endregion
        #region Canvas

        #endregion
        #region View

        #endregion
        #region Help

        #endregion

        #endregion
    }
}