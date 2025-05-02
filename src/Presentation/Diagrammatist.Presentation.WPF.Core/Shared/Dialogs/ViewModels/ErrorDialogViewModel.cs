using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Diagrammatist.Presentation.WPF.Core.Shared.Dialogs.ViewModels
{
    /// <summary>
    /// A class that represents error dialog view model and derives from <see cref="ObservableObject"/>.
    /// </summary>
    public partial class ErrorDialogViewModel : ObservableObject
    {
        /// <summary>
        /// Gets or sets the message to be displayed in the dialog.
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Gets or sets the caption of the dialog.
        /// </summary>
        public string Caption { get; }

        private readonly Action _closeAction;

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorDialogViewModel"/> class.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="caption"></param>
        /// <param name="closeAction"></param>
        public ErrorDialogViewModel(string message, string caption, Action closeAction)
        {
            Message = message;
            Caption = caption;
            _closeAction = closeAction;
        }

        /// <summary>
        /// Executes when the user clicks the "OK" button.
        /// </summary>
        [RelayCommand]
        private void Close()
        {
            _closeAction?.Invoke();
        }
    }
}
