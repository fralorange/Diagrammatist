using DiagramApp.Client.ViewModels.Wrappers;

namespace DiagramApp.Application.AppServices.Helpers
{
    public static class UndoableCommandHelper
    {
        public static void ExecuteAction(ObservableCanvas canvas, Action currentAction, Action undoAction)
        {
            canvas.ClearRedoCommands();

            Action action = null!;
            action = () =>
            {
                currentAction.Invoke();
                canvas.AddUndoCommand(() =>
                {
                    undoAction.Invoke();
                    canvas.AddRedoCommand(action);
                });
            };

            action.Invoke();
        }
    }
}
