using System.Windows;

namespace Diagrammatist.Presentation.WPF.Core.Services.Alert
{
    /// <summary>
    /// A class that implements <see cref="IAlertService"/>. Provides base operations for user alerts.
    /// </summary>
    public class AlertService : IAlertService
    {
        /// <inheritdoc/>
        public void ShowError(string message, string caption)
        {
            MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        /// <inheritdoc/>
        public MessageBoxResult RequestConfirmation(string message, string caption)
        {
            return MessageBox.Show(message, caption, MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
        }
    }
}
