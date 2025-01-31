using Diagrammatist.Domain.Figures;
using Diagrammatist.Presentation.WPF.Core.Models.Figures;

namespace Diagrammatist.Presentation.WPF.Core.Mappers.Figures
{
    /// <summary>
    /// Extension methods for mapping figures to models. 
    /// </summary>
    public static class FigureMapperExtension
    {
        private static void MapCommonProperties(FigureModel target, Figure source)
        {
            target.Name = source.Name;
            target.PosX = source.PosX;
            target.PosY = source.PosY;
            target.Rotation = source.Rotation;
            target.ZIndex = source.ZIndex;
            target.BackgroundColor = source.BackgroundColor;
        }

        private static void MapCommonProperties(Figure target, FigureModel source)
        {
            target.Name = source.Name;
            target.PosX = source.PosX;
            target.PosY = source.PosY;
            target.Rotation = source.Rotation;
            target.ZIndex = source.ZIndex;
            target.BackgroundColor = source.BackgroundColor;
        }

        /// <summary>
        /// Map shape figure from domain to model.
        /// </summary>
        /// <param name="figure">Source.</param>
        /// <returns><see cref="ShapeFigureModel"/></returns>
        public static ShapeFigureModel ToModel(this ShapeFigure figure)
        {
            var model = new ShapeFigureModel
            {
                Width = figure.Width,
                Height = figure.Height,
                KeepAspectRatio = figure.KeepAspectRatio,
                Data = figure.Data,
            };

            MapCommonProperties(model, figure);
            return model;
        }

        /// <summary>
        /// Map shape figure from model to domain.
        /// </summary>
        /// <param name="model">Source.</param>
        /// <returns><see cref="ShapeFigure"/></returns>
        public static ShapeFigure ToDomain(this ShapeFigureModel model)
        {
            var domain = new ShapeFigure
            {
                Width = model.Width,
                Height = model.Height,
                Data = model.Data,
            };

            MapCommonProperties(domain, model);
            return domain;
        }

        /// <summary>
        /// Map line figure from domain to model.
        /// </summary>
        /// <param name="figure">Source.</param>
        /// <returns><see cref="LineFigureModel"/></returns>
        public static LineFigureModel ToModel(this LineFigure figure)
        {
            var model = new LineFigureModel
            {
                Points = figure.Points,
                Thickness = figure.Thickness,
                IsDashed = figure.IsDashed,
                HasArrow = figure.HasArrow,
            };

            MapCommonProperties(model, figure);
            return model;
        }

        /// <summary>
        /// Map line figure from model to domain.
        /// </summary>
        /// <param name="model">Source.</param>
        /// <returns><see cref="LineFigure"/></returns>
        public static LineFigure ToDomain(this LineFigureModel model)
        {
            var domain = new LineFigure
            {
                Points = model.Points,
                Thickness = model.Thickness,
                IsDashed = model.IsDashed,
                HasArrow = model.HasArrow,
            };

            MapCommonProperties(domain, model);
            return domain;
        }

        /// <summary>
        /// Map text figure from domain to model.
        /// </summary>
        /// <param name="figure">Source.</param>
        /// <returns><see cref="TextFigureModel"/></returns>
        public static TextFigureModel ToModel(this TextFigure figure)
        {
            var model = new TextFigureModel
            {
                Text = figure.Text,
                TextColor = figure.TextColor,
                FontSize = figure.FontSize,
                HasBackground = figure.HasBackground,
                HasOutline = figure.HasOutline,
            };

            MapCommonProperties(model, figure);
            return model;
        }

        /// <summary>
        /// Map text figure from model to domain.
        /// </summary>
        /// <param name="figure">Source.</param>
        /// <returns><see cref="TextFigure"/></returns>
        public static TextFigure ToDomain(this TextFigureModel figure)
        {
            var domain = new TextFigure
            {
                Text = figure.Text,
                TextColor = figure.TextColor,
                FontSize = figure.FontSize,
                HasBackground = figure.HasBackground,
                HasOutline = figure.HasOutline,
            };

            MapCommonProperties(domain, figure);
            return domain;
        }

        /// <summary>
        /// Map figure from domain to model.
        /// </summary>
        /// <param name="figure">Source.</param>
        /// <returns><see cref="FigureModel"/></returns>
        /// <exception cref="ArgumentException"></exception>
        public static FigureModel ToModel(this Figure figure)
        {
            return figure switch
            {
                ShapeFigure shapeFigure => shapeFigure.ToModel(),
                LineFigure lineFigure => lineFigure.ToModel(),
                TextFigure textFigure => textFigure.ToModel(),
                _ => throw new ArgumentException("Unsupported figure type", nameof(figure))
            };
        }

        /// <summary>
        /// Map figure model to domain.
        /// </summary>
        /// <param name="model">Source.</param>
        /// <returns><see cref="Figure"/></returns>
        /// <exception cref="ArgumentException"></exception>
        public static Figure ToDomain(this FigureModel model)
        {
            return model switch
            {
                ShapeFigureModel shapeFigureModel => shapeFigureModel.ToDomain(),
                LineFigureModel lineFigureModel => lineFigureModel.ToDomain(),
                TextFigureModel textFigureModel => textFigureModel.ToDomain(),
                _ => throw new ArgumentException("Unsupported figure model type", nameof(model))
            };
        }
    }
}
