using Diagrammatist.Presentation.WPF.Core.Commands.Base;

namespace Diagrammatist.Presentation.WPF.Core.Commands.Helpers.Undoable
{
    /// <summary>
    /// Helps to create <see cref="IUndoableCommand"/> in different code spots that has 'Paste' behavior.
    /// </summary>
    public static class PasteHelper
    {
        /// <summary>
        /// Creates <see cref="IUndoableCommand"/> that pastes item from clipboard.
        /// </summary>
        /// <typeparam name="T">Type of item to paste.</typeparam>
        /// <param name="collection">Target collection to paste into.</param>
        /// <param name="itemToPaste">Item to paste.</param>
        /// <param name="getSelected">Function to get the currently selected item.</param>
        /// <param name="setSelected">Action to update the selected item.</param>
        /// <returns>An <see cref="IUndoableCommand"/>.</returns>
        public static IUndoableCommand CreatePasteCommand<T>(
            ICollection<T> collection,
            T itemToPaste,
            Func<T?> getSelected,
            Action<T?> setSelected) where T : class
        {
            var previousSelection = getSelected();

            return CommonUndoableHelper.CreateUndoableCommand(
                () =>
                {
                    collection.Add(itemToPaste);
                    setSelected(itemToPaste);
                },
                () =>
                {
                    collection.Remove(itemToPaste);
                    setSelected(previousSelection);
                });
        }

        /// <summary>
        /// Creates <see cref="IUndoableCommand"/> that pastes item from clipboard and puts item on specific position.
        /// </summary>
        /// <typeparam name="T">Type of item to paste.</typeparam>
        /// <param name="collection">Target collection to paste into.</param>
        /// <param name="itemToPaste">Item to paste.</param>
        /// <param name="getSelected">Function to get the currently selected item.</param>
        /// <param name="setSelected">Action to update the selected item.</param>
        /// <param name="setPosition">Action to set position for pasted item.</param>
        /// <param name="destination">Destination pos.</param>
        /// <returns>An <see cref="IUndoableCommand"/>.</returns>
        public static IUndoableCommand CreatePasteCommand<T>(
            ICollection<T> collection,
            T itemToPaste,
            Func<T?> getSelected,
            Action<T?> setSelected,
            Action<T, double, double> setPosition,
            Tuple<double, double> destination) where T : class
        {
            var previousSelection = getSelected();

            return CommonUndoableHelper.CreateUndoableCommand(
                () =>
                {
                    collection.Add(itemToPaste);
                    setPosition(itemToPaste, destination.Item1, destination.Item2);
                    setSelected(itemToPaste);
                },
                () =>
                {
                    collection.Remove(itemToPaste);
                    setSelected(previousSelection);
                });
        }
    }
}
