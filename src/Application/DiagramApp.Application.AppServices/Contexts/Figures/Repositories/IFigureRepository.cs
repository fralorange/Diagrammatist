using DiagramApp.Domain.Figures;

namespace DiagramApp.Application.AppServices.Contexts.Figures.Repositories
{
    /// <summary>
    /// An interface for figure repository.
    /// </summary>
    public interface IFigureRepository
    {
        /// <summary>
        /// Gets figures from data storage.
        /// </summary>
        /// <returns>A hash map with <see cref="string"/> key and collection of <see cref="Figures"/> as value.</returns>
        public Task<Dictionary<string, List<Figure>>> GetAsync();
    }
}
