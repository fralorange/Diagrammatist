using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Diagrammatist.Presentation.WPF.Core.Converters
{
    /// <summary>
    /// A class that converts from <see cref="List{T}"/> of <see cref="System.Drawing.Point"/> to <see cref="PointCollection"/> class.
    /// </summary>
    [ValueConversion(typeof(List<System.Drawing.Point>), typeof(PointCollection))]
    public class DataToPointCollectionConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is List<System.Drawing.Point> points)
            {
                return new PointCollection(points.Select(p => new Point(p.X, p.Y)));
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
