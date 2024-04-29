using System.Globalization;

namespace DiagramApp.Client.Converters
{
    public class BoolToStrokeDashArrayConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool isDashed && isDashed)
            {
                return new DoubleCollection([4, 2]);
            }
            return new DoubleCollection([1,0]);
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
