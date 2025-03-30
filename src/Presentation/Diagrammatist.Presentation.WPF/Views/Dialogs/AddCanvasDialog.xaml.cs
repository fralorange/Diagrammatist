using Diagrammatist.Presentation.WPF.Core.Helpers;
using System.Windows;
using System.Windows.Input;

namespace Diagrammatist.Presentation.WPF.Views.Dialogs
{
    /// <summary>
    /// A class that represents 'add canvas' dialog window and derives from <see cref="Window"/>.
    /// </summary>
    /// <remarks>
    /// This window used to create new canvases.
    /// </remarks>
    public partial class AddCanvasDialog : Window
    {
        public AddCanvasDialog()
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
