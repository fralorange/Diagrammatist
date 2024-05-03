using Microsoft.Maui.Controls.Shapes;
using System.Globalization;

namespace DiagramApp.Client.Converters
{
    public class PathDataToDataConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is string data)
            {
                return (Geometry)new PathGeometryConverter().ConvertFromInvariantString(data)!;
            }
            return null;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
