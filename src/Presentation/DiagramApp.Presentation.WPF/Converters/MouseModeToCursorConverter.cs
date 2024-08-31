using DiagramApp.Presentation.WPF.ViewModels.Enums.Modes;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Input;

namespace DiagramApp.Presentation.WPF.Converters
{
    [ValueConversion(typeof(MouseMode), typeof(Cursors))]
    public class MouseModeToCursorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            MouseMode mode = (MouseMode)value;
            return mode switch
            {
                MouseMode.Select => Cursors.Arrow,
                MouseMode.Pan => Cursors.SizeAll,
                _ => Cursors.Arrow,
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
