using DiagramApp.Application.AppServices.Contexts.Canvas.Services;
using DiagramApp.Contracts.Canvas;
using DiagramApp.Contracts.Settings;
using DiagramApp.Domain.Canvas.Constants;

namespace DiagramApp.Application.AppServices.Tests.Contexts.Canvas.Services
{
    /// <summary>
    /// Canvas interaction service tests.
    /// </summary>
    public class CanvasInteractionServiceTests
    {
        [Theory]
        [InlineData(754, 648)]
        [InlineData(1049, 1745)]
        [InlineData(500, 1456)]
        [InlineData(-532, 510)]
        public void PanCanvas_ChangesScreenOffsetAndEnsuresItWithinTheBorder(double newX, double newY)
        {
            // Arrange
            var canvas = new CanvasDto
            {
                ImaginaryWidth = 1024,
                ImaginaryHeight = 1024,
                Settings = new DiagramSettingsDto { Width = 512, Height = 512, FileName = string.Empty, Background = string.Empty, Type = string.Empty },
                Zoom = 1,
                ScreenOffset = new() { X = 0, Y = 0 },
            };

            var service = new CanvasInteractionService();

            // Act
            service.Pan(canvas, newX, newY);

            var actualBorderConditionX = canvas.ScreenOffset.X >= 0 && canvas.ScreenOffset.X <= canvas.ImaginaryWidth;
            var actualBorderConditionY = canvas.ScreenOffset.Y >= 0 && canvas.ScreenOffset.Y <= canvas.ImaginaryHeight;
            var actualEqualityConditionX = canvas.ScreenOffset.X == newX || canvas.ScreenOffset.X == canvas.ImaginaryWidth || canvas.ScreenOffset.X == 0;
            var actualEqualityConditionY = canvas.ScreenOffset.Y == newY || canvas.ScreenOffset.Y == canvas.ImaginaryHeight || canvas.ScreenOffset.Y == 0;

            // Assert

            Assert.True(actualBorderConditionX && actualBorderConditionY && actualEqualityConditionX && actualEqualityConditionY);
        }

        [Theory]
        [InlineData(true, 1)]
        [InlineData(true, 2)]
        [InlineData(false, 1)]
        [InlineData(false, 0.5)]
        public void ZoomCanvas_ZoomAccordingToMinMaxConstraint(bool IsZoomIn, double currentZoom)
        {
            // Arrange
            var canvas = new CanvasDto
            {
                ImaginaryWidth = 1024,
                ImaginaryHeight = 1024,
                Settings = new DiagramSettingsDto { Width = 512, Height = 512, FileName = string.Empty, Background = string.Empty, Type = string.Empty },
                Zoom = currentZoom,
                ScreenOffset = new() { X = 0, Y = 0 },
            };

            var service = new CanvasInteractionService();

            // Act
            service.Zoom(canvas, IsZoomIn);

            // Assert
            Assert.True(canvas.Zoom >= CanvasZoomConstants.MinZoom || canvas.Zoom <= CanvasZoomConstants.MaxZoom);
        }

        [Fact]
        public void ZoomInCanvasWithMouseParams_ZoomsAccordingToMouseParams()
        {
            // Arrange
            var canvas = new CanvasDto
            {
                ImaginaryWidth = 1024,
                ImaginaryHeight = 1024,
                Settings = new DiagramSettingsDto { Width = 512, Height = 512, FileName = string.Empty, Background = string.Empty, Type = string.Empty },
                Zoom = 1,
                ScreenOffset = new() { X = 0, Y = 0 },
            };

            var service = new CanvasInteractionService();

            var mouseX = 200;
            var mouseY = 150;

            // Act
            service.Zoom(canvas, true, mouseX, mouseY);

            var expectedX = 20;
            var expectedY = 15;

            // Assert

            Assert.Equal((expectedX, expectedY), (canvas.ScreenOffset.X, canvas.ScreenOffset.Y));
        }

        [Fact]
        public void ZoomOutCanvasWithMouseParams_ZoomsAccordingToMouseParams()
        {
            // Arrange
            var canvas = new CanvasDto
            {
                ImaginaryWidth = 1024,
                ImaginaryHeight = 1024,
                Settings = new DiagramSettingsDto { Width = 512, Height = 512, FileName = string.Empty, Background = string.Empty, Type = string.Empty },
                Zoom = 2,
                ScreenOffset = new() { X = 100, Y = 100 },
            };

            var service = new CanvasInteractionService();

            var mouseX = 200;
            var mouseY = 150;

            // Act
            service.Zoom(canvas, false, mouseX, mouseY);

            var expectedX = 130;
            var expectedY = 75;

            // Assert

            Assert.Equal((expectedX, expectedY), (canvas.ScreenOffset.X, canvas.ScreenOffset.Y));
        }
    }
}
