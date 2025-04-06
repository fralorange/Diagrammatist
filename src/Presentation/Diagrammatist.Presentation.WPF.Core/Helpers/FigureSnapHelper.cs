using System.Windows;
using System.Windows.Media;

namespace Diagrammatist.Presentation.WPF.Core.Helpers
{
    /// <summary>
    /// A class that helps to get figure snap points.
    /// </summary>
    public static class FigureSnapHelper
    {
        private enum LineType { Horizontal, Vertical, Diagonal }

        /// <summary>
        /// Gets magnetic points.
        /// </summary>
        /// <param name="geometry">Geometry from which points are extracted.</param>
        /// <param name="pathWidth">Width to scale the geometry to.</param>
        /// <param name="pathHeight">Height to scale the geometry to.</param>
        /// <param name="keepAspectRatio">Whether to keep aspect ratio during scaling.</param>
        /// <returns><see cref="List{T}"/> of <see cref="Point"/> objects.</returns>
        public static List<Point> GetMagneticPoints(Geometry geometry, double pathWidth, double pathHeight, bool keepAspectRatio)
        {
            var points = new List<Point>();
            var flattened = geometry.GetOutlinedPathGeometry();
            Rect bounds = flattened.Bounds;

            // Calculate scale factors and offsets
            double scaleX = pathWidth / bounds.Width, scaleY = pathHeight / bounds.Height;
            double offsetX = 0, offsetY = 0;

            if (keepAspectRatio)
            {
                double uniformScale = Math.Min(scaleX, scaleY);
                scaleX = scaleY = uniformScale;
                offsetX = (pathWidth - bounds.Width * uniformScale) / 2;
                offsetY = (pathHeight - bounds.Height * uniformScale) / 2;
            }

            // Add snap points for each figure
            foreach (var figure in flattened.Figures)
            {
                if (figure.IsClosed)
                    AddFigureSnapPoints(figure, points, bounds);
            }

            // Remove duplicate points using tolerance
            double tolerance = 2.0;
            var filteredPoints = new List<Point>();
            foreach (var point in points)
            {
                var scaledPoint = new Point((point.X - bounds.X) * scaleX + offsetX, (point.Y - bounds.Y) * scaleY + offsetY);
                bool isDuplicate = filteredPoints.Any(p =>
                    Math.Abs(p.X - scaledPoint.X) < tolerance &&
                    Math.Abs(p.Y - scaledPoint.Y) < tolerance);

                if (!isDuplicate)
                    filteredPoints.Add(scaledPoint);
            }

            return filteredPoints;
        }

        /// <summary>
        /// Gets line type.
        /// </summary>
        /// <param name="a">Start point.</param>
        /// <param name="b">Next point.</param>
        /// <returns><see cref="LineType"/>.</returns>
        private static LineType GetLineType(Point a, Point b)
        {
            return a.Y == b.Y ? LineType.Horizontal : (a.X == b.X ? LineType.Vertical : LineType.Diagonal);
        }

        /// <summary>
        /// Adds group vertices
        /// </summary>
        /// <param name="start">Start point.</param>
        /// <param name="end">End point.</param>
        /// <param name="vertices">List of vertices.</param>
        private static void AddGroupVertices(Point start, Point end, List<Point> vertices)
        {
            if (start != end)
                vertices.Add(new Point((start.X + end.X) / 2, (start.Y + end.Y) / 2));
            vertices.Add(end);
        }

        /// <summary>
        /// Calculates a point on a cubic Bézier curve at the specified parameter position.
        /// </summary>
        /// <param name="t">Interpolation parameter between 0 and 1 (0 = start point, 1 = end point)</param>
        /// <param name="p0">Starting point of the Bézier curve</param>
        /// <param name="p1">First control point influencing the curve shape</param>
        /// <param name="p2">Second control point influencing the curve shape</param>
        /// <param name="p3">Ending point of the Bézier curve</param>
        /// <returns>Point coordinates at position t along the curve defined by p0-p3</returns>
        private static Point CalculateBezierPoint(double t, Point p0, Point p1, Point p2, Point p3)
        {
            double u = 1 - t, tt = t * t, uu = u * u, uuu = uu * u, ttt = tt * t;
            return new Point(
                uuu * p0.X + 3 * uu * t * p1.X + 3 * u * tt * p2.X + ttt * p3.X,
                uuu * p0.Y + 3 * uu * t * p1.Y + 3 * u * tt * p2.Y + ttt * p3.Y);
        }

        /// <summary>
        /// Adds snap points for a single figure.
        /// </summary>
        /// <param name="figure">The figure to process.</param>
        /// <param name="points">The list of points to which the snap points will be added.</param>
        private static void AddFigureSnapPoints(PathFigure figure, List<Point> points, Rect bounds)
        {
            var vertices = new List<Point> { figure.StartPoint };
            var current = figure.StartPoint;
            double centerX = bounds.X + bounds.Width / 2, centerY = bounds.Y + bounds.Height / 2;

            foreach (var segment in figure.Segments)
            {
                switch (segment)
                {
                    case LineSegment line:
                        AddLineSnapPoints(ref current, line.Point, vertices, centerX, centerY);
                        break;

                    case PolyLineSegment polyLine:
                        AddPolyLineSnapPoints(ref current, polyLine.Points, vertices, centerX, centerY);
                        break;

                    case BezierSegment bezier:
                        AddBezierSnapPoints(ref current, bezier, vertices);
                        break;

                    case PolyBezierSegment polyBezier:
                        AddPolyBezierSnapPoints(ref current, polyBezier.Points, vertices);
                        break;
                }
            }

            points.AddRange(vertices.GroupBy(p => new { X = Math.Round(p.X, 1), Y = Math.Round(p.Y, 1) })
                                   .Select(g => g.First()));
        }

        private static void AddLineSnapPoints(ref Point current, Point end, List<Point> vertices, double centerX, double centerY)
        {
            if (current.Y == end.Y && centerX >= Math.Min(current.X, end.X) && centerX <= Math.Max(current.X, end.X))
                vertices.Add(new Point(centerX, current.Y));
            else if (current.X == end.X && centerY >= Math.Min(current.Y, end.Y) && centerY <= Math.Max(current.Y, end.Y))
                vertices.Add(new Point(current.X, centerY));

            vertices.Add(new Point((current.X + end.X) / 2, (current.Y + end.Y) / 2));
            current = end;
            vertices.Add(current);
        }

        private static void AddPolyLineSnapPoints(ref Point current, PointCollection polyPoints, List<Point> vertices, double centerX, double centerY)
        {
            var groupStart = current;
            var groupEnd = polyPoints[0];
            var lineType = GetLineType(groupStart, groupEnd);

            for (int i = 1; i < polyPoints.Count; i++)
            {
                var nextPoint = polyPoints[i];
                var nextLineType = GetLineType(groupEnd, nextPoint);

                if (lineType != LineType.Diagonal && nextLineType == lineType)
                    groupEnd = nextPoint;
                else
                {
                    if (lineType == LineType.Horizontal && centerX >= Math.Min(groupStart.X, groupEnd.X) && centerX <= Math.Max(groupStart.X, groupEnd.X))
                        vertices.Add(new Point(centerX, groupStart.Y));
                    else if (lineType == LineType.Vertical && centerY >= Math.Min(groupStart.Y, groupEnd.Y) && centerY <= Math.Max(groupStart.Y, groupEnd.Y))
                        vertices.Add(new Point(groupStart.X, centerY));

                    AddGroupVertices(groupStart, groupEnd, vertices);
                    groupStart = groupEnd;
                    groupEnd = nextPoint;
                    lineType = GetLineType(groupStart, groupEnd);
                }
            }

            AddGroupVertices(groupStart, groupEnd, vertices);
            current = groupEnd;
        }

        private static void AddBezierSnapPoints(ref Point current, BezierSegment bezier, List<Point> vertices)
        {
            var mid = CalculateBezierPoint(0.5, current, bezier.Point1, bezier.Point2, bezier.Point3);
            vertices.Add(mid);
            vertices.Add(bezier.Point3);
            current = bezier.Point3;
        }

        private static void AddPolyBezierSnapPoints(ref Point current, PointCollection points, List<Point> vertices)
        {
            for (int i = 0; i < points.Count; i += 3)
            {
                if (i + 2 >= points.Count) break;
                var mid = CalculateBezierPoint(0.5, current, points[i], points[i + 1], points[i + 2]);
                vertices.Add(mid);
                vertices.Add(points[i + 2]);
                current = points[i + 2];
            }
        }
    }
}
