namespace DiagramApp.Domain.Figures
{
    /// <summary>
    /// Figure collection.
    /// </summary>
    public class FigureCollection
    {
        private readonly List<Figure> _figures = [];

        /// <summary>
        /// Returns figure collection.
        /// </summary>
        /// <returns><see cref="List{T}"/> of figures</returns>
        public List<Figure> Figures { get { return _figures; } }

        /// <summary>
        /// Returns figure by predicate.
        /// </summary>
        /// <param name="predicate">Predicate.</param>
        /// <returns><see cref="Figure"/></returns>
        public Figure? GetFigure(Func<Figure, bool> predicate)
        {
            return _figures.FirstOrDefault(predicate);
        }

        /// <summary>
        /// Adds figure to collection.
        /// </summary>
        /// <param name="figure">Target figure.</param>
        public void AddFigure(Figure figure)
        {
            _figures.Add(figure);
        }

        /// <summary>
        /// Removes figure from collection.
        /// </summary>
        /// <param name="figure">Target figure.</param>
        public void RemoveFigure(Figure figure) 
        {
            _figures.Remove(figure);
        }

        /// <summary>
        /// Clears figures from collection.
        /// </summary>
        public void ClearFigures()
        {
            _figures.Clear();
        }
    }
}
