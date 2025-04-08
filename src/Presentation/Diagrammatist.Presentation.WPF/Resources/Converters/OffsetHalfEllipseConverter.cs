using System.Globalization;
using System.Windows.Data;

namespace Diagrammatist.Presentation.WPF.Resources.Converters
{
    /// <summary>
    /// A class that fixes ellipse offset.
    /// </summary>
    [ValueConversion(typeof(double), typeof(double))]
    public class OffsetHalfEllipseConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double coordinate)
            {
                double ellipseSize = 5;
                return coordinate - ellipseSize / 2;
            }
            return value;
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

}