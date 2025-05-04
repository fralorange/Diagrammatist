using System.Globalization;
using System.Windows.Data;

namespace Diagrammatist.Presentation.WPF.Resources.Converters
{
    /// <summary>
    /// A class that implements <see cref="IMultiValueConverter"/>.
    /// This class compares two objects and returns a boolean value.
    /// </summary>
    public class ObjectComparisonMultiConverter : IMultiValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length != 2 || values[0] == null || values[1] == null)
                return false;
            return values[0].Equals(values[1]);
        }

        /// <inheritdoc/>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
