using Diagrammatist.Presentation.WPF.Core.Models.Connection;
using Diagrammatist.Presentation.WPF.Core.Models.Figures;

namespace Diagrammatist.Presentation.WPF.Core.Managers.Connection
{
    /// <summary>
    /// A class that implements <see cref="IConnectionManager"/>. Manages connections in the app.
    /// </summary>
    public class ConnectionManager : IConnectionManager
    {
        private List<ConnectionModel> _connections = [];

        /// <inheritdoc/>
        public IReadOnlyList<ConnectionModel> Connections => _connections;

        /// <inheritdoc/>
        public void AddConnection(ConnectionModel connection)
        {
            _connections.Add(connection);
        }

        /// <inheritdoc/>
        public void RemoveConnection(ConnectionModel connection)
        {
            _connections.Remove(connection);
        }

        /// <inheritdoc/>
        public ConnectionModel? GetConnection(LineFigureModel line)
        {
            return _connections.FirstOrDefault(x => x.Line == line);
        }

        /// <inheritdoc/>
        public List<ConnectionModel> GetConnections(ShapeFigureModel figure)
        {
            return _connections.Where(c => c.SourceMagneticPoint?.Owner == figure || c.DestinationMagneticPoint?.Owner == figure).ToList();
        }
    }
}
