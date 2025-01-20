using Diagrammatist.Presentation.WPF.Core.Commands.Base;
using System.Collections.Concurrent;

namespace Diagrammatist.Presentation.WPF.Core.Commands.Managers
{
    /// <summary>
    /// A class that implements <see cref="IUndoableCommandManager"/>. Manages undoable commands.
    /// </summary>
    public sealed class UndoableCommandManager : IUndoableCommandManager
    {
        private readonly ConcurrentDictionary<object, Stack<IUndoableCommand>> _undoLayer = [];
        private readonly ConcurrentDictionary<object, Stack<IUndoableCommand>> _redoLayer = [];
        private object? CurrentKey { get; set; }

        /// <inheritdoc/>
        public event EventHandler<EventArgs>? OperationPerformed;

        /// <inheritdoc/>
        public bool CanUndo
        {
            get
            {
                if (CurrentKey is not null && _undoLayer.TryGetValue(CurrentKey, out var undoStack))
                {
                    return undoStack.Count > 0;
                }
                return false;
            }
        }

        /// <inheritdoc/>
        public bool CanRedo
        {
            get
            {
                if (CurrentKey is not null && _redoLayer.TryGetValue(CurrentKey, out var redoStack))
                {
                    return redoStack.Count > 0;
                }
                return false;
            }
        }

        /// <inheritdoc/>
        public void Execute(IUndoableCommand command)
        {
            if (command is null)
                return;

            if (CurrentKey is not null && _undoLayer.TryGetValue(CurrentKey, out var undoStack) && _redoLayer.TryGetValue(CurrentKey, out var redoStack))
            {
                command.Execute(null);
                undoStack.Push(command);
                redoStack.Clear();

                PerformOperation();
            }
        }

        /// <inheritdoc/>
        public void Undo()
        {
            if (CanUndo && _undoLayer.TryGetValue(CurrentKey!, out var undoStack) && _redoLayer.TryGetValue(CurrentKey!, out var redoStack))
            {
                var command = undoStack.Pop();
                command.Undo();
                redoStack.Push(command);

                PerformOperation();
            }
        }

        /// <inheritdoc/>
        public void Redo()
        {
            if (CanRedo && _undoLayer.TryGetValue(CurrentKey!, out var undoStack) && _redoLayer.TryGetValue(CurrentKey!, out var redoStack))
            {
                var command = redoStack.Pop();
                command.Execute(null);
                undoStack.Push(command);

                PerformOperation();
            }
        }

        /// <inheritdoc/>
        public void Clear()
        {
            if (CurrentKey is not null && _undoLayer.TryGetValue(CurrentKey!, out var undoStack) && _redoLayer.TryGetValue(CurrentKey!, out var redoStack))
            {
                undoStack.Clear();
                redoStack.Clear();

                PerformOperation();
            }
        }

        public void UpdateContent(object? key)
        {
            CurrentKey = key;

            if (CurrentKey is not null)
            {
                _undoLayer.TryAdd(CurrentKey, []);
                _redoLayer.TryAdd(CurrentKey, []);
            }

            PerformOperation();
        }

        public void DeleteContent(object? key)
        {
            CurrentKey = null;

            if (key is not null)
            {
                _undoLayer.Remove(key, out var _);
                _redoLayer.Remove(key, out var _);
            }

            PerformOperation();
        }

        private void PerformOperation()
        {
            OperationPerformed?.Invoke(this, new());
        }
    }
}
