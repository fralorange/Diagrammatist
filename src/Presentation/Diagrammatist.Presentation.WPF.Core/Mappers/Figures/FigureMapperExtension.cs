using Diagrammatist.Domain.Figures;
using Diagrammatist.Domain.Figures.Special.Container;
using Diagrammatist.Domain.Figures.Special.Flowchart;
using Diagrammatist.Presentation.WPF.Core.Foundation.Extensions;
using Diagrammatist.Presentation.WPF.Core.Models.Figures;
using Diagrammatist.Presentation.WPF.Core.Models.Figures.Special.Container;
using Diagrammatist.Presentation.WPF.Core.Models.Figures.Special.Flowchart;

namespace Diagrammatist.Presentation.WPF.Core.Mappers.Figures
{
    /// <summary>
    /// Extension methods for mapping figures to models. 
    /// </summary>
    public static class FigureMapperExtension
    {
        private static void MapCommonProperties(FigureModel target, Figure source)
        {
            target.Id = source.Id;
            target.Name = source.Name;
            target.PosX = source.PosX;
            target.PosY = source.PosY;
            target.Rotation = source.Rotation;
            target.ZIndex = source.ZIndex;
            target.BackgroundColor = System.Windows.Media.Color
                .FromArgb(source.BackgroundColor.A, source.BackgroundColor.R, source.BackgroundColor.G, source.BackgroundColor.B);
        }

        private static void MapCommonProperties(Figure target, FigureModel source)
        {
            target.Id = source.Id;
            target.Name = source.Name;
            target.PosX = source.PosX;
            target.PosY = source.PosY;
            target.Rotation = source.Rotation;
            target.ZIndex = source.ZIndex;
            target.BackgroundColor = System.Drawing.Color
                .FromArgb(source.BackgroundColor.A, source.BackgroundColor.R, source.BackgroundColor.G, source.BackgroundColor.B);
        }

        private static void MapShapeProperties(ShapeFigureModel target, ShapeFigure source)
        {
            target.Width = source.Width;
            target.Height = source.Height;
            target.Data = source.Data;
            target.KeepAspectRatio = source.KeepAspectRatio;
        }

        private static void MapShapeProperties(ShapeFigure target, ShapeFigureModel source)
        {
            target.Width = source.Width;
            target.Height = source.Height;
            target.Data = source.Data;
            target.KeepAspectRatio = source.KeepAspectRatio;
        }

        private static void MapLineProperties(LineFigureModel target, LineFigure source)
        {
            target.Points = source.Points.Select(p => new System.Windows.Point(p.X, p.Y)).ToObservableCollection();
            target.Thickness = source.Thickness;
            target.IsDashed = source.IsDashed;
            target.HasArrow = source.HasArrow;
        }

        private static void MapLineProperties(LineFigure target, LineFigureModel source)
        {
            target.Points = source.Points.Select(p => new System.Drawing.PointF((float)p.X, (float)p.Y)).ToList();
            target.Thickness = source.Thickness;
            target.IsDashed = source.IsDashed;
            target.HasArrow = source.HasArrow;
        }

        /// <summary>
        /// Map shape figure from domain to model.
        /// </summary>
        /// <param name="figure">Source.</param>
        /// <returns><see cref="ShapeFigureModel"/></returns>
        public static ShapeFigureModel ToModel(this ShapeFigure figure)
        {
            var model = new ShapeFigureModel();

            MapShapeProperties(model, figure);
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
            var domain = new ShapeFigure();

            MapShapeProperties(domain, model);
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
            var model = new LineFigureModel();

            MapLineProperties(model, figure);
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
            var domain = new LineFigure();

            MapLineProperties(domain, model);
            MapCommonProperties(domain, model);
            return domain;
        }

        /// <summary>
        /// Map flow line figure from domain to model.
        /// </summary>
        /// <param name="figure"></param>
        /// <returns></returns>
        public static FlowLineFigureModel ToModel(this FlowLineFigure figure)
        {
            var model = new FlowLineFigureModel
            {
                Condition = (FlowConditionModel)Enum.Parse(typeof(FlowConditionModel), figure.Condition.ToString()),
                Label = figure.Label,
                LabelFontSize = figure.LabelFontSize
            };

            MapLineProperties(model, figure);
            MapCommonProperties(model, figure);

            return model;
        }

        /// <summary>
        /// Map flow line figure from model to domain.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static FlowLineFigure ToDomain(this FlowLineFigureModel model)
        {
            var domain = new FlowLineFigure
            {
                Condition = (FlowCondition)Enum.Parse(typeof(FlowCondition), model.Condition.ToString()),
                Label = model.Label,
                LabelFontSize = model.LabelFontSize,
                LabelPositionX = model.LabelPosition.X,
                LabelPositionY = model.LabelPosition.Y
            };

            MapLineProperties(domain, model);
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
                TextColor = System.Windows.Media.Color
                    .FromArgb(figure.TextColor.A, figure.TextColor.R, figure.TextColor.G, figure.TextColor.B),
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
                TextColor = System.Drawing.Color
                    .FromArgb(figure.TextColor.A, figure.TextColor.R, figure.TextColor.G, figure.TextColor.B),
                FontSize = figure.FontSize,
                HasBackground = figure.HasBackground,
                HasOutline = figure.HasOutline,
            };

            MapCommonProperties(domain, figure);
            return domain;
        }

        /// <summary>
        /// Map container figure from domain to model.
        /// </summary>
        /// <param name="figure">Source.</param>
        /// <returns><see cref="ContainerFigureModel"/></returns>
        public static ContainerFigureModel ToModel(this ContainerFigure figure)
        {
            var model = new ContainerFigureModel
            {
                Text = figure.Text,
                TextColor = System.Windows.Media.Color
                    .FromArgb(figure.TextColor.A, figure.TextColor.R, figure.TextColor.G, figure.TextColor.B),
                FontSize = figure.FontSize,
            };

            MapShapeProperties(model, figure);
            MapCommonProperties(model, figure);
            return model;
        }

        /// <summary>
        /// Map container figure from model to domain.
        /// </summary>
        /// <param name="model"></param>
        /// <returns><see cref="ContainerFigure"/></returns>
        public static ContainerFigure ToDomain(this ContainerFigureModel model)
        {
            var domain = new ContainerFigure
            {
                Text = model.Text,
                TextColor = System.Drawing.Color
                    .FromArgb(model.TextColor.A, model.TextColor.R, model.TextColor.G, model.TextColor.B),
                FontSize = model.FontSize,
            };

            MapShapeProperties(domain, model);
            MapCommonProperties(domain, model);
            return domain;
        }

        /// <summary>
        /// Map flowchart figure from domain to model.
        /// </summary>
        /// <param name="figure"></param>
        /// <returns><see cref="FlowchartFigureModel"/></returns>
        public static FlowchartFigureModel ToModel(this FlowchartFigure figure)
        {
            var model = new FlowchartFigureModel
            {
                Text = figure.Text,
                TextColor = System.Windows.Media.Color
                    .FromArgb(figure.TextColor.A, figure.TextColor.R, figure.TextColor.G, figure.TextColor.B),
                FontSize = figure.FontSize,
                Subtype = (FlowchartSubtypeModel)Enum.Parse(typeof(FlowchartSubtypeModel), figure.Subtype.ToString()),
            };

            MapShapeProperties(model, figure);
            MapCommonProperties(model, figure);
            return model;
        }

        /// <summary>
        /// Map flowchart figure from model to domain.
        /// </summary>
        /// <param name="model"></param>
        /// <returns><see cref="FlowchartFigure"/></returns>
        public static FlowchartFigure ToDomain(this FlowchartFigureModel model)
        {
            var domain = new FlowchartFigure
            {
                Text = model.Text,
                TextColor = System.Drawing.Color.FromArgb(model.TextColor.A, model.TextColor.R, model.TextColor.G, model.TextColor.B),
                FontSize = model.FontSize,
                Subtype = (FlowchartSubtype)Enum.Parse(typeof(FlowchartSubtype), model.Subtype.ToString()),
            };

            MapShapeProperties(domain, model);
            MapCommonProperties(domain, model);
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
                FlowchartFigure flowchartFigure => flowchartFigure.ToModel(),
                ContainerFigure containerFigure => containerFigure.ToModel(),
                ShapeFigure shapeFigure => shapeFigure.ToModel(),
                FlowLineFigure flowLineFigure => flowLineFigure.ToModel(),
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
                FlowchartFigureModel flowchartFigureModel => flowchartFigureModel.ToDomain(),
                ContainerFigureModel containerFigureModel => containerFigureModel.ToDomain(),
                ShapeFigureModel shapeFigureModel => shapeFigureModel.ToDomain(),
                FlowLineFigureModel flowLineFigureModel => flowLineFigureModel.ToDomain(),
                LineFigureModel lineFigureModel => lineFigureModel.ToDomain(),
                TextFigureModel textFigureModel => textFigureModel.ToDomain(),
                _ => throw new ArgumentException("Unsupported figure model type", nameof(model))
            };
        }
    }
}
