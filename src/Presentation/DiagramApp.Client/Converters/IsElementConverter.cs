using System.Globalization;

namespace DiagramApp.Client.Converters
{
    public class IsElementConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is null || parameter is null or not Type) return false;

            var valueType = value.GetType();
            var parameterType = parameter as Type;

            return parameterType!.IsAssignableFrom(valueType);
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
