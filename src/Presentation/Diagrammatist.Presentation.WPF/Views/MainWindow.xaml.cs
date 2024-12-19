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
        public MainWindow(MainViewModel viewModel)
        {
            DataContext = viewModel;

            viewModel.OnRequestClose += CloseWindow;

            InitializeComponent();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            if (DataContext is MainViewModel viewModel && viewModel.HasGlobalChangesFlag)
            {
                // TO-DO: same issue, avoid using message boxes and other dialog creation classes
                // in code-behind, try to take it to the service in future.
                var result = MessageBox.Show(
                    "You have unsaved changes. Do you wish to save all and exit?",
                    "Confirm your actions",
                    MessageBoxButton.YesNoCancel,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    // might use bool return value and check if it actually saved if not, then e.Cancel = true
                    // can be achieved by using request messages, like request bool result of saving from canvas viewmodel and etc.
                    viewModel.MenuSaveAll();
                } else if (result == MessageBoxResult.Cancel)
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
