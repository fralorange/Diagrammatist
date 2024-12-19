namespace Diagrammatist.Presentation.WPF.Framework.Commands.Undoable.Manager
{
    /// <summary>
    /// An interface that defines base methods to manage undoable commands.
    /// </summary>
    public interface IUndoableCommandManager
    {
        /// <summary>
        /// Occurs when <see cref="Execute(IUndoableCommand)"/>, <see cref="Undo"/>, <see cref="Redo"/> or <see cref="Clear"/> used.
        /// </summary>
        /// <remarks>
        /// This event is triggered in assumption that <see cref="CanUndo"/> or <see cref="CanRedo"/> changes.
        /// </remarks>
        event EventHandler<EventArgs>? OperationPerformed;
        /// <summary>
        /// Gets a value indicating whether an undo operation can be performed.
        /// </summary>
        bool CanUndo { get; }
        /// <summary>
        /// Gets a value indicating whether a redo operation can be performed.
        /// </summary>
        bool CanRedo { get; }   
        /// <summary>
        /// Executes command.
        /// </summary>
        /// <param name="command">Target command with execution logic.</param>
        void Execute(IUndoableCommand command);
        /// <summary>
        /// Cancels the effects of an command execution.
        /// </summary>
        void Undo();
        /// <summary>
        /// Performs command execution again.
        /// </summary>
        void Redo();
        /// <summary>
        /// Clears history of all commands inside manager.
        /// </summary>
        void Clear();
        /// <summary>
        /// Updates manager content.
        /// </summary>
        /// <param name="key">Key.</param>
        void UpdateContent(object? key);
        /// <summary>
        /// Deletes manager content
        /// </summary>
        /// <param name="key">Key.</param>
        void DeleteContent(object? key);
    }
}
