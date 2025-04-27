using Diagrammatist.Presentation.WPF.Core.Helpers;
using Diagrammatist.Presentation.WPF.Simulator.ViewModels;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ApplicationEnt = System.Windows.Application;

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

        private void SimulatorLoaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is SimulatorWindowViewModel vm)
            {
                vm.RequestOpen += OpenFile;
            }
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

        private string OpenFile()
        {
            var lua = LocalizationHelper.GetLocalizedValue<string>("SimulatorResources", "Lua");
            var filter = $"{ApplicationEnt.Current.Resources["Filter"]}|*.{ApplicationEnt.Current.Resources["Extension"]}|" +
                $"{lua}|*.lua";

            OpenFileDialog openFileDialog = new()
            {
                Filter = filter
            };

            if (openFileDialog.ShowDialog() == true)
            {
                return openFileDialog.FileName;
            }

            return string.Empty;
        }
    }
}
