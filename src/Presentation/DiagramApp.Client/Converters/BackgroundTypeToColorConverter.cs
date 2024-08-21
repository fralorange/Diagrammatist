using DiagramApp.Domain.Settings;
using System.Globalization;

namespace DiagramApp.Client.Converters
{
    class BackgroundTypeToColorConverter : IValueConverter
    {
        private readonly ResourceDictionary _colors = App.Current!.Resources.MergedDictionaries
            .FirstOrDefault(dict => dict.Source.ToString().Contains("Colors.xaml"))!;

        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is BackgroundType backgroundType)
            {
                return backgroundType switch
                {
                    BackgroundType.Default => AppInfo.RequestedTheme.Equals(AppTheme.Light) ? Colors.White : _colors["Gray950"], 
                    BackgroundType.White => Colors.White,
                    BackgroundType.Black => _colors["Gray950"],
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
