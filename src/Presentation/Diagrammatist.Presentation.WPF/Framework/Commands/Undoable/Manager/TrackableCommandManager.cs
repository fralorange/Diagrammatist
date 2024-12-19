
namespace Diagrammatist.Presentation.WPF.Framework.Commands.Undoable.Manager
{
    /// <summary>
    /// A class that implements <see cref="ITrackableCommandManager"/>. Manages tracking in undoable commands.
    /// </summary>
    public class TrackableCommandManager : ITrackableCommandManager
    {
        private readonly IUndoableCommandManager _innerManager;

        /// <inheritdoc/>
        public event EventHandler<EventArgs>? StateChanged;

        /// <inheritdoc/>
        public bool HasChanges { get; private set; }

        public TrackableCommandManager(IUndoableCommandManager innerManager)
        {
            _innerManager = innerManager;
        }

        /// <inheritdoc/>
        public void MarkSaved() => ChangeState(false);

        /// <inheritdoc/>
        public void Execute(IUndoableCommand command)
        {
            _innerManager.Execute(command);
            ChangeState(true);
        }

        /// <inheritdoc/>
        public void Redo()
        {
            _innerManager.Redo();
            ChangeState(true);
        }

        /// <inheritdoc/>
        public void Undo()
        {
            _innerManager.Undo();
            ChangeState(true);
        }

        /// <inheritdoc/>
        public void Clear()
        {
            _innerManager.Clear();
            ChangeState(false);
        }

        /// <inheritdoc/>
        public void UpdateContent(object? key)
        {
            _innerManager.UpdateContent(key);
        }

        private void ChangeState(bool state)
        {
            HasChanges = state;
            StateChanged?.Invoke(null, new());
        }
    }
}
