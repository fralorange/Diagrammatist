using System.Globalization;

namespace DiagramApp.Client.Converters
{
    public class BoolToAspectRatioConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool aspect)
            {
                return (aspect) ? Stretch.Uniform : Stretch.Fill;
            }
            return null;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
