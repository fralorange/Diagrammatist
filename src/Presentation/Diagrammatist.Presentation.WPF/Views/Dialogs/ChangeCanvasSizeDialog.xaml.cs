using Diagrammatist.Presentation.WPF.Core.Helpers;
using System.Windows;
using System.Windows.Input;

namespace Diagrammatist.Presentation.WPF.Views.Dialogs
{
    /// <summary>
    /// A class that represents 'change canvas size' dialog windows and derives from <see cref="Window"/>.
    /// </summary>
    /// <remarks>
    /// This class used to change canvas size through changing width and height of the current canvas.
    /// </remarks>
    public partial class ChangeCanvasSizeDialog : Window
    {
        public ChangeCanvasSizeDialog()
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
