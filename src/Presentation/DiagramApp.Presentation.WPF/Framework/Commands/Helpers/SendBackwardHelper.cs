using DiagramApp.Contracts.Figures;
using DiagramApp.Presentation.WPF.Framework.Commands.UndoableCommand;

namespace DiagramApp.Presentation.WPF.Framework.Commands.Helpers
{
    public static class SendBackwardHelper
    {
        public static IUndoableCommand CreateSendBackwardCommand(FigureDto target)
        {
            var action = new Action(() =>
            {
                if (target.ZIndex > 1)
                {
                    target.ZIndex--;
                }
            });

            var undo = new Action(() =>
            {
                target.ZIndex++;
            });

            return new UndoableCommand.UndoableCommand(action, undo);
        }
    }
}
