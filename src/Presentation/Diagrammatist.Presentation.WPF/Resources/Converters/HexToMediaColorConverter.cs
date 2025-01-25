using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Diagrammatist.Presentation.WPF.Resources.Converters
{
    /// <summary>
    /// A class that converts from hex to media color.
    /// </summary>
    [ValueConversion(typeof(string), typeof(Color))]
    public class HexToMediaColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string hex)
            {
                return ColorConverter.ConvertFromString(hex);
            }
            return Color.FromArgb(0, 255, 255, 255);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Color color)
            {
                return System.Drawing.ColorTranslator.ToHtml(System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B));
            }
            return "00FFFFFF";
        }
    }
}
