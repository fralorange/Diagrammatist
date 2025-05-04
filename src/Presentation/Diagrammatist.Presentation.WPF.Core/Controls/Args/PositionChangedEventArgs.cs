using System.Windows;

namespace Diagrammatist.Presentation.WPF.Core.Controls.Args
{
    /// <summary>
    /// A class that derives from <see cref="PositionChangingEventArgs"/> 
    /// and represents data for an event that occurs when an object position changes.
    /// </summary>
    public class PositionChangedEventArgs : PositionChangingEventArgs
    {
        /// <summary>
        /// Gets or sets initial position of an object.
        /// </summary>
        public Point InitialPos { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="PositionChangedEventArgs"/> class with the specified do and undo actions.
        /// </summary>
        /// <param name="dataContext"></param>
        /// <param name="oldX"></param>
        /// <param name="oldY"></param>
        /// <param name="newX"></param>
        /// <param name="newY"></param>
        public PositionChangedEventArgs(object? dataContext, double oldX, double oldY, double newX, double newY, double initX, double initY) 
            : base(dataContext, oldX, oldY, newX, newY)
        {
            InitialPos = new(initX, initY);
        }
    }
}
