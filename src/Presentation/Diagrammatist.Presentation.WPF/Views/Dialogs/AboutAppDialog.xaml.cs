using System.Windows;
using System.Windows.Input;

namespace Diagrammatist.Presentation.WPF.Views.Dialogs
{
    /// <summary>
    /// A class that represents 'about' dialog window and derives from <see cref="Window"/>.
    /// </summary>
    /// <remarks>
    /// This window shows information about the app.
    /// </remarks>
    public partial class AboutAppDialog : Window
    {
        public AboutAppDialog()
        {
            InitializeComponent();
        }

        private void OnCloseButtonClick(object sender, RoutedEventArgs e)
        {
            SystemCommands.CloseWindow(this);
        }

        private void OnIconMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2 && e.ChangedButton == MouseButton.Left)
            {
                Close();
            }
            else if (e.ChangedButton == MouseButton.Left || e.ChangedButton == MouseButton.Right)
            {
                ShowSystemMenu(e.GetPosition(this));
            }
        }

        public void ShowSystemMenu(Point point)
        {
            // Increment coordinates to allow double-click
            ++point.X;
            ++point.Y;
            if (WindowState == WindowState.Normal)
            {
                point.X += Left;
                point.Y += Top;
            }
            SystemCommands.ShowSystemMenu(this, point);
        }
    }
}
