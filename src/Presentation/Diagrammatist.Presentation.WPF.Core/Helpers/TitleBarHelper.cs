using System.Windows;
using System.Windows.Input;

namespace Diagrammatist.Presentation.WPF.Core.Helpers
{
    /// <summary>
    /// A static class that helps to perform operations to setup custom titlebar.
    /// </summary>
    public static class TitleBarHelper
    {
        /// <summary>
        /// Closes window.
        /// </summary>
        public static void CloseWindow(Window window)
        {
            SystemCommands.CloseWindow(window);
        }

        /// <summary>
        /// Processes double click operations.
        /// </summary>
        /// <param name="window">Current window.</param>
        /// <param name="e">Mouse button event args.</param>
        public static void DoubleClickProcess(Window window, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2 && e.ChangedButton == MouseButton.Left)
            {
                window.Close();
            }
            else if (e.ChangedButton == MouseButton.Left || e.ChangedButton == MouseButton.Right)
            {
                ShowSystemMenu(window, e.GetPosition(window));
            }
        }

        /// <summary>
        /// Shows system menu.
        /// </summary>
        /// <param name="window">Current window.</param>
        /// <param name="point">System menu position.</param>
        public static void ShowSystemMenu(Window window, Point point)
        {
            // Increment coordinates to allow double-click
            ++point.X;
            ++point.Y;
            if (window.WindowState == WindowState.Normal)
            {
                point.X += window.Left;
                point.Y += window.Top;
            }
            SystemCommands.ShowSystemMenu(window, point);
        }
    }
}
