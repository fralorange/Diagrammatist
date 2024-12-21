using System.ComponentModel;

namespace Diagrammatist.Presentation.WPF.Core.Foundation.BaseClasses.ObservableObject.Args
{
    /// <summary>
    /// An extended class that derives from <see cref="PropertyChangedEventArgs"/> and adds tracking of old and new values.
    /// </summary>
    public class ExtendedPropertyChangedEventArgs : PropertyChangedEventArgs
    {
        /// <summary>
        /// Old value of property.
        /// </summary>
        public object? OldValue { get; set; }
        /// <summary>
        /// New value of property.
        /// </summary>
        public object? NewValue { get; set; }

        /// <summary>
        /// Initializes new data for <see cref="INotifyPropertyChanged.PropertyChanged"/> event with extended functionality.
        /// </summary>
        public ExtendedPropertyChangedEventArgs(string? propertyName, object? oldValue, object? newValue)
            : base(propertyName)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}
