using System.Windows.Data;
using System.Windows;
using System.Globalization;

namespace Diagrammatist.Presentation.WPF.Resources.Converters
{
    /// <summary>
    /// A value converter that converts an enumeration to a visibility state.
    /// </summary>
    [ValueConversion(typeof(Enum), typeof(Visibility))]
    public class EnumToVisibilityConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter is string paramStr && value is Enum enumValue)
            {
                if (enumValue.ToString() == paramStr)
                {
                    return Visibility.Visible;
                }
            }
            return Visibility.Collapsed;
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
