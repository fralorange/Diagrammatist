using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Diagrammatist.Presentation.WPF.Resources.Converters
{
    /// <summary>
    /// A class that converts from <see cref="bool"/> to <see cref="Visibility"/>.
    /// </summary>
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class BoolToVisibilityConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool param)
            {
                return (param) ? Visibility.Visible : Visibility.Collapsed;
            }
            return Visibility.Visible;
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
