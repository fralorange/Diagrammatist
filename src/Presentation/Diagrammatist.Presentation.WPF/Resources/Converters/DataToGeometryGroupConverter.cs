using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Diagrammatist.Presentation.WPF.Resources.Converters
{
    /// <summary>
    /// A class that converts from <see cref="List{T}"/> to <see cref="Geometry"/>.
    /// </summary>
    [ValueConversion(typeof(List<string>), typeof(Geometry))]
    public class DataToGeometryGroupConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is List<string> stringGeometryGroup)
            {
                var geometryGroup = new GeometryGroup();
                foreach (var item in stringGeometryGroup)
                {
                    var pathGeometry = Geometry.Parse(item);
                    geometryGroup.Children.Add(pathGeometry);
                }
                return geometryGroup;
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
