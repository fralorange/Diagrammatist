using Diagrammatist.Presentation.WPF.Core.Models.Connection;
using Diagrammatist.Presentation.WPF.Core.Models.Figures;

namespace Diagrammatist.Presentation.WPF.Core.Services.Connection
{
    /// <summary>
    /// An interface for managing connections.
    /// </summary>
    public interface IConnectionService
    {
        /// <summary>
        /// Adds a new connection.
        /// </summary>
        /// <param name="connections">Connections.</param>
        /// <param name="connection">Target connection.</param>
        public void AddConnection(ICollection<ConnectionModel> connections, ConnectionModel connection);
        /// <summary>
        /// Removes existing connection.
        /// </summary>
        /// <param name="connection">Target connection.</param>
        public void RemoveConnection(ICollection<ConnectionModel> connections, ConnectionModel connection);
        /// <summary>
        /// Gets connection by line.
        /// </summary>
        /// <param name="line">Connection line.</param>
        /// <returns><see cref="ConnectionModel"/>.</returns>
        public ConnectionModel? GetConnection(ICollection<ConnectionModel> connections, LineFigureModel line);
        /// <summary>
        /// Gets all connections that target figure has.
        /// </summary>
        /// <param name="figure">Target figure.</param>
        /// <returns>All <see cref="ConnectionModel"/> that figure has, packed in <see cref="ICollection{T}{T}"/>.</returns>
        public List<ConnectionModel> GetConnections(ICollection<ConnectionModel> connections, ShapeFigureModel figure);
    }
}
