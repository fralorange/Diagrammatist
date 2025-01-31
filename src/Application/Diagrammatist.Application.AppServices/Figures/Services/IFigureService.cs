using Diagrammatist.Domain.Figures;

namespace Diagrammatist.Application.AppServices.Figures.Services
{
    /// <summary>
    /// An interface for figure service.
    /// </summary>
    public interface IFigureService
    {
        /// <summary>
        /// Gets figures from the repository.
        /// </summary>
        /// <returns>A hash map with <see cref="string"/> key and collection of <see cref="Figure"/> as value.</returns>
        public Task<Dictionary<string, List<Figure>>> GetAsync();
    }
}
