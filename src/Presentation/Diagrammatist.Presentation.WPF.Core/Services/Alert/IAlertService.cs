using System.Windows;

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
        /// <returns><see cref="MessageBoxResult"/> as a result of user choice.</returns>
        MessageBoxResult RequestConfirmation(string message, string caption);
    }
}
