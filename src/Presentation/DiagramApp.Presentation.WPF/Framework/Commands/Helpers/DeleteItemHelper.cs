using DiagramApp.Presentation.WPF.Framework.Commands.UndoableCommand;

namespace DiagramApp.Presentation.WPF.Framework.Commands.Helpers
{
    public static class DeleteItemHelper
    {
        public static IUndoableCommand CreateDeleteItemCommand<T>(ICollection<T>? collection, T target) where T : class
        {
            var action = new Action(() =>
            {
                collection?.Remove(target);
            });

            var undo = new Action(() =>
            {
                collection?.Add(target);
            });

            return new UndoableCommand.UndoableCommand(action, undo);
        }
    }
}
