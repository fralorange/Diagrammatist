using System.Globalization;

namespace DiagramApp.Client.Converters
{
    public class NullToScrollBarVisibilityConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value == null ? ScrollBarVisibility.Never : ScrollBarVisibility.Always;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
