using System.Windows;

namespace Diagrammatist.Presentation.WPF.Core.Helpers
{
    /// <summary>
    /// A class that helps to perform operations with grid.
    /// </summary>
    public static class GridHelper
    {
        /// <summary>
        /// Snaps coordinates to grid.
        /// </summary>
        /// <param name="selBorder">Selection border. (if exists)</param>
        /// <param name="x">X-axis coordinate.</param>
        /// <param name="y">Y-axis coordinate.</param>
        /// <param name="gridStep">Grid step.</param>
        public static void SnapCoordinatesToGrid(ref double x, ref double y, double gridStep)
        {
            x = Math.Round(x / gridStep) * gridStep;
            y = Math.Round(y / gridStep) * gridStep;
        }
    }
}
