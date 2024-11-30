using System.Collections.ObjectModel;
using System.Globalization;

namespace DiagramApp.Client.Converters
{
    public class PointsToPointCollectionConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is ObservableCollection<System.Drawing.Point> points)
            {
                return new PointCollection(points.Select(p => new Point(p.X, p.Y)).ToArray());
            }
            return null;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
