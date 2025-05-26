using Diagrammatist.Presentation.WPF.Core.Shared.Records;
using Diagrammatist.Presentation.WPF.Core.Shared.Enums;

namespace Diagrammatist.Presentation.WPF.Core.Services.Alert
{
    /// <summary>
    /// An interface that used to alert user of some action (e.g. warning or some action confirmation).
    /// </summary>
    public interface IAlertService
    {
        /// <summary>
        /// Shows simple error message to user.
        /// </summary>
        /// <param name="message">Error message.</param>
        /// <param name="caption">Error caption.</param>
        void ShowError(string message, string caption);
        /// <summary>
        /// Requests user confirmation.
        /// </summary>
        /// <param name="message">Confirmation message.</param>
        /// <param name="caption">Confirmation caption.</param>
        /// <returns><see cref="ConfirmationResult"/> as a result of user choice.</returns>
        ConfirmationResult RequestConfirmation(string message, string caption);
        /// <summary>
        /// Shows warning message to user.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="caption"></param>
        /// <returns></returns>
        ConfirmationResponse ShowWarning(string message, string caption);
    }
}
