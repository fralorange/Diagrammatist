using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Diagrammatist.Presentation.WPF.Core.Shared.Dialogs.ViewModels
{
    /// <summary>
    /// A class that represents viewmodel of confirmation dialog window.
    /// </summary>
    public partial class ConfirmationDialogViewModel : ObservableObject
    {
        /// <summary>
        /// Gets or sets the message to be displayed in the dialog.
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Gets or sets the caption of the dialog.
        /// </summary>
        public string Caption { get; }

        private readonly Action<bool?> _respond;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfirmationDialogViewModel"/> class.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="caption"></param>
        /// <param name="respond"></param>
        public ConfirmationDialogViewModel(string message, string caption, Action<bool?> respond)
        {
            Message = message;
            Caption = caption;
            _respond = respond;
        }

        /// <summary>
        /// Executes when the user clicks the "Yes" button.
        /// </summary>
        [RelayCommand]
        private void Yes() => _respond(true);

        /// <summary>
        /// Executes when the user clicks the "No" button.
        /// </summary>
        [RelayCommand]
        private void No() => _respond(false);

        /// <summary>
        /// Executes when the user clicks the "Cancel" button.
        /// </summary>
        [RelayCommand]
        private void Cancel() => _respond(null);
    }
}
