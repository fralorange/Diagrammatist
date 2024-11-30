using DiagramApp.Application.AppServices.Contexts.Figures.Repositories;
using DiagramApp.Domain.Figures;
using DiagramApp.Infrastructure.DataAccess.Contexts.Figures.Repositories;

namespace DiagramApp.Infrastructure.DataAccess.Tests.Contexts.Figures.Repositories
{
    /// <summary>
    /// A figure repository tests class.
    /// </summary>
    public class FigureRepositoryTests
    {
        [Fact]
        public async Task GetFiguresFromJson_ReturnsSuccessfully()
        {
            // Arrange
            IFigureRepository repository = new JsonFigureRepository();

            // Act
            var figures = await repository.GetAsync();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.NotEmpty(figures);
                foreach (var keyValueFigures in figures)
                {
                    Assert.NotEmpty(keyValueFigures.Value);
                    foreach (var figure in keyValueFigures.Value)
                    {
                        Assert.IsAssignableFrom<Figure>(figure);
                    }
                }
            });
        }
    }
}
