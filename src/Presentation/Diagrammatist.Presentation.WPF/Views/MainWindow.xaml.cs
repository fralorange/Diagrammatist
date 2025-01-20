using Diagrammatist.Presentation.WPF.Core.Services.Alert;
using Diagrammatist.Presentation.WPF.ViewModels;
using System.ComponentModel;
using System.Windows;

namespace Diagrammatist.Presentation.WPF.Views
{
    /// <summary>
    /// A class that represents main window and derives from <see cref="Window"/>.
    /// </summary>
    /// <remarks>
    /// This class can be considered as the whole app that builds from different components from <see cref="Components"/> namespace as the puzzle.
    /// </remarks>
    public partial class MainWindow : Window
    {
        private readonly IAlertService _alertService;

        public MainWindow(MainViewModel viewModel, IAlertService alertService)
        {
            DataContext = viewModel;
            _alertService = alertService;

            viewModel.OnRequestClose += CloseWindow;

            InitializeComponent();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            if (DataContext is MainViewModel viewModel && viewModel.HasGlobalChangesFlag)
            {
                var result = _alertService.RequestConfirmation("You have unsaved changes. Do you wish to save all and exit?",
                     "Confirm your actions");

                if (result == MessageBoxResult.Yes && !viewModel.SaveAll() || result == MessageBoxResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
        }

        private void CloseWindow()
        {
            Close();
        }
    }
}
