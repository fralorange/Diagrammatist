using Diagrammatist.Application.AppServices.Canvas.Helpers;
using Diagrammatist.Domain.Canvas;
using Diagrammatist.Domain.Canvas.Constants;
using System.Drawing;
using CanvasEntity = Diagrammatist.Domain.Canvas.Canvas;

namespace Diagrammatist.Application.AppServices.Tests.Contexts.Canvas.Helpers.Unit
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

            var canvas = new CanvasEntity
            {
                Settings = new Settings { Width = canvasWidth, Height = canvasHeight, FileName = string.Empty, Background = Color.DimGray, Type = default },
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
