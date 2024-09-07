using DiagramApp.Presentation.WPF.ViewModels.Components.Enums.Modes;
using System.Globalization;
using System.Windows.Data;

namespace DiagramApp.Presentation.WPF.Framework.Converters
{
    [ValueConversion(typeof(MouseMode), typeof(bool))]
    public class MouseModeIsControlConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is MouseMode mode
                && parameter is string enumStr
                && Enum.TryParse(typeof(MouseMode), enumStr, out object? result)
                && result is MouseMode paramMode)
            {
                return mode == paramMode;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
