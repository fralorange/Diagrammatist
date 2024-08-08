using DiagramApp.Client.ViewModels.Wrappers;
using DiagramApp.Domain.Figures;
using Microsoft.Maui.Controls.Shapes;
using System.Globalization;
using PathFigure = DiagramApp.Domain.Figures.PathFigure;

namespace DiagramApp.Client.Converters
{
    public class FigureToValidPropertyConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value switch
            {
                PathFigure { PathData: { } data } pathFigure => (Geometry)new PathGeometryConverter().ConvertFromInvariantString(data)!,
                PolylineFigure { Points: { } points } polylineFigure => new PointCollection(points.Select(p => new Point(p.X, p.Y)).ToArray()),
                TextFigure { Text: { } text } textFigure => text,
                _ => value,
            };
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
