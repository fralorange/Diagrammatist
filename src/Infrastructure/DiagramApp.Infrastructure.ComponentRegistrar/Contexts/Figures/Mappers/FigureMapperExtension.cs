using DiagramApp.Contracts.Figures;
using DiagramApp.Domain.Figures;

namespace DiagramApp.Infrastructure.ComponentRegistrar.Contexts.Figures.Mappers
{
    /// <summary>
    /// Extension methods for mapping figures to DTOs. 
    /// </summary>
    public static class FigureMapperExtension
    {
        private static void MapCommonProperties(FigureDto target, Figure source)
        {
            target.Name = source.Name;
            target.PosX = source.PosX;
            target.PosY = source.PosY;
            target.Rotation = source.Rotation;
            target.ZIndex = source.ZIndex;
            target.BackgroundColor = source.BackgroundColor;
        }

        private static void MapCommonProperties(Figure target, FigureDto source)
        {
            target.Name = source.Name;
            target.PosX = source.PosX;
            target.PosY = source.PosY;
            target.Rotation = source.Rotation;
            target.ZIndex = source.ZIndex;
            target.BackgroundColor = source.BackgroundColor;
        }

        /// <summary>
        /// Map shape figure to dto.
        /// </summary>
        /// <param name="figure">Source.</param>
        /// <returns><see cref="ShapeFigureDto"/></returns>
        public static ShapeFigureDto ToDto(this ShapeFigure figure)
        {
            var dto = new ShapeFigureDto
            {
                Width = figure.Width,
                Height = figure.Height,
                Data = figure.Data,
            };
            
            MapCommonProperties(dto, figure);
            return dto;
        }

        /// <summary>
        /// Map shape figure dto to entity.
        /// </summary>
        /// <param name="dto">Source.</param>
        /// <returns><see cref="ShapeFigure"/></returns>
        public static ShapeFigure ToEntity(this ShapeFigureDto dto)
        {
            var entity = new ShapeFigure
            {
                Width = dto.Width,
                Height = dto.Height,
                Data = dto.Data,
            };

            MapCommonProperties(entity, dto);
            return entity;
        }

        /// <summary>
        /// Map line figure to dto.
        /// </summary>
        /// <param name="figure">Source.</param>
        /// <returns><see cref="LineFigureDto"/></returns>
        public static LineFigureDto ToDto(this LineFigure figure)
        {
            var dto = new LineFigureDto
            {
                Points = figure.Points,
                Thickness = figure.Thickness,
                IsDashed = figure.IsDashed,
                HasArrow = figure.HasArrow,
            };

            MapCommonProperties(dto, figure);
            return dto;
        }

        /// <summary>
        /// Map line figure dto to entity.
        /// </summary>
        /// <param name="dto">Source.</param>
        /// <returns><see cref="LineFigure"/></returns>
        public static LineFigure ToEntity(this LineFigureDto dto)
        {
            var entity = new LineFigure
            {
                Points = dto.Points,
                Thickness = dto.Thickness,
                IsDashed = dto.IsDashed,
                HasArrow = dto.HasArrow,
            };

            MapCommonProperties(entity, dto);
            return entity;
        }

        /// <summary>
        /// Map text figure to dto.
        /// </summary>
        /// <param name="figure">Source.</param>
        /// <returns><see cref="TextFigureDto"/></returns>
        public static TextFigureDto ToDto(this TextFigure figure)
        {
            var dto = new TextFigureDto
            {
                Text = figure.Text,
                TextColor = figure.TextColor,
                FontSize = figure.FontSize,
                HasBackground = figure.HasBackground,
                HasOutline = figure.HasOutline,
            };

            MapCommonProperties(dto, figure);
            return dto;
        }

        /// <summary>
        /// Map text figure dto to entity.
        /// </summary>
        /// <param name="dto">Source.</param>
        /// <returns><see cref="TextFigure"/></returns>
        public static TextFigure ToEntity(this TextFigureDto dto)
        {
            var entity = new TextFigure
            {
                Text = dto.Text,
                TextColor = dto.TextColor,
                FontSize = dto.FontSize,
                HasBackground = dto.HasBackground,
                HasOutline = dto.HasOutline,
            };

            MapCommonProperties(entity, dto);
            return entity;
        }

        /// <summary>
        /// Map figure to dto.
        /// </summary>
        /// <param name="figure">Source.</param>
        /// <returns><see cref="FigureDto"/></returns>
        /// <exception cref="ArgumentException"></exception>
        public static FigureDto ToDto(this Figure figure)
        {
            return figure switch
            {
                ShapeFigure shapeFigure => shapeFigure.ToDto(),
                LineFigure lineFigure => lineFigure.ToDto(),
                TextFigure textFigure => textFigure.ToDto(),
                _ => throw new ArgumentException("Unsupported figure type", nameof(figure))
            };
        }

        /// <summary>
        /// Map figure dto to entity.
        /// </summary>
        /// <param name="dto">Source.</param>
        /// <returns><see cref="Figure"/></returns>
        /// <exception cref="ArgumentException"></exception>
        public static Figure ToEntity(this FigureDto dto)
        {
            return dto switch
            {
                ShapeFigureDto shapeFigureDto => shapeFigureDto.ToEntity(),
                LineFigureDto lineFigureDto => lineFigureDto.ToEntity(),
                TextFigureDto textFigureDto => textFigureDto.ToEntity(),
                _ => throw new ArgumentException("Unsupported figure dto type", nameof(dto))
            };
        }
    }
}
