using Diagrammatist.Presentation.WPF.Core.Commands.Base;
using Diagrammatist.Presentation.WPF.Core.Managers.Connection;
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
        public static IUndoableCommand CreateDeleteItemCommand<T>(ICollection<T> collection, T target, IConnectionManager connectionManager)
            where T : FigureModel
        {
            var connection = GetConnection(target, connectionManager);

            return CommonUndoableHelper.CreateUndoableCommand(
                () =>
                {
                    collection.Remove(target);
                    if (connection is not null) connectionManager.RemoveConnection(connection); 
                },
                () =>
                {
                    collection.Add(target);
                    if (connection is not null) connectionManager.AddConnection(connection);
                }
            );
        }

        private static ConnectionModel? GetConnection<T>(T target, IConnectionManager connectionManager)
        {
            if (target is LineFigureModel line)
            {
                return connectionManager.GetConnection(line);
            }

            return null;
        }
    }
}
