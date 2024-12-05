using Diagrammatist.Application.AppServices.Canvas.Services;
using Diagrammatist.Domain.Canvas;
using FluentAssertions;
using CanvasEntity = Diagrammatist.Domain.Canvas.Canvas;

namespace Diagrammatist.Application.AppServices.Tests.Contexts.Canvas.Services
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
        public void EditCanvas_CanvasSettingsEditedSuccessfully()
        {
            // Arrange
            var canvas = new CanvasEntity { Settings = new Settings() };

            var newSettings = new Settings();

            var newFileName = "New File";
            var newWidth = 1024;
            var newHeight = 512;

            newSettings.FileName = newFileName;
            newSettings.Width = newWidth;
            newSettings.Height = newHeight;

            var service = new CanvasManipulationService();

            // Act
            service.UpdateCanvasSettings(canvas, newSettings);
            canvas.Settings = newSettings;

            // Assert
            Assert.Multiple(
                () => Assert.Equal(canvas.Settings.FileName, newFileName),
                () => Assert.Equal(canvas.Settings.Width, newWidth),
                () => Assert.Equal(canvas.Settings.Height, newHeight)
            );
        }
    }
}
