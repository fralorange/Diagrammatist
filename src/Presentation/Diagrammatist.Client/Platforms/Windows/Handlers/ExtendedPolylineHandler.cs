using DiagramApp.Client.Controls;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;

namespace DiagramApp.Client.Platforms.Windows.Handlers
{
    public partial class ExtendedPolylineHandler : ShapeViewHandler
    {
        public static new IPropertyMapper<ExtendedPolyline, ExtendedPolylineHandler> Mapper = new PropertyMapper<ExtendedPolyline, ExtendedPolylineHandler>(ShapeViewHandler.Mapper)
        {
            [nameof(IShapeView.Shape)] = MapShape,
            [nameof(ExtendedPolyline.Points)] = MapPoints,
            [nameof(ExtendedPolyline.FillRule)] = MapFillRule,
            [nameof(ExtendedPolyline.ShowArrow)] = MapShowArrow,
        };

        public ExtendedPolylineHandler() : base(Mapper)
        {

        }

        public ExtendedPolylineHandler(IPropertyMapper mapper) : base(mapper ?? Mapper)
        {

        }

        private static void MapShowArrow(ExtendedPolylineHandler handler, ExtendedPolyline polyline)
        {
            handler.PlatformView?.InvalidateShape(polyline);
        }

        private static void MapShape(ExtendedPolylineHandler handler, ExtendedPolyline polyline)
        {
            handler.PlatformView?.UpdateShape(polyline);
        }

        private static void MapPoints(ExtendedPolylineHandler handler, ExtendedPolyline polyline)
        {
            handler.PlatformView?.InvalidateShape(polyline);
        }

        public static void MapFillRule(ExtendedPolylineHandler handler, ExtendedPolyline polyline)
        {
            IDrawable? drawable = handler.PlatformView?.Drawable;

            if (drawable == null)
                return;

            if (drawable is ShapeDrawable shapeDrawable)
                shapeDrawable.UpdateWindingMode(polyline.FillRule == FillRule.EvenOdd ? WindingMode.EvenOdd : WindingMode.NonZero);

            handler.PlatformView?.InvalidateShape(polyline);
        }
    }
}
