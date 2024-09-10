using DiagramApp.Contracts.Figures;

namespace DiagramApp.Infrastructure.ComponentRegistrar.Contexts.Figures.Cloners
{
    /// <summary>
    /// A class that contains extensions methods for cloning figure DTOs.
    /// </summary>
    public static class FigureClonerExtension
    {
        private static void CloneCommonProperties(FigureDto target, FigureDto source)
        {
            target.Name = source.Name;
            target.PosX = source.PosX;
            target.PosY = source.PosY;
            target.Rotation = source.Rotation;
            target.ZIndex = source.ZIndex;
            target.BackgroundColor = source.BackgroundColor;
        }

        /// <summary>
        /// Clone shape figure dto.
        /// </summary>
        /// <param name="figure"></param>
        /// <returns></returns>
        public static ShapeFigureDto Clone(this ShapeFigureDto figure)
        {
            var cloneFigure = new ShapeFigureDto
            {
                Width = figure.Width,
                Height = figure.Height,
                KeepAspectRatio = figure.KeepAspectRatio,
                Data = figure.Data.Select(s => s).ToList()
            };

            CloneCommonProperties(cloneFigure, figure);

            return cloneFigure;
        }

        /// <summary>
        /// Clone line figure dto.
        /// </summary>
        /// <param name="figure"></param>
        /// <returns></returns>
        public static LineFigureDto Clone(this LineFigureDto figure)
        {
            var cloneFigure = new LineFigureDto
            {
                Thickness = figure.Thickness,
                HasArrow = figure.HasArrow,
                IsDashed = figure.IsDashed,
                Points = figure.Points.Select(p => p).ToList(),
            };

            CloneCommonProperties(cloneFigure, figure);

            return cloneFigure;
        }

        /// <summary>
        /// Clone text figure dto.
        /// </summary>
        /// <param name="figure"></param>
        /// <returns></returns>
        public static TextFigureDto Clone(this TextFigureDto figure)
        {
            var cloneFigure = new TextFigureDto
            {
                Text = figure.Text,
                TextColor = figure.TextColor,
                FontSize = figure.FontSize,
                HasBackground = figure.HasBackground,
                HasOutline = figure.HasOutline,
            };

            CloneCommonProperties(cloneFigure, figure);

            return cloneFigure;
        }

        public static FigureDto Clone(this FigureDto figure)
        {
            return figure switch
            {
                ShapeFigureDto shapeFigure => shapeFigure.Clone(),
                LineFigureDto lineFigure => lineFigure.Clone(),
                TextFigureDto textFigure => textFigure.Clone(),
                _ => throw new ArgumentException("Unsupported figure type", nameof(figure))
            };
        }
    }
}
