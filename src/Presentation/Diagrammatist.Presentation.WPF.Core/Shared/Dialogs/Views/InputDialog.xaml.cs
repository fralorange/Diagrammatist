using Diagrammatist.Presentation.WPF.Core.Helpers;
using System.Windows;
using System.Windows.Input;

namespace Diagrammatist.Presentation.WPF.Core.Shared.Dialogs.Views
{
    /// <summary>
    /// A class that represents input dialog window.
    /// </summary>
    public partial class InputDialog : Window
    {
        public InputDialog()
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
    }
}
