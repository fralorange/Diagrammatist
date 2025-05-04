using Diagrammatist.Presentation.WPF.Core.Models.Connection;
using Diagrammatist.Presentation.WPF.Core.Models.Figures;
using Diagrammatist.Presentation.WPF.Core.Services.Connection;

namespace Diagrammatist.Presentation.WPF.Core.Helpers
{
    /// <summary>
    /// A class helper that helps to perform connection operations.
    /// </summary>
    public static class ConnectionHelper
    {
        /// <summary>
        /// Gets connection for line figure model.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connections"></param>
        /// <param name="target"></param>
        /// <param name="connectionService"></param>
        /// <returns></returns>
        public static ConnectionModel? GetConnection<T>(ICollection<ConnectionModel> connections, T target, IConnectionService connectionService)
        {
            if (target is LineFigureModel line)
            {
                return connectionService.GetConnection(connections, line);
            }

            return null;
        }
    }
}
