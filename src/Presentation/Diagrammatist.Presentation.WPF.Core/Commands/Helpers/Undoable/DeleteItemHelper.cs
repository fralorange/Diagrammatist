using Diagrammatist.Presentation.WPF.Core.Commands.Base;
using Diagrammatist.Presentation.WPF.Core.Services.Connection;
using Diagrammatist.Presentation.WPF.Core.Models.Connection;
using Diagrammatist.Presentation.WPF.Core.Models.Figures;

namespace Diagrammatist.Presentation.WPF.Core.Commands.Helpers.Undoable
{
    /// <summary>
    /// Helps to create <see cref="IUndoableCommand"/> in different code spots.
    /// </summary>
    public static class DeleteItemHelper
    {
        /// <summary>
        /// Creates <see cref="IUndoableCommand"/> that deletes item from collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">Target collection.</param>
        /// <param name="target">Target item.</param>
        /// <returns><see cref="IUndoableCommand"/> instance.</returns>
        public static IUndoableCommand CreateDeleteItemCommand<T>(ICollection<T> collection, T target, IConnectionService connectionService, ICollection<ConnectionModel> connections)
            where T : FigureModel
        {
            var connection = GetConnection(connections, target, connectionService);

            return CommonUndoableHelper.CreateUndoableCommand(
                () =>
                {
                    collection.Remove(target);
                    if (connection is not null) connectionService.RemoveConnection(connections, connection); 
                },
                () =>
                {
                    collection.Add(target);
                    if (connection is not null) connectionService.AddConnection(connections, connection);
                }
            );
        }

        private static ConnectionModel? GetConnection<T>(ICollection<ConnectionModel> connections, T target, IConnectionService connectionService)
        {
            if (target is LineFigureModel line)
            {
                return connectionService.GetConnection(connections, line);
            }

            return null;
        }
    }
}
