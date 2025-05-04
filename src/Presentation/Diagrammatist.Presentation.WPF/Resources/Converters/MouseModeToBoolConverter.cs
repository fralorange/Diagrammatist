using Diagrammatist.Presentation.WPF.Core.Shared.Enums;
using System.Globalization;
using System.Windows.Data;

namespace Diagrammatist.Presentation.WPF.Resources.Converters
{
    /// <summary>
    /// A class that converts from <see cref="MouseMode"/> to <see cref="bool"/>.
    /// </summary>
    [ValueConversion(typeof(MouseMode), typeof(bool))]
    public class MouseModeToBoolConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is MouseMode mode && mode is MouseMode.Line)
            {
                return true;
            }
            return false;
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
