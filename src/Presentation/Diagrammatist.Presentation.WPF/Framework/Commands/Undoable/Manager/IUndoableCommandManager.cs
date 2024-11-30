namespace Diagrammatist.Presentation.WPF.Framework.Commands.Undoable.Manager
{
    /// <summary>
    /// An interface that defines base methods to manage undoable commands.
    /// </summary>
    public interface IUndoableCommandManager
    {
        /// <summary>
        /// Executes command.
        /// </summary>
        /// <param name="command">Target command with execution logic.</param>
        public void Execute(IUndoableCommand command);
        /// <summary>
        /// Cancels the effects of an command execution.
        /// </summary>
        public void Undo();
        /// <summary>
        /// Performs command execution again.
        /// </summary>
        public void Redo();
        /// <summary>
        /// Clears history of all commands inside manager.
        /// </summary>
        public void Clear();
        /// <summary>
        /// Updates manager content.
        /// </summary>
        /// <param name="key">Key.</param>
        public void UpdateContent(object? key);
    }
}
