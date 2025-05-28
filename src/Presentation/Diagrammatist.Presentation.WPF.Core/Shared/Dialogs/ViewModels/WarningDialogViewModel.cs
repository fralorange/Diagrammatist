using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Diagrammatist.Presentation.WPF.Core.Shared.Records;
using Diagrammatist.Presentation.WPF.Core.Shared.Enums;

namespace Diagrammatist.Presentation.WPF.Core.Shared.Dialogs.ViewModels
{
    /// <summary>
    /// A that derives from <see cref="ObservableObject"/>. 
    /// </summary>
    public partial class WarningDialogViewModel : ObservableObject
    {
        /// <summary>
        /// Gets or sets the message to be displayed in the dialog.
        /// </summary>
        public string Message { get; }
        /// <summary>
        /// Gets or sets the caption of the dialog.
        /// </summary>
        public string Caption { get; }

        /// <summary>
        /// Gets or sets whether the dialog window would show again.
        /// </summary>
        [ObservableProperty]
        private bool doNotShowAgain;

        private readonly Action<ConfirmationResponse> _respond;

        /// <summary>
        /// Initializes a new instance of the <see cref="WarningDialogViewModel"/> class.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="caption"></param>
        /// <param name="respond"></param>
        public WarningDialogViewModel(string message, string caption, Action<ConfirmationResponse> respond)
        {
            Message = message;
            Caption = caption;
            _respond = respond;
        }

        /// <summary>
        /// Continues the dialog and returns a <see cref="ConfirmationResponse"/> with <see cref="ConfirmationResult.Yes"/> result.
        /// </summary>
        [RelayCommand]
        private void Continue() => _respond(new ConfirmationResponse(ConfirmationResult.Yes, DoNotShowAgain));

        /// <summary>
        /// Cancels the dialog and returns a <see cref="ConfirmationResponse"/> with <see cref="ConfirmationResult.Cancel"/> result.
        /// </summary>
        [RelayCommand]
        private void Cancel() => _respond(new ConfirmationResponse(ConfirmationResult.Cancel, DoNotShowAgain));
    }
}
