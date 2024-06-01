using DiagramApp.Application.AppServices.Services.File;
using DiagramApp.Contracts.Canvas;
using DiagramApp.Contracts.Canvas.Figures;
using DiagramApp.Contracts.DiagramSettings;
using FluentAssertions;

namespace DiagramApp.Tests.Unit
{
    public class FeatureTestingFileService
    {
        [Fact]
        public void FileService_SaveFile_FileSavedCorrectly()
        {
            // Arrange
            var service = new FileService();
            string projectDirectory = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory)!;
            string filePath = projectDirectory + "\\testFile.dgmt";

            var dto = new CanvasDto
            {
                Settings = new DiagramSettingsDto
                {
                    FileName = "Unnamed",
                    Width = 512,
                    Height = 512,
                    Type = "Custom",
                    Background = "Default",
                },
                ImaginaryWidth = 512,
                ImaginaryHeight = 512,
                Controls = "Move",
                GridSpacing = 50,
                IsGridVisible = true,
                Rotation = 0,
                Offset = new OffsetDto
                {
                    X = 100,
                    Y = 200,
                },
                Zoom = 1,
                Figures = new List<FigureDto>
                {
                    new TextFigureDto
                    {
                          Name = "Figure1",
                          TranslationX = 0,
                          TranslationY = 0,
                          ZIndex = 1,
                          Text = "Lorem",
                          FontSize = 14,
                          HasBackground = true,
                          HasOutline = true,
                    },
                    new PathFigureDto
                    {
                        Name = "Figure2",
                        TranslationX = 120,
                        TranslationY = 140,
                        ZIndex = 1,
                        Width = 50,
                        Height = 50,
                        Aspect = false,
                        PathData = "M0,0 L0,100 L100,100 L100,0 Z"
                    },
                    new PolylineFigureDto
                    {
                        Name = "Figure3",
                        TranslationX = 256,
                        TranslationY = 240,
                        ZIndex = 2,
                        Dashed = true,
                        LineJoin = "Miter",
                        Thickness = 2,
                        Points = [new(0, 0), new (100, 100)]
                    }
                }
            };

            // Act
            service.Save(dto, filePath);

            // Assert
            Assert.True(File.Exists(filePath));
        }

        [Fact]
        public void FileService_LoadFile_FileLoadedCorrectly()
        {
            // Arrange
            var service = new FileService();
            string projectDirectory = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory)!;
            string filePath = projectDirectory + "\\testFile.dgmt";

            var expected = new CanvasDto
            {
                Settings = new DiagramSettingsDto
                {
                    FileName = "Unnamed",
                    Width = 512,
                    Height = 512,
                    Type = "Custom",
                    Background = "Default",
                },
                ImaginaryWidth = 512,
                ImaginaryHeight = 512,
                Controls = "Move",
                GridSpacing = 50,
                IsGridVisible = true,
                Rotation = 0,
                Offset = new OffsetDto
                {
                    X = 100,
                    Y = 200,
                },
                Zoom = 1,
                Figures = new List<FigureDto>
                {
                    new TextFigureDto
                    {
                          Name = "Figure1",
                          TranslationX = 0,
                          TranslationY = 0,
                          ZIndex = 1,
                          Text = "Lorem",
                          FontSize = 14,
                          HasBackground = true,
                          HasOutline = true,
                    },
                    new PathFigureDto
                    {
                        Name = "Figure2",
                        TranslationX = 120,
                        TranslationY = 140,
                        ZIndex = 1,
                        Width = 50,
                        Height = 50,
                        Aspect = false,
                        PathData = "M0,0 L0,100 L100,100 L100,0 Z"
                    },
                    new PolylineFigureDto
                    {
                        Name = "Figure3",
                        TranslationX = 256,
                        TranslationY = 240,
                        ZIndex = 2,
                        Dashed = true,
                        LineJoin = "Miter",
                        Thickness = 2,
                        Points = [new(0, 0), new (100, 100)]
                    }
                }
            };

            // Act
            var actual = service.Load(filePath);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }
    }
}
