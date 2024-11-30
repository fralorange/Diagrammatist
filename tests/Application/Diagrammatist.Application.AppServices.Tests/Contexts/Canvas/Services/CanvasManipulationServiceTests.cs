using Diagrammatist.Application.AppServices.Contexts.Canvas.Services;
using Diagrammatist.Contracts.Canvas;
using Diagrammatist.Domain.Settings;
using Diagrammatist.Infrastructure.ComponentRegistrar.Contexts.Settings.Mappers;
using FluentAssertions;

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
            var settings = new DiagramSettings();
            var settingsDto = settings.ToDto();

            var service = new CanvasManipulationService();

            var expectedCanvas = new CanvasDto
            {
                ImaginaryWidth = 2048,
                ImaginaryHeight = 2048,
                Settings = settings.ToDto(),
                Zoom = 1,
                ScreenOffset = new() { X = default, Y = default },
                Figures = [],
            };

            // Act
            var canvas = await service.CreateCanvasAsync(settingsDto);

            // Assert
            canvas.Should().BeEquivalentTo(expectedCanvas);
        }

        [Fact]
        public void EditCanvas_CanvasSettingsEditedSuccessfully()
        {
            // Arrange
            var canvas = new CanvasDto { Settings = new DiagramSettings().ToDto() };

            var settings = new DiagramSettings();
            var newSettings = settings.ToDto();

            var newFileName = "New File";
            var newWidth = 1024;
            var newHeight = 512;

            newSettings.FileName = newFileName;
            newSettings.Width = newWidth;
            newSettings.Height = newHeight;

            var service = new CanvasManipulationService();

            // Act
            service.UpdateCanvas(canvas, newSettings);

            // Assert
            Assert.Multiple(
                () => Assert.Equal(canvas.Settings.FileName, newFileName),
                () => Assert.Equal(canvas.Settings.Width, newWidth),
                () => Assert.Equal(canvas.Settings.Height, newHeight)
            );
        }
    }
}
