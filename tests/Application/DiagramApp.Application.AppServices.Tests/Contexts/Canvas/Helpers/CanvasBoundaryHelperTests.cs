using DiagramApp.Application.AppServices.Contexts.Canvas.Helpers;
using DiagramApp.Contracts.Canvas;
using DiagramApp.Contracts.Settings;
using DiagramApp.Domain.Canvas.Constants;
using Moq;

namespace DiagramApp.Application.AppServices.Tests.Contexts.Canvas.Helpers
{
    /// <summary>
    /// Canvas boundary helper tests.
    /// </summary>
    public class CanvasBoundaryHelperTests
    {
        [Theory]
        [InlineData(CanvasZoomConstants.MinZoom)]
        [InlineData(CanvasZoomConstants.DefaultZoom)]
        [InlineData(CanvasZoomConstants.MaxZoom)]
        public void BorderSizeChanges_WhenZooming(double zoom)
        {
            // Arrange
            var canvasWidth = 512;
            var canvasHeight = 512;

            var canvas = new CanvasDto
            {
                Settings = new DiagramSettingsDto { Width = canvasWidth, Height = canvasHeight, FileName = string.Empty, Background = string.Empty, Type = string.Empty },
                Zoom = zoom
            };

            var borderMultiplier = 4;

            var expectedWidth = (int)(canvasWidth * borderMultiplier / zoom);
            var expectedHeight = (int)(canvasHeight * borderMultiplier / zoom);

            // Act
            CanvasBoundaryHelper.UpdateCanvasBounds(canvas);

            // Assert
            Assert.Equal((expectedWidth, expectedHeight), (canvas.ImaginaryWidth, canvas.ImaginaryHeight));
        }
    }
}
