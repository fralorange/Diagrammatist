using Diagrammatist.Presentation.WPF.Core.Helpers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Diagrammatist.Presentation.WPF.Simulator.Views
{
    /// <summary>
    /// A class that derived from <see cref="Window"/>. This class defines simulator view.
    /// </summary>
    public partial class SimulatorWindow : Window
    {
        public SimulatorWindow()
        {
            InitializeComponent();
        }

        private void OnCloseButtonClick(object sender, RoutedEventArgs e)
        {
            TitleBarHelper.CloseWindow(this);
        }

        private void OnIconMouseDown(object sender, MouseButtonEventArgs e)
        {
            TitleBarHelper.DoubleClickProcess(this, e);
        }

        private void OnListBoxPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.Source is ListBox listBox)
            {
                FocusHelper.ClearFocusAndSelection(listBox);
            }
        }
    }
}
