namespace Diagrammatist.Presentation.WPF.Framework.Commands.Undoable.Manager
{
    /// <summary>
    /// An interface that defines base methods and properties to decorate undoable commands with tracking.
    /// </summary>
    public interface ITrackableCommandManager : IUndoableCommandManager
    {
        /// <summary>
        /// Occurs when <see cref="HasChanges"/> changes.
        /// </summary>
        /// <remarks>
        /// This event is triggered when app changes flushes or appears.
        /// </remarks>
        event EventHandler<EventArgs>? StateChanged;
        /// <summary>
        /// Gets or sets 'has changes' flag.
        /// </summary>
        /// <remarks>
        /// This property used to determine whether app has changes or not.
        /// </remarks>
        bool HasChanges { get; }
        /// <summary>
        /// Marks app that all changes have been saved.
        /// </summary>
        void MarkSaved();
    }
}
