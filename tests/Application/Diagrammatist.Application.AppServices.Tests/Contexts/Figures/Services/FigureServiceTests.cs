using Diagrammatist.Application.AppServices.Figures.Repositories;
using Diagrammatist.Application.AppServices.Figures.Services;
using Diagrammatist.Domain.Figures;
using Moq;

namespace Diagrammatist.Application.AppServices.Tests.Contexts.Figures.Services
{
    /// <summary>
    /// A figure service tests class.
    /// </summary>
    public class FigureServiceTests
    {
        [Fact]
        public async Task GetAsync_ReturnsDictionariesSuccessfully()
        {
            // Arrange
            var dictionaryMock = new Dictionary<string, List<Figure>>
            {
                { "Path", [new ShapeFigure() { Data = ["M0,0 L100,0 L100,100, L0,100 Z"] }] },
                { "Points", [new LineFigure() { Points = [new(0, 0), new(50, 50)] }] },
                { "Text", [new TextFigure() { Text = "Lorem" }] }
            };

            var repositoryMock = new Mock<IFigureRepository>();
            repositoryMock.Setup(r => r.GetAsync()).ReturnsAsync(dictionaryMock);

            IFigureService service = new FigureService(repositoryMock.Object);

            // Act
            var figures = await service.GetAsync();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.NotEmpty(dictionaryMock);
                Assert.IsType<Dictionary<string, List<Figure>>>(figures);
            });
        }
    }
}
