namespace Diagrammatist.Presentation.WPF.Core.Commands.Undoable
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UndoableCommand"/> class that can undo previous execution.
    /// </summary>
    public sealed class UndoableCommand : IUndoableCommand
    {
        private readonly Action _execute;
        private readonly Action _undo;

        /// <inheritdoc/>
        public event EventHandler? CanExecuteChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="UndoableCommand"/> class.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <param name="undo">The undo behavior logic.</param>
        public UndoableCommand(Action execute, Action undo)
        {
            ArgumentNullException.ThrowIfNull(execute);
            ArgumentNullException.ThrowIfNull(undo);

            _execute = execute;
            _undo = undo;
        }

        /// <inheritdoc/>
        public bool CanExecute(object? parameter = null)
        {
            return true;
        }

        /// <inheritdoc/>
        public void Execute(object? parameter = null)
        {
            _execute();
        }

        /// <inheritdoc/>
        public void Undo()
        {
            _undo();
        }
    }
}
