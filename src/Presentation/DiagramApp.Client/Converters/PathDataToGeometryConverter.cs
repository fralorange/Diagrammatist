using Microsoft.Maui.Controls.Shapes;
using System.Globalization;

namespace DiagramApp.Client.Converters
{
    public class PathDataToGeometryConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is string pathData)
                return (Geometry)new PathGeometryConverter().ConvertFromInvariantString(pathData)!;
            else 
                return value;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
