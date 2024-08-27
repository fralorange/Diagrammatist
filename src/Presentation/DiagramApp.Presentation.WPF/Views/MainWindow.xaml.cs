using DiagramApp.Presentation.WPF.ViewModels;
using System.Windows;

namespace DiagramApp.Presentation.WPF.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow(MainViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }
    }
}
