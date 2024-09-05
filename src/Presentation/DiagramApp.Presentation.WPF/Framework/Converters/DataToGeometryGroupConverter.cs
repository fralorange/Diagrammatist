using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace DiagramApp.Presentation.WPF.Framework.Converters
{
    [ValueConversion(typeof(List<string>), typeof(Geometry))]
    public class DataToGeometryGroupConverter : IValueConverter
    {
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

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
