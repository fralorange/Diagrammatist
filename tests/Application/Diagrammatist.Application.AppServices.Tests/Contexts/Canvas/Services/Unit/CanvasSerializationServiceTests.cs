using Diagrammatist.Application.AppServices.Canvas.Serializer;
using Diagrammatist.Application.AppServices.Canvas.Services;
using CanvasEnt = Diagrammatist.Domain.Canvas.Canvas;
using Moq;
using System.Drawing;
using FluentAssertions;

namespace Diagrammatist.Application.AppServices.Tests.Contexts.Canvas.Services.Unit
{
    /// <summary>
    /// A canvas serialization service unit tests.
    /// </summary>
    public class CanvasSerializationServiceTests
    {
        [Fact]
        public void SaveCanvas_ShouldCallSaveToFile()
        {
            // Arrange
            var mockSerializer = new Mock<ICanvasSerializer>();
            var service = new CanvasSerializationService(mockSerializer.Object);
            var canvas = new CanvasEnt
            {
                Settings = new()
                {
                    Width = 512,
                    Height = 512,
                    Background = Color.Snow,
                    FileName = "File1",
                    Type = Domain.Canvas.Diagrams.Custom
                }
            };

            // Act
            service.SaveCanvas(canvas, "test_path");

            // Assert
            mockSerializer.Verify(s => s.SaveToFile(canvas, "test_path"), Times.Once);
        }

        [Fact]
        public void LoadCanvas_ShouldReturnDeserializedCanvas()
        {
            // Arrange
            var mockSerializer = new Mock<ICanvasSerializer>();
            var expected = new CanvasEnt
            {
                Settings = new()
                {
                    Width = 512,
                    Height = 512,
                    Background = Color.Snow,
                    FileName = "File1",
                    Type = Domain.Canvas.Diagrams.Custom
                }
            };

            mockSerializer.Setup(s => s.LoadFromFile("test_path")).Returns(expected);

            var service = new CanvasSerializationService(mockSerializer.Object);

            // Act
            var actual = service.LoadCanvas("test_path");

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }
    }
}
