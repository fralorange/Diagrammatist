﻿using System.Windows.Input;

namespace Diagrammatist.Presentation.WPF.Core.Commands.Base
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
