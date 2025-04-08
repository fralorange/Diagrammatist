using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Diagrammatist.Presentation.WPF.Resources.Converters
{
    /// <summary>
    /// A class that converts from <see cref="IEnumerable{T}{T}"/> of <see cref="Point"/> to <see cref="PointCollection"/> class.
    /// </summary>
    [ValueConversion(typeof(IEnumerable<Point>), typeof(PointCollection))]
    public class DataToPointCollectionConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IEnumerable<Point> points)
            {
                return new PointCollection(points);
            }
            return value;
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
