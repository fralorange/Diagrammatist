using DiagramApp.Contracts.Figures;
using DiagramApp.Presentation.WPF.Framework.Commands.UndoableCommand;

namespace DiagramApp.Presentation.WPF.Framework.Commands.Helpers
{
    public static class BringForwardHelper
    {
        public static IUndoableCommand CreateBringForwardCommand(FigureDto target)
        {
            var action = new Action(() =>
            {
                if (target.ZIndex < 100)
                {
                    target.ZIndex++;
                }
            });

            var undo = new Action(() =>
            {
                target.ZIndex--;
            });

            return new UndoableCommand.UndoableCommand(action, undo);
        }
    }
}
