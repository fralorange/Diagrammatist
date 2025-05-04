using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Diagrammatist.Presentation.WPF.Resources.Converters
{
    /// <summary>
    /// A multi value converter that check extracts figure model from either simulation node base or connection model.
    /// </summary>
    public class FigureModelExtractorMultiConverter : IMultiValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            foreach (var val in values)
            {
                if (val != DependencyProperty.UnsetValue && val != null)
                    return val;
            }

            return DependencyProperty.UnsetValue;
        }

        /// <inheritdoc/>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
