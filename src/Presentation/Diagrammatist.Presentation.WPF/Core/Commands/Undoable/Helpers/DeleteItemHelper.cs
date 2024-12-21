using Diagrammatist.Presentation.WPF.Core.Commands.Undoable;

namespace Diagrammatist.Presentation.WPF.Core.Commands.Undoable.Helpers
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
        public static IUndoableCommand CreateDeleteItemCommand<T>(ICollection<T>? collection, T target) where T : class
        {
            return CommonUndoableHelper.CreateUndoableCommand(
                () => collection?.Remove(target),
                () => collection?.Add(target)
            );
        }
    }
}
