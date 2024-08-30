using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace DiagramApp.Presentation.WPF.Converters
{
    [ValueConversion(typeof(System.Drawing.Color), typeof(Brush))]
    public class ColorToBrushConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is  System.Drawing.Color color)
            {
                return new SolidColorBrush(Color.FromArgb(color.A, color.R, color.G, color.B));
            }
            return Brushes.DimGray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
