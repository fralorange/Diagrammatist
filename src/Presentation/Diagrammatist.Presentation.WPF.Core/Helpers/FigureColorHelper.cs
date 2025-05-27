using Diagrammatist.Presentation.WPF.Core.Models.Figures;
using Diagrammatist.Presentation.WPF.Core.Models.Figures.Special.Container;
using Diagrammatist.Presentation.WPF.Core.Shared.Records;
using System.Windows.Media;
using ApplicationEnt = System.Windows.Application;

namespace Diagrammatist.Presentation.WPF.Core.Helpers
{
    /// <summary>
    /// A helper class for updating figure colors based on the current application theme.
    /// </summary>
    public static class FigureColorHelper
    {
        /// <summary>
        /// Gets the background color for figures based on the application theme.
        /// </summary>
        public static Color BackgroundColor =>
            (Color)ApplicationEnt.Current.Resources["AppBackground"];

        /// <summary>
        /// Gets the text color for figures based on the application theme.
        /// </summary>
        public static Color TextColor =>
            (Color)ApplicationEnt.Current.Resources["AppTextColor"];

        /// <summary>
        /// Gets the theme color for figures based on the application theme.
        /// </summary>
        public static Color ThemeColor =>
            (Color)ApplicationEnt.Current.Resources["AppThemeColor"];

        /// <summary>
        /// Applies the current theme colors to the specified figure model.
        /// </summary>
        /// <param name="figure"></param>
        public static void ApplyColors(FigureModel figure)
        {
            figure.SuppressNotifications = true;

            switch (figure)
            {
                case LineFigureModel line:
                    line.BackgroundColor = ThemeColor;
                    break;
                case ContainerFigureModel container:
                    container.TextColor = TextColor;
                    container.BackgroundColor = BackgroundColor;
                    break;
                case TextFigureModel text:
                    text.TextColor = TextColor;
                    text.BackgroundColor = BackgroundColor;
                    break;
                default:
                    figure.BackgroundColor = BackgroundColor;
                    break;
            }

            figure.SuppressNotifications = false;
        }

        /// <summary>
        /// Applies the current theme colors to a collection of figure models.
        /// </summary>
        /// <param name="models"></param>
        public static void ApplyColors(IEnumerable<FigureModel>? models)
        {
            if (models is null)
                return;

            foreach (var model in models)
            {
                ApplyColors(model);
            }
        }

        /// <summary>
        /// Captures the current colors of a collection of figure models into a list of snapshots.
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public static List<FigureColorSnapshot> Capture(IEnumerable<FigureModel>? models)
        {
            var snapshots = new List<FigureColorSnapshot>();
            if (models is null)
                return snapshots;

            foreach (var f in models)
            {
                snapshots.Add(new FigureColorSnapshot(
                    Figure: f,
                    BackgroundColor: f.BackgroundColor,
                    TextColor: f is ContainerFigureModel c ? c.TextColor
                             : f is TextFigureModel t ? t.TextColor
                             : (Color?)null,
                    ThemeColor: f is LineFigureModel l ? l.BackgroundColor 
                                : (Color?)null
                ));
            }

            return snapshots;
        }

        /// <summary>
        /// Restores the colors of figure models from a collection of snapshots.
        /// </summary>
        /// <param name="snapshots"></param>
        public static void Restore(IEnumerable<FigureColorSnapshot> snapshots)
        {
            foreach (var snap in snapshots)
            {
                var f = snap.Figure;
                f.SuppressNotifications = true;

                f.BackgroundColor = snap.BackgroundColor;

                switch (f)
                {
                    case ContainerFigureModel c when snap.TextColor.HasValue:
                        c.TextColor = snap.TextColor.Value;
                        break;
                    case TextFigureModel t when snap.TextColor.HasValue:
                        t.TextColor = snap.TextColor.Value;
                        break;
                    case LineFigureModel l when snap.ThemeColor.HasValue:
                        l.BackgroundColor = snap.ThemeColor.Value;
                        break;
                }

                f.SuppressNotifications = false;
            }
        }
    }
}
