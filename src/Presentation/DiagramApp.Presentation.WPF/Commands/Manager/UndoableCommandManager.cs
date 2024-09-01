using DiagramApp.Presentation.WPF.Commands.UndoableCommand;

namespace DiagramApp.Presentation.WPF.Commands.Manager
{
    /// <summary>
    /// A class that implements <see cref="IUndoableCommandManager"/>. Manages undoable commands.
    /// </summary>
    public sealed class UndoableCommandManager : IUndoableCommandManager
    {
        private readonly Stack<IUndoableCommand> _undoStack = [];
        private readonly Stack<IUndoableCommand> _redoStack = [];

        /// <summary>
        /// Gets a value indicating whether an undo operation can be performed.
        /// </summary>
        private bool CanUndo => _undoStack.Count > 0;

        /// <summary>
        /// Gets a value indicating whether a redo operation can be performed.
        /// </summary>
        private bool CanRedo => _redoStack.Count > 0;

        /// <inheritdoc/>
        public void Execute(IUndoableCommand command)
        {
            command.Execute(null);
            _undoStack.Push(command);
            _redoStack.Clear();
        }

        /// <inheritdoc/>
        public void Undo()
        {
            if (CanUndo)
            {
                var command = _undoStack.Pop();
                command.Undo();
                _redoStack.Push(command);
            }
        }

        /// <inheritdoc/>
        public void Redo()
        {
            if (CanRedo)
            {
                var command = _redoStack.Pop();
                command.Execute(null);
                _undoStack.Push(command);
            }
        }

        /// <inheritdoc/>
        public void Clear()
        {
            _undoStack.Clear();
            _redoStack.Clear();
        }
    }
}
