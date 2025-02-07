using Diagrammatist.Presentation.WPF.Core.Helpers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using ApplicationEnt = System.Windows.Application;

namespace Diagrammatist.Presentation.WPF.Core.Renderers.Line
{
    /// <summary>
    /// A class that draws a visual (non-persistent) line.
    /// </summary>
    public class LineDrawer
    {
        private Panel _panel;
        private int _gridStep;

        private Polyline? _line;

        private bool _isShiftPressed;
        private bool _isDrawing;

        /// <summary>
        /// Gets or sets 'is drawing' parameter.
        /// </summary>
        /// <remarks>
        /// This property used to determine whether line in is drawing mode or not.
        /// </remarks>
        public bool IsDrawing
        {
            get => _isDrawing;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LineDrawer"/> class.
        /// </summary>
        /// <param name="panel">The panel where drawing will take place.</param>
        public LineDrawer(Panel panel, int gridStep)
        {
            _panel = panel;
            _gridStep = gridStep;
        }

        /// <summary>
        /// Adds new point to line.
        /// </summary>
        /// <param name="newPoint">A new line point.</param>
        public void AddPoint(Point newPoint)
        {
            if (_line is null)
            {
                _line = new Polyline
                {
                    Style = (Style)ApplicationEnt.Current.Resources["Line"],
                    Stretch = Stretch.None,
                    StrokeLineJoin = PenLineJoin.Bevel,
                };

                _isDrawing = true;
                _panel.Children.Insert(0, _line);
            }

            newPoint = GetSnappedToGridPoint(newPoint);

            _line.Points.Add(newPoint);
        }

        /// <summary>
        /// Updates current line latest point.
        /// </summary>
        /// <param name="currentPoint">Current point that latest point updates to.</param>
        /// <param name="isShiftPressed">Shift pressed parameter.</param>
        public void UpdateLine(Point currentPoint, bool isShiftPressed)
        {
            if (_line is null)
                return;

            _isShiftPressed = isShiftPressed;

            if (_isShiftPressed)
            {
                currentPoint = GetSnappedToAnglePoint(currentPoint);
            }

            currentPoint = GetSnappedToGridPoint(currentPoint);

            if (_line.Points.Count == 1)
            {
                _line.Points.Add(currentPoint);
            }
            else
            {
                _line.Points[^1] = currentPoint;
            }
        }

        /// <summary>
        /// Ends drawing operation.
        /// </summary>
        /// <returns></returns>
        public (List<Point> Points, Point Position)? EndDrawing()
        {
            if (_line is null || _line.Points.Count < 3)
                return null;

            try
            {
                var newList = new List<Point>(_line.Points.SkipLast(1));
                var position = GetPolylinePosition();

                return (newList, position);
            }
            finally
            {
                _isDrawing = false;
                _line.Points.Clear();
                _panel.Children.Remove(_line);
                _line = null;
            }
        }

        private Point GetSnappedToAnglePoint(Point point)
        {
            if (_line is null || _line.Points.Count < 2)
                return point;

            var previousPoint = _line.Points[^2];

            var deltaX = point.X - previousPoint.X;
            var deltaY = point.Y - previousPoint.Y;

            if (deltaX == 0 && deltaY == 0)
                return point;

            var angle = Math.Atan2(deltaY, deltaX);
            var snapAngle = Math.Round(angle / (Math.PI / 4)) * (Math.PI / 4);

            var distance = Math.Sqrt(deltaX * deltaX + deltaY * deltaY);

            var snappedX = previousPoint.X + Math.Cos(snapAngle) * distance;
            var snappedY = previousPoint.Y + Math.Sin(snapAngle) * distance;

            return new Point(snappedX, snappedY);
        }

        private Point GetSnappedToGridPoint(Point point)
        {
            var snappedX = point.X;
            var snappedY = point.Y;

            GridHelper.SnapCoordinatesToGrid(new Thickness(0), ref snappedX, ref snappedY, _gridStep);

            return new Point(snappedX, snappedY);
        }

        private Point GetPolylinePosition()
        {
            if (_line is null || _line.Points.Count == 0)
                return new Point(0, 0);

            var points = _line.Points.SkipLast(1);

            double minX = points.Min(p => p.X);
            double minY = points.Min(p => p.Y);

            return new(minX, minY);
        }
    }
}
