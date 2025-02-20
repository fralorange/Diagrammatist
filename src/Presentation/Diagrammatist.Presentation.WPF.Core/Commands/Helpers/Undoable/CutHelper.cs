using Diagrammatist.Presentation.WPF.Core.Commands.Base;
using Diagrammatist.Presentation.WPF.Core.Commands.Helpers.General;
using Diagrammatist.Presentation.WPF.Core.Services.Clipboard;

namespace Diagrammatist.Presentation.WPF.Core.Commands.Helpers.Undoable
{
    /// <summary>
    /// Helps to create <see cref="IUndoableCommand"/> in different code spots that has 'Cut' behavior.
    /// </summary>
    public static class CutHelper
    {
        /// <summary>
        /// Creates an undoable command for cutting an item.
        /// </summary>
        /// <typeparam name="T">Type of the item.</typeparam>
        /// <param name="manager">Clipboard manager used in copy command.</param>
        /// <param name="collection">Target collection.</param>
        /// <param name="getSelected">Function to get current selection.</param>
        /// <param name="setSelected">Action to set the current selection.</param>
        /// <returns>Undoable command for cutting the item.</returns>
        public static IUndoableCommand CreateCutCommand<T>(
            IClipboardService<T> manager,
            ICollection<T> collection,
            Func<T> getSelected,
            Action<T?> setSelected) where T : class
        {
            var previousSelection = getSelected();

            CopyHelper.Copy(manager, previousSelection);

            return CommonUndoableHelper.CreateUndoableCommand(
                () =>
                {
                    collection.Remove(getSelected());
                    setSelected(null);
                },
                () =>
                {
                    collection.Add(previousSelection);
                    setSelected(previousSelection);
                });
        }
    }
}
