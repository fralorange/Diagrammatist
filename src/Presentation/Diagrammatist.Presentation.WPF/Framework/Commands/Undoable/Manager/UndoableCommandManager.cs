using System.Runtime.CompilerServices;

namespace Diagrammatist.Presentation.WPF.Framework.Commands.Undoable.Manager
{
    /// <summary>
    /// A class that implements <see cref="IUndoableCommandManager"/>. Manages undoable commands.
    /// </summary>
    public sealed class UndoableCommandManager : IUndoableCommandManager
    {
        private readonly ConditionalWeakTable<object, Stack<IUndoableCommand>> _undoLayer = [];
        private readonly ConditionalWeakTable<object, Stack<IUndoableCommand>> _redoLayer = [];

        private object? CurrentKey { get; set; }

        /// <summary>
        /// Gets a value indicating whether an undo operation can be performed.
        /// </summary>
        private bool CanUndo
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

        /// <summary>
        /// Gets a value indicating whether a redo operation can be performed.
        /// </summary>
        private bool CanRedo
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
            }
        }

        /// <inheritdoc/>
        public void Clear()
        {
            if (CurrentKey is not null && _undoLayer.TryGetValue(CurrentKey!, out var undoStack) && _redoLayer.TryGetValue(CurrentKey!, out var redoStack))
            {
                undoStack.Clear();
                redoStack.Clear();
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
        }
    }
}
