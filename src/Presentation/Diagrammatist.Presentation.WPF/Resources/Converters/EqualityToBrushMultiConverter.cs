using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Diagrammatist.Presentation.WPF.Resources.Converters
{
    /// <summary>
    /// A multi value converter that check for equality and then converts into brushes.
    /// </summary>
    public class EqualityToBrushMultiConverter : IMultiValueConverter
    {
        /// <summary>
        /// Gets or sets highlight brush.
        /// </summary>
        public Brush HighlightBrush { get; set; } = Brushes.Yellow;

        /// <summary>
        /// Gets or sets default brush.
        /// </summary>
        public Brush DefaultBrush { get; set; } = Brushes.Transparent;

        /// <inheritdoc/>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return values.Length >= 2 && values[0] == values[1]
                ? HighlightBrush
                : DefaultBrush;
        }

        /// <inheritdoc/>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
