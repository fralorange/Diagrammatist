using System.Drawing;
using System.Globalization;
using System.Windows.Data;
using MediaColor = System.Windows.Media.Color;

namespace Diagrammatist.Presentation.WPF.Resources.Converters
{
    /// <summary>
    /// A value converter that converts from color to colorstate two way.
    /// </summary>
    [ValueConversion(typeof(Color), typeof(MediaColor))]
    public class ColorToMediaColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Color color)
            {
                return MediaColor.FromArgb(color.A, color.R, color.G, color.B);
            }
            return MediaColor.FromRgb(255, 255, 255);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is MediaColor mediaColor)
            {
                return Color.FromArgb(mediaColor.A, mediaColor.R, mediaColor.G, mediaColor.B);
            }
            return Color.FromArgb(1, 255, 255, 255);
        }
    }
}
