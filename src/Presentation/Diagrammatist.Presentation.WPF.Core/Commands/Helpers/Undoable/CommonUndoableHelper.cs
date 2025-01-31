using Diagrammatist.Presentation.WPF.Core.Commands.Base;

namespace Diagrammatist.Presentation.WPF.Core.Commands.Helpers.Undoable
{
    /// <summary>
    /// Helps to create common <see cref="IUndoableCommand"/> instances.
    /// </summary>
    public static class CommonUndoableHelper
    {
        /// <summary>
        /// Creates <see cref="IUndoableCommand"/> instance.
        /// </summary>
        /// <param name="doAction">Do <see cref="Action"/> delegate.</param>
        /// <param name="undoAction">Undo <see cref="Action"/> delegate.</param>
        /// <returns><see cref="IUndoableCommand"/> instance.</returns>
        public static IUndoableCommand CreateUndoableCommand(Action doAction, Action undoAction)
            => new UndoableCommand(doAction, undoAction);
    }
}
