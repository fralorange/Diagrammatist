using DiagramApp.Client.Mappers.Figure;
using DiagramApp.Client.ViewModels.Wrappers;
using DiagramApp.Contracts.Canvas;
using DiagramApp.Contracts.DiagramSettings;
using DiagramApp.Domain.Canvas;
using DiagramApp.Domain.DiagramSettings;
using System.Collections.ObjectModel;

namespace DiagramApp.Client.Mappers.Canvas
{
    public class CanvasMapper : ICanvasMapper
    {
        private readonly IFigureMapper _figureMapper;

        public CanvasMapper(IFigureMapper figureMapper)
        {
            _figureMapper = figureMapper;
        }

        public CanvasDto ToDto(ObservableCanvas canvas)
        {
            return new CanvasDto
            {
                ImaginaryWidth = canvas.ImaginaryWidth,
                ImaginaryHeight = canvas.ImaginaryHeight,
                Settings = new DiagramSettingsDto
                {
                    FileName = canvas.Settings.FileName,
                    Width = canvas.Settings.Width,
                    Height = canvas.Settings.Height,
                    Background = canvas.Settings.Background.ToString(),
                    Type = canvas.Settings.Type.ToString(),
                },
                Zoom = canvas.Zoom,
                Offset = new OffsetDto
                {
                    X = canvas.Offset.X,
                    Y = canvas.Offset.Y,
                },
                Controls = canvas.Controls.ToString(),
                IsGridVisible = canvas.IsGridVisible,
                GridSpacing = canvas.GridSpacing,
                Rotation = canvas.Rotation,
                FileLocation = canvas.FileLocation,
                Figures = canvas.Figures.Select(f => _figureMapper.ToDto(f)).ToList()
            };
        }

        public ObservableCanvas FromDto(CanvasDto dto)
        {
            var settings = new DiagramSettings
            {
                FileName = dto.Settings.FileName,
                Width = dto.Settings.Width,
                Height = dto.Settings.Height,
                Background = (BackgroundType)Enum.Parse(typeof(BackgroundType), dto.Settings.Background),
                Type = (DiagramType)Enum.Parse(typeof(DiagramType), dto.Settings.Type),
            };

            var canvas = new Domain.Canvas.Canvas(settings)
            {
                Controls = (ControlsType)Enum.Parse(typeof(ControlsType), dto.Controls),
                ImaginaryWidth = dto.ImaginaryWidth,
                ImaginaryHeight = dto.ImaginaryHeight,
                Zoom = dto.Zoom,
                Offset = new Offset
                {
                    X = dto.Offset.X,
                    Y = dto.Offset.Y
                },
            };

            var obsCanvas = new ObservableCanvas(canvas)
            {
                IsGridVisible = dto.IsGridVisible,
                GridSpacing = dto.GridSpacing,
                Rotation = dto.Rotation,
                FileLocation = dto.FileLocation,
            };

            foreach (var item in dto.Figures)
            {
                obsCanvas.Figures.Add(_figureMapper.FromDto(item));
            }

            return obsCanvas;
        }
    }
}
