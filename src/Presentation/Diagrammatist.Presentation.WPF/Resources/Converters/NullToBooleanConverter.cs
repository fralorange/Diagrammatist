using System.Globalization;
using System.Windows.Data;

namespace Diagrammatist.Presentation.WPF.Resources.Converters
{
    /// <summary>
    /// A class that converts from null to boolean.
    /// </summary>
    /// <remarks>
    /// Returns true if object is not null, otherwise false.
    /// </remarks>
    [ValueConversion(null, typeof(bool))]
    public class NullToBooleanConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is not null;
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
