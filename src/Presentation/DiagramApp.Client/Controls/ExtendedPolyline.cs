using DiagramApp.Client.Platforms.Windows.Handlers;
using Microsoft.Maui.Controls.Shapes;

namespace DiagramApp.Client.Controls
{
    public class ExtendedPolyline : Shape, IShape
    {
        public ExtendedPolyline() : base()
        {
        }

        public ExtendedPolyline(PointCollection points) : this()
        {
            Points = points;
        }

        public static readonly BindableProperty PointsProperty =
            BindableProperty.Create(nameof(Points), typeof(PointCollection), typeof(ExtendedPolyline), null, defaultValueCreator: bindable => new PointCollection());

        public static readonly BindableProperty FillRuleProperty =
            BindableProperty.Create(nameof(FillRule), typeof(FillRule), typeof(ExtendedPolyline), FillRule.EvenOdd);

        public static readonly BindableProperty ShowArrowProperty =
            BindableProperty.Create(nameof(ShowArrow), typeof(bool), typeof(ExtendedPolyline), false, propertyChanged: OnShowArrowChanged);

        public PointCollection Points
        {
            set { SetValue(PointsProperty, value); }
            get { return (PointCollection)GetValue(PointsProperty); }
        }

        public FillRule FillRule
        {
            set { SetValue(FillRuleProperty, value); }
            get { return (FillRule)GetValue(FillRuleProperty); }
        }

        public bool ShowArrow
        {
            set { SetValue(ShowArrowProperty, value); }
            get { return (bool)GetValue(ShowArrowProperty); }
        }

        public override PathF GetPath()
        {
            var path = new PathF();

            if (Points?.Count > 0)
            {
                path.MoveTo((float)Points[0].X, (float)Points[0].Y);

                for (int index = 1; index < Points.Count; index++)
                {
                    path.LineTo((float)Points[index].X, (float)Points[index].Y);
                }

                if (ShowArrow && Points.Count > 1)
                {
                    AddArrowToEnd(path, Points[Points.Count - 2], Points[Points.Count - 1]);
                }
            }

            return path;
        }

        private void AddArrowToEnd(PathF path, Point start, Point end)
        {
            var arrowLength = 10;
            var arrowAngle = Math.PI / 6;

            var angle = Math.Atan2(end.Y - start.Y, end.X - start.X);

            var arrowPoint1 = new Point(
                end.X - arrowLength * Math.Cos(angle - arrowAngle),
                end.Y - arrowLength * Math.Sin(angle - arrowAngle));

            var arrowPoint2 = new Point(
                end.X - arrowLength * Math.Cos(angle + arrowAngle),
                end.Y - arrowLength * Math.Sin(angle + arrowAngle));

            path.MoveTo((float)end.X, (float)end.Y);
            path.LineTo((float)arrowPoint1.X, (float)arrowPoint1.Y);
            path.MoveTo((float)end.X, (float)end.Y);
            path.LineTo((float)arrowPoint2.X, (float)arrowPoint2.Y);
        }

        private static void OnShowArrowChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var arrowedPolyline = (ExtendedPolyline)bindable;
            arrowedPolyline.InvalidateMeasure();
        }
    }
}
