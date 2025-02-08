using Diagrammatist.Application.AppServices.Canvas.Serializer;
using Diagrammatist.Application.AppServices.Canvas.Services;
using Diagrammatist.Application.AppServices.Figures.Serializer.Configuration;
using Diagrammatist.Domain.Figures;
using FluentAssertions;
using System.Drawing;
using CanvasEnt = Diagrammatist.Domain.Canvas.Canvas;

namespace Diagrammatist.Application.AppServices.Tests.Contexts.Canvas.Services.Integration
{
    /// <summary>
    /// A canvas serialization service integration tests.
    /// </summary>
    public class CanvasSerializationServiceTests
    {
        [Fact]
        public void SaveAndLoadCanvas_ShouldPersistDataCorrectly()
        {
            // Arrange
            var configurator = new SerializationConfigurator();
            var serializer = new CanvasSerializer(configurator.Configure());
            var service = new CanvasSerializationService(serializer);

            var canvas = new CanvasEnt
            {
                ImaginaryWidth = 2048,
                ImaginaryHeight = 2048,
                Settings = new()
                {
                    Width = 512,
                    Height = 512,
                    Background = Color.Snow,
                    FileName = "File1",
                    Type = Domain.Canvas.Diagrams.Custom
                },
                Figures =
                [
                    new ShapeFigure() { Name = "Shape1", PosX = 50, PosY = 100, Data = ["M 10,100 C 10,300 300,-200 300,100"], KeepAspectRatio = false},
                    new TextFigure() { Name = "Text1", PosX = 100, PosY = 100, Text = "Lorem", TextColor = Color.Black, HasBackground = false, HasOutline = true },
                    new LineFigure { Name = "Line1", PosX = 70, PosY = 80, IsDashed = true, BackgroundColor = Color.Aqua, Points = [new(1,1), new(2,2), new(3,3)]}
                ]
            };

            string filePath = "test_canvas.dgmt";

            try
            {
                // Act
                service.SaveCanvas(canvas, filePath);
                var loadedCanvas = service.LoadCanvas(filePath);

                // Assert
                loadedCanvas.Should().BeEquivalentTo(canvas, opt => opt
                    .Using<Color>(ctx => ctx.Subject.ToArgb().Should().Be(ctx.Expectation.ToArgb()))
                    .WhenTypeIs<Color>()
                    .IncludingAllRuntimeProperties()
                );
            }
            finally
            {
                // Cleanup
                if (File.Exists(filePath))
                    File.Delete(filePath);
            }
        }
    }
}
