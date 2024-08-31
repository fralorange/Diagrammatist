using DiagramApp.Presentation.WPF.ViewModels.Enums.Modes;
using System.Globalization;
using System.Windows.Data;

namespace DiagramApp.Presentation.WPF.Converters
{
    [ValueConversion(typeof(MouseMode), typeof(bool))]
    public class MouseModeIsPanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            MouseMode mode = (MouseMode)value;
            return mode == MouseMode.Pan;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
