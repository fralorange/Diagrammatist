using System.Globalization;
using System.Windows.Data;

namespace DiagramApp.Presentation.WPF.Converters
{
    [ValueConversion(typeof(Enum), typeof(IEnumerable<string>))]
    public class EnumToCollectionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Enum)
            {
                return Enum.GetValues(value.GetType());
            }
            return Array.Empty<string>();
        }

        public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
