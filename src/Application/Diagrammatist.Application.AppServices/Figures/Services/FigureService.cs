using Diagrammatist.Application.AppServices.Figures.Repositories;
using Diagrammatist.Domain.Figures;

namespace Diagrammatist.Application.AppServices.Figures.Services
{
    /// <summary>
    /// A implemented class from <see cref="FigureService"/>. A service that gets figures data from repository.
    /// </summary>
    public class FigureService : IFigureService
    {
        private IFigureRepository _repository;

        /// <summary>
        /// Initializes new figure service.
        /// </summary>
        /// <param name="repository">A <see cref="IFigureRepository"/> implementation.</param>
        public FigureService(IFigureRepository repository)
        {
            _repository = repository;
        }

        /// <inheritdoc/>
        public async Task<Dictionary<string, List<Figure>>> GetAsync()
        {
            var figures = await _repository.GetAsync();

            return figures.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value);
        }
    }
}
