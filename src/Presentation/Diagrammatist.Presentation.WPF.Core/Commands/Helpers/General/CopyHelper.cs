using Diagrammatist.Presentation.WPF.Core.Managers.Clipboard;

namespace Diagrammatist.Presentation.WPF.Core.Commands.Helpers.General
{
    /// <summary>
    /// Helps to create reusable command in different code spots that has 'Copy' behavior.
    /// </summary>
    public static class CopyHelper
    {
        /// <summary>
        /// Creates a reusable command for copying an item.
        /// </summary>
        /// <typeparam name="T">Type of the item.</typeparam>
        /// <param name="manager">Clipboard manager.</param>
        /// <param name="figure">Target figure.</param>
        /// <returns>Undoable command for cutting the item.</returns>
        public static void Copy<T>(
            IClipboardManager<T> manager,
            T figure) where T : class
        {
            manager.CopyToClipboard(figure);
        }
    }
}
