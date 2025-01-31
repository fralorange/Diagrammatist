using Diagrammatist.Presentation.WPF.Core.Foundation.Base.ObservableObject.Args;
using Diagrammatist.Presentation.WPF.Core.Foundation.Base.ObservableObject.Handler;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Diagrammatist.Presentation.WPF.Core.Foundation.Base.ObservableObject
{
    /// <summary>
    /// An extended class for objects of which the properties must be observable that derives from <see cref="ObservableObject"/>.
    /// </summary>
    public class ExtendedObservableObject : CommunityToolkit.Mvvm.ComponentModel.ObservableObject
    {
        /// <summary>
        /// Raised when a property changes, with old and new values.
        /// </summary>
        public event ExtendedPropertyChangedEventHandler? ExtendedPropertyChanged;

        /// <summary>
        /// Raises the standard <see cref="INotifyPropertyChanged.PropertyChanged"/> event and the extended event.
        /// </summary>
        /// <param name="propertyName">(optional) The name of the property that changed.</param>
        /// <param name="oldValue">The old value of the property.</param>
        /// <param name="newValue">The new value of the property.</param>
        protected void OnPropertyChanged(object? oldValue, object? newValue, [CallerMemberName] string? propertyName = null)
        {
            // Invoke the extended event
            ExtendedPropertyChanged?.Invoke(this, new ExtendedPropertyChangedEventArgs(propertyName, oldValue, newValue));

            // Invoke the standard PropertyChanged event for WPF binding
            OnPropertyChanged(propertyName);
        }

        /// <summary>
        /// Updates the property and raises both standard and extended events.
        /// </summary>
        /// <typeparam name="T">The type of the property that changed.</typeparam>
        /// <param name="field">The field storing the property's value.</param>
        /// <param name="newValue">The property's value after the change occurred.</param>
        /// <param name="propertyName">(optional) The name of the property that changed.</param>
        /// <returns><see langword="true"/> if the property was changed, <see langword="false"/> otherwise.</returns>
        protected new bool SetProperty<T>([NotNullIfNotNull(nameof(newValue))] ref T field, T newValue, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, newValue))
            {
                return false;
            }

            OnPropertyChanging(propertyName);

            var oldValue = field;
            field = newValue;

            OnPropertyChanged(oldValue, newValue, propertyName);

            return true;
        }
    }
}
