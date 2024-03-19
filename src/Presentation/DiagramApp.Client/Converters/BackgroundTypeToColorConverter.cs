using DiagramApp.Domain.DiagramSettings;
using System.Globalization;

namespace DiagramApp.Client.Converters
{
    class BackgroundTypeToColorConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is BackgroundType backgroundType)
            {
                return backgroundType switch
                {
                    BackgroundType.White => Colors.White,
                    BackgroundType.Black => Colors.Black,
                    BackgroundType.Transparent => Colors.Transparent,
                    _ => throw new ArgumentOutOfRangeException(nameof(value)),
                };
            }

            return Colors.Transparent;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
