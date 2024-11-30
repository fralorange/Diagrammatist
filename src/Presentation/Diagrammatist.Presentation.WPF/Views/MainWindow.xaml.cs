using Diagrammatist.Presentation.WPF.ViewModels;
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

        private void CloseWindow()
        {
            Close();
        }
    }
}
