using Diagrammatist.Presentation.WPF.Core.Models.Figures;
using System.Windows;

namespace Diagrammatist.Presentation.WPF.Core.Services.Formatting
{
    /// <summary>
    /// A class that implements the <see cref="IFormattingContext"/> interface.
    /// This class provides a context for formatting operations on figures.
    /// </summary>
    public class FormattingContext : IFormattingContext
    {
        /// <inheritdoc/>
        public void MoveElement(FigureModel figure, Point newPosition)
        {
            ExecuteIsolatedAction(figure, () =>
            {
                figure.PosX = newPosition.X;
                figure.PosY = newPosition.Y;
            });
        }

        /// <inheritdoc/>
        public void SetElementSize(FigureModel figure, double width, double height)
        {
            ExecuteIsolatedAction(figure, () =>
            {               
                if (figure is ShapeFigureModel shape)
                {
                    shape.Width = width;
                    shape.Height = height;
                    shape.KeepAspectRatio = true;
                }
                else if (figure is TextFigureModel text)
                {
                    text.FontSize = height * 0.25;
                }
            });
        }

        private void ExecuteIsolatedAction(FigureModel figure, Action action)
        {
            figure.SuppressNotifications = true;
            action();
            figure.SuppressNotifications = false;
        }
    }
}
