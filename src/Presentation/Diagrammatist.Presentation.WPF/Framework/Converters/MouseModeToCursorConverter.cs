using DiagramApp.Presentation.WPF.ViewModels.Components.Enums.Modes;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Input;

namespace DiagramApp.Presentation.WPF.Framework.Converters
{
    /// <summary>
    /// A class that converts from <see cref="MouseMode"/> enum to <see cref="Cursors"/> class.
    /// </summary>
    [ValueConversion(typeof(MouseMode), typeof(Cursors))]
    public class MouseModeToCursorConverter : IValueConverter
    {
        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
