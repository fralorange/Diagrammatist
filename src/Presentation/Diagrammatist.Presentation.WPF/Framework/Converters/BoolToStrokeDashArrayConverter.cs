using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace DiagramApp.Presentation.WPF.Framework.Converters
{
    /// <summary>
    /// A class that converts from <see cref="bool"/> too <see cref="DoubleCollection"/>.
    /// </summary>
    [ValueConversion(typeof(bool), typeof(DoubleCollection))]
    public class BoolToStrokeDashArrayConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool isDashed && isDashed)
            {
                return new DoubleCollection([4, 2]);
            }
            return new DoubleCollection([1, 0]);
        }

        /// <inheritdoc/>
        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
