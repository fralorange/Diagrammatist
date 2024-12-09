using System.Windows;

namespace Diagrammatist.Presentation.WPF.Framework.Controls.Args
{
    /// <summary>
    /// A class that derives from <see cref="EventArgs"/> and represents data for an event that occurs when an object position changes.
    /// </summary>
    public class PositionChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets a data context of a visual.
        /// </summary>
        public object? DataContext { get; }

        /// <summary>
        /// Gets an old position of an object.
        /// </summary>
        public Point OldPos { get; }

        /// <summary>
        /// Gets a new position of an object.
        /// </summary>
        public Point NewPos { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="PositionChangedEventArgs"/> class with the specified do and undo actions.
        /// </summary>
        public PositionChangedEventArgs(object? dataContext, double oldX, double oldY, double newX, double newY)
        {
            DataContext = dataContext;
            OldPos = new(oldX, oldY);
            NewPos = new(newX, newY);
        }
    }
}
