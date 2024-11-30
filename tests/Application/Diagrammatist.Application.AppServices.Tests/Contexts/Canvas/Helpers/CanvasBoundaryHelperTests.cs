using DiagramApp.Application.AppServices.Contexts.Canvas.Helpers;
using DiagramApp.Contracts.Canvas;
using DiagramApp.Contracts.Settings;
using DiagramApp.Domain.Canvas.Constants;
using Moq;
using System.Drawing;

namespace DiagramApp.Application.AppServices.Tests.Contexts.Canvas.Helpers
{
    /// <summary>
    /// Canvas boundary helper tests.
    /// </summary>
    public class CanvasBoundaryHelperTests
    {
        [Theory]
        [InlineData(0.5)]
        [InlineData(CanvasZoomConstants.DefaultZoom)]
        [InlineData(2)]
        public void BorderSizeChanges_WhenZooming(double zoom)
        {
            // Arrange
            var canvasWidth = 512;
            var canvasHeight = 512;

            var canvas = new CanvasDto
            {
                Settings = new DiagramSettingsDto { Width = canvasWidth, Height = canvasHeight, FileName = string.Empty, Background = Color.DimGray, Type = default },
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
