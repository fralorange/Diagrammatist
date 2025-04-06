using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MvvmDialogs;

namespace Diagrammatist.Presentation.WPF.Core.Shared.Dialogs.ViewModels
{
    /// <summary>
    /// A class that derives from <see cref="ObservableObject"/>.
    /// Represents 'print' functionality inside dialog window.
    /// </summary>
    public partial class OutputDialogViewModel : ObservableObject, IModalDialogViewModel
    {
        private bool? _dialogResult;

        /// <inheritdoc/>
        public bool? DialogResult
        {
            get => _dialogResult;
            private set => SetProperty(ref _dialogResult, value);
        }

        /// <summary>
        /// Gets or sets output message.
        /// </summary>
        [ObservableProperty]
        private string _message;

        /// <summary>
        /// Initializes <see cref="OutputDialogViewModel"/>.
        /// </summary>
        /// <param name="message"></param>
        public OutputDialogViewModel(string message)
        {
            Message = message;
        }

        /// <summary>
        /// Confirms dialog result.
        /// </summary>
        [RelayCommand]
        private void Ok()
        {
            DialogResult = true;
        }
    }
}
