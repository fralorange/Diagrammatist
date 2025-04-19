using Diagrammatist.Presentation.WPF.Core.Foundation.Base.UndoableCommand;
using System.Collections.Concurrent;

namespace Diagrammatist.Presentation.WPF.Core.Managers.Command
{
    /// <summary>
    /// A class that implements <see cref="ITrackableCommandManager"/>. Manages tracking in undoable commands.
    /// </summary>
    public class TrackableCommandManager : ITrackableCommandManager
    {
        private readonly IUndoableCommandManager _innerManager;

        private readonly ConcurrentDictionary<object, bool> _changesLayer = [];

        private object? CurrentKey { get; set; }

        /// <inheritdoc/>
        public event EventHandler<EventArgs>? StateChanged;
        /// <inheritdoc/>
        public event EventHandler<EventArgs>? OperationPerformed;

        /// <inheritdoc/>
        public bool HasGlobalChanges => _changesLayer.Any(kv => kv.Value);
        /// <inheritdoc/>
        public bool HasChanges { get; private set; }
        /// <inheritdoc/>
        public bool CanUndo => _innerManager.CanUndo;
        /// <inheritdoc/>
        public bool CanRedo => _innerManager.CanRedo;

        public TrackableCommandManager(IUndoableCommandManager innerManager)
        {
            _innerManager = innerManager;
            _innerManager.OperationPerformed += (sender, args) => OperationPerformed?.Invoke(this, args);
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

            CurrentKey = key;

            if (CurrentKey is not null && !_changesLayer.TryAdd(CurrentKey, false) && _changesLayer.TryGetValue(CurrentKey, out var changes))
            {
                HasChanges = changes;
            }
            else
            {
                HasChanges = false;
            }
        }

        public void DeleteContent(object? key)
        {
            _innerManager.DeleteContent(key);

            CurrentKey = null;

            if (key is not null)
            {
                _changesLayer.Remove(key, out var _);
            }
        }

        private void ChangeState(bool state)
        {
            HasChanges = state;

            if (CurrentKey is not null && _changesLayer.ContainsKey(CurrentKey))
            {
                _changesLayer[CurrentKey] = state;
            }

            StateChanged?.Invoke(null, new());
        }
    }
}
