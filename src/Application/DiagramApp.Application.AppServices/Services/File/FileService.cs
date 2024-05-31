using DiagramApp.Contracts.Canvas;
using DiagramApp.Contracts.Canvas.Figures;
using DiagramApp.Contracts.DiagramSettings;

namespace DiagramApp.Application.AppServices.Services.File
{
    public class FileService : IFileService
    {
        public void SaveAsync(CanvasDto canvasDto, string filePath)
        {
            using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            using (var writer = new BinaryWriter(stream)) 
            {
                writer.Write(canvasDto.ImaginaryWidth);
                writer.Write(canvasDto.ImaginaryHeight);
                writer.Write(canvasDto.Settings.FileName);
                writer.Write(canvasDto.Settings.Width);
                writer.Write(canvasDto.Settings.Height);
                writer.Write(canvasDto.Settings.Background);
                writer.Write(canvasDto.Settings.Type);
                writer.Write(canvasDto.Zoom);
                writer.Write(canvasDto.Offset.X);
                writer.Write(canvasDto.Offset.Y);
                writer.Write(canvasDto.Controls);
                writer.Write(canvasDto.IsGridVisible);
                writer.Write(canvasDto.GridSpacing);
                writer.Write(canvasDto.Rotation);
                writer.Write(canvasDto.Figures.Count);
                foreach (var figure in canvasDto.Figures)
                {
                    writer.Write(figure.GetType().Name);
                    writer.Write(figure.Name);
                    writer.Write(figure.TranslationX);
                    writer.Write(figure.TranslationY);
                    writer.Write(figure.ZIndex);
                    switch (figure)
                    {
                        case PathFigureDto pathFigure:
                            writer.Write(pathFigure.PathData);
                            writer.Write(pathFigure.Width);
                            writer.Write(pathFigure.Height);
                            writer.Write(pathFigure.Aspect);
                            break;
                        case PolylineFigureDto polylineFigure:
                            writer.Write(polylineFigure.Points.Count);
                            foreach (var point in polylineFigure.Points)
                            {
                                writer.Write(point.X);
                                writer.Write(point.Y);
                            }
                            writer.Write(polylineFigure.Thickness);
                            writer.Write(polylineFigure.Dashed);
                            writer.Write(polylineFigure.LineJoin);
                            break;
                        case TextFigureDto textFigure:
                            writer.Write(textFigure.Text);
                            writer.Write(textFigure.FontSize);
                            writer.Write(textFigure.HasOutline);
                            writer.Write(textFigure.HasBackground);
                            break;
                    }
                }
            }
        }

        public CanvasDto? LoadAsync(string filePath)
        {
            if (!System.IO.File.Exists(filePath) || !filePath.EndsWith(".dgmt"))
                return null;

            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            using (var reader = new BinaryReader(stream))
            {
                var canvasDto = new CanvasDto
                {
                    ImaginaryWidth = reader.ReadInt32(),
                    ImaginaryHeight = reader.ReadInt32(),
                    Settings = new DiagramSettingsDto
                    {
                        FileName = reader.ReadString(),
                        Width = reader.ReadInt32(),
                        Height = reader.ReadInt32(),
                        Background = reader.ReadString(),
                        Type = reader.ReadString()
                    },
                    Zoom = reader.ReadDouble(),
                    Offset = new OffsetDto
                    {
                        X = reader.ReadDouble(),
                        Y = reader.ReadDouble(),
                    },
                    Controls = reader.ReadString(),
                    IsGridVisible = reader.ReadBoolean(),
                    GridSpacing = reader.ReadDouble(),
                    Rotation = reader.ReadDouble(),
                };

                int figureCount = reader.ReadInt32();
                for (int i = 0; i < figureCount; i++)
                {
                    var figureType = reader.ReadString();
                    var figureName = reader.ReadString();
                    var translationX = reader.ReadDouble();
                    var translationY = reader.ReadDouble();
                    var zIndex = reader.ReadDouble();
                    FigureDto figure = figureType switch
                    {
                        nameof(PathFigureDto) => new PathFigureDto
                        {
                            Name = figureName,
                            TranslationX = translationX,
                            TranslationY = translationY,
                            ZIndex = zIndex,
                            PathData = reader.ReadString(),
                            Width = reader.ReadDouble(),
                            Height = reader.ReadDouble(),
                            Aspect = reader.ReadBoolean(),
                        },
                        nameof(PolylineFigureDto) => new PolylineFigureDto
                        {
                            Name = figureName,
                            TranslationX = translationX,
                            TranslationY = translationY,
                            ZIndex = zIndex,
                            Points = Enumerable.Range(0, reader.ReadInt32()).Select(_ => new System.Drawing.Point
                            {
                                X = reader.ReadInt32(),
                                Y = reader.ReadInt32(),
                            }).ToList(),
                            Thickness = reader.ReadDouble(),
                            Dashed = reader.ReadBoolean(),
                            LineJoin = reader.ReadString()
                        },
                        nameof(TextFigureDto) => new TextFigureDto
                        {
                            Name = figureName,
                            TranslationX = translationX,
                            TranslationY = translationY,
                            ZIndex = zIndex,
                            Text = reader.ReadString(),
                            FontSize = reader.ReadDouble(),
                            HasOutline = reader.ReadBoolean(),
                            HasBackground = reader.ReadBoolean(),
                        },
                        _ => throw new InvalidOperationException("Unknown figure type")
                    };
                    canvasDto.Figures.Add(figure);
                }

                return canvasDto;
            }
        }
    }
}
