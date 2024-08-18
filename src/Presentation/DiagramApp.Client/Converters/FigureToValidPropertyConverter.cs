using DiagramApp.Client.ViewModels.Wrappers;
using DiagramApp.Domain.Figures;
using Microsoft.Maui.Controls.Shapes;
using System.Globalization;
using ShapeFigure = DiagramApp.Domain.Figures.ShapeFigure;

namespace DiagramApp.Client.Converters
{
    public class FigureToValidPropertyConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value switch
            {
                ShapeFigure { Data: { } data } pathFigure => (Geometry)new PathGeometryConverter().ConvertFromInvariantString(data)!,
                LineFigure { Points: { } points } polylineFigure => new PointCollection(points.Select(p => new Point(p.X, p.Y)).ToArray()),
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
