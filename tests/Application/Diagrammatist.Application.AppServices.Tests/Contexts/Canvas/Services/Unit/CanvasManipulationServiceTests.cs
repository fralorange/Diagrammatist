using Diagrammatist.Application.AppServices.Canvas.Services;
using Diagrammatist.Domain.Canvas;
using FluentAssertions;
using System.Drawing;
using CanvasEntity = Diagrammatist.Domain.Canvas.Canvas;

namespace Diagrammatist.Application.AppServices.Tests.Contexts.Canvas.Services.Unit
{
    /// <summary>
    /// Canvas manipulation service tests.
    /// </summary>
    public class CanvasManipulationServiceTests
    {
        [Fact]
        public async void CreateCanvas_CanvasCreatedWithDefaultSettingsAndBorders()
        {
            // Arrange
            var settings = new Settings();

            var service = new CanvasManipulationService();

            var expectedCanvas = new CanvasEntity
            {
                ImaginaryWidth = 2048,
                ImaginaryHeight = 2048,
                Settings = settings,
                Zoom = 1,
                Offset = new() { X = default, Y = default },
                Figures = [],
            };

            // Act
            var canvas = await service.CreateCanvasAsync(settings);

            // Assert
            canvas.Should().BeEquivalentTo(expectedCanvas);
        }

        [Fact]
        public void EditCanvas_CanvasSizeChangedSuccessfully()
        {
            // Arrange
            var canvas = new CanvasEntity { Settings = new Settings() };
            var service = new CanvasManipulationService();

            var newSize = new Size(1024, 1024);

            // Act
            service.UpdateCanvas(canvas, newSize);

            // Assert
            Assert.Multiple(
                () => Assert.Equal(newSize.Width, canvas.Settings.Width),
                () => Assert.Equal(newSize.Height, canvas.Settings.Height));
        }

        [Fact]
        public void EditCanvas_CanvasBackgroundChangedSuccessfully()
        {
            // Arrange
            var canvas = new CanvasEntity { Settings = new Settings() { Background = Color.AntiqueWhite } };
            var service = new CanvasManipulationService();

            var background = Color.Aquamarine;

            // Act
            service.UpdateCanvas(canvas, background);

            // Assert
            Assert.Equal(background, canvas.Settings.Background);
        }
    }
}
