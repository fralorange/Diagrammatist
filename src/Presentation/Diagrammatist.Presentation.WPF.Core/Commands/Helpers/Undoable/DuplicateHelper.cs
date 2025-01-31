using Diagrammatist.Presentation.WPF.Core.Commands.Base;

namespace Diagrammatist.Presentation.WPF.Core.Commands.Helpers.Undoable
{
    /// <summary>
    /// Helps to create <see cref="IUndoableCommand"/> in different code spots that has 'Duplicate' behavior.
    /// </summary>
    public static class DuplicateHelper
    {
        /// <summary>
        /// Creates <see cref="IUndoableCommand"/> that duplicates item.
        /// </summary>
        /// <typeparam name="T">Type of item to duplicate.</typeparam>
        /// <param name="collection">Target collection to duplicate into.</param>
        /// <param name="getSelected">Function to get the currently selected item.</param>
        /// <param name="setSelected">Action to update the selected item.</param>
        /// <param name="clone">Function to clone the item.</param>
        /// <returns>An <see cref="IUndoableCommand"/>.</returns>
        public static IUndoableCommand CreateDuplicateCommand<T>(
            ICollection<T> collection,
            Func<T> getSelected,
            Action<T?> setSelected,
            Func<T, T> clone) where T : class
        {
            var prevFigure = getSelected();
            var newFigure = clone(prevFigure);
            
            return CommonUndoableHelper.CreateUndoableCommand(
                () =>
                {
                    collection.Add(newFigure);
                    setSelected(newFigure);
                },
                () =>
                {
                    collection.Remove(newFigure);
                    setSelected(prevFigure);
                });
        }
    }
}
