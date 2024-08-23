using DiagramApp.Application.AppServices.Contexts.Canvas.Services;
using DiagramApp.Contracts.Canvas;
using DiagramApp.Domain.Settings;
using DiagramApp.Infrastructure.ComponentRegistrar.Contexts.Settings.Mappers;
using FluentAssertions;

namespace DiagramApp.Application.AppServices.Tests.Contexts.Canvas.Services
{
    /// <summary>
    /// Canvas manipulation service tests.
    /// </summary>
    public class CanvasManipulationServiceTests
    {
        [Fact]
        public void CreateCanvas_CanvasCreatedWithDefaultSettingsAndBorders()
        {
            // Arrange
            var settings = new DiagramSettings();

            var service = new CanvasManipulationService();

            var expectedCanvas = new CanvasDto
            {
                ImaginaryWidth = 2048,
                ImaginaryHeight = 2048,
                Settings = settings.ToDto(),
                Zoom = 1,
                Rotation = 0,
                ScreenOffset = new() { X = default, Y = default },
                GridSpacing = 10,
                IsGridVisible = true,
                Figures = [],
            };

            // Act
            var canvas = service.CreateCanvas(settings);

            // Assert
            canvas.Should().BeEquivalentTo(expectedCanvas);
        }
    }
}
