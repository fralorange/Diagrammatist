using Diagrammatist.Presentation.WPF.Core.Models.Connection;
using Diagrammatist.Presentation.WPF.Core.Models.Figures;

namespace Diagrammatist.Presentation.WPF.Core.Services.Connection
{
    /// <summary>
    /// A class that implements <see cref="IConnectionService"/>. Provides connection management logic.
    /// </summary>
    public class ConnectionService : IConnectionService
    {
        /// <inheritdoc/>
        public void AddConnection(ICollection<ConnectionModel> connections, ConnectionModel connection)
        {
            connections.Add(connection);
        }

        /// <inheritdoc/>
        public void RemoveConnection(ICollection<ConnectionModel> connections, ConnectionModel connection)
        {
            connections.Remove(connection);
        }

        /// <inheritdoc/>
        public ConnectionModel? GetConnection(ICollection<ConnectionModel> connections, LineFigureModel line)
        {
            return connections.FirstOrDefault(x => x.Line == line);
        }

        /// <inheritdoc/>
        public List<ConnectionModel> GetConnections(ICollection<ConnectionModel> connections, ShapeFigureModel figure)
        {
            return connections
                .Where(c => c.SourceMagneticPoint?.Owner == figure || c.DestinationMagneticPoint?.Owner == figure)
                .ToList();
        }
    }
}
