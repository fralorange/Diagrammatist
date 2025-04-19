using System.Windows.Input;

namespace Diagrammatist.Presentation.WPF.Core.Foundation.Base.UndoableCommand
{
    /// <summary>
    /// An interface expanding <see cref="ICommand"/> with undo behavior.
    /// </summary>
    public interface IUndoableCommand : ICommand
    {
        /// <summary>
        /// Undoes previous execution.
        /// </summary>
        void Undo();
    }
}
