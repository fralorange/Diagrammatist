using Diagrammatist.Presentation.WPF.Core.Helpers;
using Diagrammatist.Presentation.WPF.Core.Models.Figures.Magnetic;
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
        /// <summary>
        /// Occurs when logic requires early exit from drawing.
        /// </summary>
        public event Action? RequestEarlyExit;

        private Panel _panel;
        private int _gridStep;

        private Polyline? _line;

        private bool _isShiftPressed;
        private bool _isCtrlPressed;
        private bool _isDrawing;

        private MagneticPointModel? _startMagneticPoint;
        private MagneticPointModel? _endMagneticPoint;

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
        /// <param name="gridStep">A grid step.</param>
        public LineDrawer(Panel panel, int gridStep)
        {
            _panel = panel;
            _gridStep = gridStep;
        }

        /// <summary>
        /// Adds new point to line.
        /// </summary>
        /// <param name="newPoint">A new line point.</param>
        public void AddPoint(Point newPoint, IEnumerable<MagneticPointModel> magneticPoints)
        {
            var magneticPoint = GetNearestMagneticPoint(newPoint, magneticPoints);

            if (_line is null)
            {
                _line = new Polyline
                {
                    Style = (Style)ApplicationEnt.Current.Resources["Line"],
                    Stretch = Stretch.None,
                    StrokeLineJoin = PenLineJoin.Bevel,
                };

                _startMagneticPoint = magneticPoint;
                _isDrawing = true;
                _panel.Children.Insert(0, _line);
            }

            if (!_isCtrlPressed)
            {
                newPoint = GetSnappedToGridPoint(newPoint);
            }

            newPoint = magneticPoint?.Position ?? newPoint;

            _line.Points.Add(newPoint);

            if (_line.Points.Count > 1 && magneticPoint is not null && RequestEarlyExit is not null)
            {
                _endMagneticPoint = magneticPoint;

                RequestEarlyExit();
            }
        }

        /// <summary>
        /// Updates current line latest point.
        /// </summary>
        /// <param name="currentPoint">Current point that latest point updates to.</param>
        /// <param name="isShiftPressed">Shift pressed parameter.</param>
        /// <param name="isCtrlPressed">Ctrl pressed parameter.</param>
        /// <param name="magneticPoints">Magnetic points.</param>
        public void UpdateLine(Point currentPoint, bool isShiftPressed, bool isCtrlPressed, IEnumerable<MagneticPointModel> magneticPoints)
        {
            if (_line is null)
                return;

            _isShiftPressed = isShiftPressed;
            _isCtrlPressed = isCtrlPressed;

            if (_isShiftPressed)
            {
                currentPoint = GetSnappedToAnglePoint(currentPoint);
            }

            if (!_isCtrlPressed)
            {
                currentPoint = GetSnappedToGridPoint(currentPoint);
            }

            var magneticPoint = GetNearestMagneticPoint(currentPoint, magneticPoints);
            currentPoint = magneticPoint?.Position ?? currentPoint;

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
        public (List<Point> Points, MagneticPointModel? Start, MagneticPointModel? End)? EndDrawing()
        {
            try
            {
                if (_line is null || _line.Points.Count < 3) return null;

                var newList = new List<Point>(_line.Points.SkipLast(1));

                return (newList, _startMagneticPoint, _endMagneticPoint);
            }
            finally
            {
                _startMagneticPoint = null;
                _endMagneticPoint = null;
                _isDrawing = false;
                _line?.Points.Clear();
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

        private MagneticPointModel? GetNearestMagneticPoint(Point point, IEnumerable<MagneticPointModel> magneticPoints, double threshold = 10)
        {
            var nearestPoint = magneticPoints
                .Select(p => new { Point = p, Distance = GetDistance(point, p.Position) })
                .Where(p => p.Distance <= threshold)
                .OrderBy(p => p.Distance)
                .FirstOrDefault();

            return nearestPoint?.Point;
        }

        private double GetDistance(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
        }
    }
}
