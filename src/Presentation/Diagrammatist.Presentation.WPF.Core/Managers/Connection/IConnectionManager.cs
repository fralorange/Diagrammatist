using Diagrammatist.Presentation.WPF.Core.Models.Connection;
using Diagrammatist.Presentation.WPF.Core.Models.Figures;

namespace Diagrammatist.Presentation.WPF.Core.Managers.Connection
{
    /// <summary>
    /// An interface for managing connections.
    /// </summary>
    public interface IConnectionManager
    {
        /// <summary>
        /// Gets read-only connections list.
        /// </summary>
        public IReadOnlyList<ConnectionModel> Connections { get; }
        /// <summary>
        /// Adds a new connection.
        /// </summary>
        /// <param name="connection">Target connection.</param>
        public void AddConnection(ConnectionModel connection);
        /// <summary>
        /// Removes existing connection.
        /// </summary>
        /// <param name="connection">Target connection.</param>
        public void RemoveConnection(ConnectionModel connection);
        /// <summary>
        /// Gets connection by line.
        /// </summary>
        /// <param name="line">Connection line.</param>
        /// <returns><see cref="ConnectionModel"/>.</returns>
        public ConnectionModel? GetConnection(LineFigureModel line);
        /// <summary>
        /// Gets all connections that target figure has.
        /// </summary>
        /// <param name="figure">Target figure.</param>
        /// <returns>All <see cref="ConnectionModel"/> that figure has, packed in <see cref="List{T}"/>.</returns>
        public List<ConnectionModel> GetConnections(ShapeFigureModel figure);
    }
}
