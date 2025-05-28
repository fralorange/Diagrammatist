using Diagrammatist.Presentation.WPF.Core.Services.Sound;
using Diagrammatist.Presentation.WPF.Core.Shared.Dialogs.ViewModels;
using Diagrammatist.Presentation.WPF.Core.Shared.Dialogs.Views;
using Diagrammatist.Presentation.WPF.Core.Shared.Records;
using Diagrammatist.Presentation.WPF.Core.Shared.Enums;
using System.Windows;
using ApplicationEnt = System.Windows.Application;

namespace Diagrammatist.Presentation.WPF.Core.Services.Alert
{
    /// <summary>
    /// A class that implements <see cref="IAlertService"/>. Provides base operations for user alerts.
    /// </summary>
    public class AlertService : IAlertService
    {
        private readonly ISoundService _soundService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AlertService"/> class.
        /// </summary>
        /// <param name="soundService"></param>
        public AlertService(ISoundService soundService)
        {
            _soundService = soundService;
        }

        /// <inheritdoc/>
        public void ShowError(string message, string caption)
        {
            var owner = GetActiveWindow();
            var dialog = new ErrorDialog { Owner = owner };
            var vm = new ErrorDialogViewModel(message, caption, () => dialog.Close());

            _soundService.PlayWarningSound();
            dialog.DataContext = vm;
            dialog.ShowDialog();
        }

        /// <inheritdoc/>
        public ConfirmationResult RequestConfirmation(string message, string caption)
        {
            var owner = GetActiveWindow();
            var dialog = new ConfirmationDialog { Owner = owner };

            var result = ConfirmationResult.None;

            var vm = new ConfirmationDialogViewModel(message, caption, r =>
            {
                result = r switch
                {
                    true => ConfirmationResult.Yes,
                    false => ConfirmationResult.No,
                    _ => ConfirmationResult.Cancel
                };
                dialog.Close();
            });

            _soundService.PlayWarningSound();
            dialog.DataContext = vm;
            dialog.ShowDialog();

            return result;
        }

        /// <inheritdoc/>
        public ConfirmationResult RequestYesNoDecision(string message, string caption)
        {
            var owner = GetActiveWindow();
            var dialog = new YesNoDialog { Owner = owner };

            var result = ConfirmationResult.None;

            var vm = new YesNoDialogViewModel(message, caption, r =>
            {
                result = r switch
                {
                    true => ConfirmationResult.Yes,
                    false => ConfirmationResult.No,
                    _ => ConfirmationResult.No
                };
                dialog.Close();
            });

            _soundService.PlayWarningSound();
            dialog.DataContext = vm;
            dialog.ShowDialog();

            return result;
        }

        /// <inheritdoc/>
        public ConfirmationResponse ShowWarning(string message, string caption)
        {
            var owner = GetActiveWindow();
            var dialog = new WarningDialog { Owner = owner };
            var response = new ConfirmationResponse(ConfirmationResult.Cancel, false);

            var vm = new WarningDialogViewModel(message, caption, r =>
            {
                response = r;
                dialog.Close();
            });

            _soundService.PlayWarningSound();
            dialog.DataContext = vm;
            dialog.ShowDialog();

            return response;
        }

        private Window? GetActiveWindow()
        {
            return ApplicationEnt.Current?.Windows
                       .OfType<Window>()
                       .FirstOrDefault(w => w.IsActive)
                   ?? ApplicationEnt.Current?.MainWindow;
        }
    }
}
