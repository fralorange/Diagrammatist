using Diagrammatist.Presentation.WPF.ViewModels.Components.Enums.Modes;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Diagrammatist.Presentation.WPF.Resources.Converters
{
    /// <summary>
    /// A class that converts from <see cref="MouseMode"/> to <see cref="Visibility"/>.
    /// </summary>
    [ValueConversion(typeof(MouseMode), typeof(Visibility))]
    public class MouseModeToVisibilityConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is MouseMode mode && mode is MouseMode.Line)
            {
                return Visibility.Visible;
            }
            return Visibility.Collapsed;
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
