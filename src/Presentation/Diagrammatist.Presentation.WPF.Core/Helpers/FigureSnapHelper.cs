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

            double scaleX = pathWidth / bounds.Width;
            double scaleY = pathHeight / bounds.Height;
            double offsetX = 0;
            double offsetY = 0;

            if (keepAspectRatio)
            {
                double uniformScale = Math.Min(scaleX, scaleY);
                scaleX = scaleY = uniformScale;

                double scaledWidth = bounds.Width * uniformScale;
                double scaledHeight = bounds.Height * uniformScale;
                offsetX = (pathWidth - scaledWidth) / 2;
                offsetY = (pathHeight - scaledHeight) / 2;
            }

            foreach (var figure in flattened.Figures)
            {
                if (figure.IsClosed)
                {
                    AddFigureSnapPoints(figure, points);
                }
            }

            var filteredPoints = new List<Point>();
            double tolerance = 1.0;

            foreach (var point in points)
            {
                var scaledPoint = new Point(
                    (point.X - bounds.X) * scaleX + offsetX,
                    (point.Y - bounds.Y) * scaleY + offsetY);

                bool isDuplicate = filteredPoints.Any(p =>
                    Math.Abs(p.X - scaledPoint.X) < tolerance &&
                    Math.Abs(p.Y - scaledPoint.Y) < tolerance);

                if (!isDuplicate)
                {
                    filteredPoints.Add(scaledPoint);
                }
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
            if (a.Y == b.Y) return LineType.Horizontal;
            if (a.X == b.X) return LineType.Vertical;
            return LineType.Diagonal;
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
            {
                vertices.Add(new Point((start.X + end.X) / 2, (start.Y + end.Y) / 2));
            }
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
        /// <returns>
        /// Point coordinates at position t along the curve defined by p0-p3
        /// </returns>
        /// <remarks>
        /// Implements the cubic Bézier formula:
        /// B(t) = (1-t)^3*P0 + 3*(1-t)^2*t*P1 + 3*(1-t)*t^2*P2 + t^3*P3
        /// </remarks>
        /// <example>
        /// <code>
        /// var point = CalculateBezierPoint(0.5, 
        ///     new Point(0,0), 
        ///     new Point(25,100), 
        ///     new Point(75,100), 
        ///     new Point(100,0));
        /// // Returns midpoint of an S-shaped curve
        /// </code>
        /// </example>
        private static Point CalculateBezierPoint(double t, Point p0, Point p1, Point p2, Point p3)
        {
            double u = 1 - t;
            double tt = t * t;
            double uu = u * u;
            double uuu = uu * u;
            double ttt = tt * t;

            return new Point(
                uuu * p0.X + 3 * uu * t * p1.X + 3 * u * tt * p2.X + ttt * p3.X,
                uuu * p0.Y + 3 * uu * t * p1.Y + 3 * u * tt * p2.Y + ttt * p3.Y
            );
        }

        /// <summary>
        /// Adds snap points for a single figure.
        /// </summary>
        /// <param name="figure">The figure to process.</param>
        /// <param name="points">The list of points to which the snap points will be added.</param>
        private static void AddFigureSnapPoints(PathFigure figure, List<Point> points)
        {
            var vertices = new List<Point> { figure.StartPoint };
            var current = figure.StartPoint;

            foreach (var segment in figure.Segments)
            {
                if (segment is LineSegment line)
                {
                    var end = line.Point;
                    vertices.Add(new Point((current.X + end.X) / 2, (current.Y + end.Y) / 2));
                    current = end;
                    vertices.Add(current);
                }
                else if (segment is PolyLineSegment polyLine)
                {
                    var polyPoints = polyLine.Points;
                    if (polyPoints.Count == 0)
                        continue;

                    int i = 0;
                    Point groupStart = current;
                    Point groupEnd = polyPoints[i];
                    LineType currentLineType = GetLineType(groupStart, groupEnd);

                    for (i = 1; i < polyPoints.Count; i++)
                    {
                        Point nextPoint = polyPoints[i];
                        LineType nextLineType = GetLineType(groupEnd, nextPoint);

                        if (currentLineType != LineType.Diagonal && nextLineType == currentLineType)
                        {
                            groupEnd = nextPoint;
                        }
                        else
                        {
                            AddGroupVertices(groupStart, groupEnd, vertices);
                            groupStart = groupEnd;
                            groupEnd = nextPoint;
                            currentLineType = GetLineType(groupStart, groupEnd);
                        }
                    }

                    AddGroupVertices(groupStart, groupEnd, vertices);
                    current = groupEnd;
                }
                else if (segment is BezierSegment bezier)
                {
                    var mid = CalculateBezierPoint(0.5, current, bezier.Point1, bezier.Point2, bezier.Point3);
                    vertices.Add(mid);
                    vertices.Add(bezier.Point3);
                    current = bezier.Point3;
                }
                else if (segment is PolyBezierSegment polyBezier)
                {
                    var pointsList = polyBezier.Points;
                    for (int i = 0; i < pointsList.Count; i += 3)
                    {
                        if (i + 2 >= pointsList.Count) break;

                        var p1 = current;
                        var p2 = pointsList[i];
                        var p3 = pointsList[i + 1];
                        var p4 = pointsList[i + 2];

                        var mid = CalculateBezierPoint(0.5, p1, p2, p3, p4);
                        vertices.Add(mid);
                        vertices.Add(p4);
                        current = p4;
                    }
                }
            }

            var uniqueVertices = vertices
                .GroupBy(p => new { X = Math.Round(p.X, 1), Y = Math.Round(p.Y, 1) })
                .Select(g => g.First())
                .ToList();

            points.AddRange(uniqueVertices);
        }
    }
}