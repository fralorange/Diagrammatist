using Diagrammatist.Presentation.WPF.Models.Figures;

namespace Diagrammatist.Presentation.WPF.Core.Commands.Undoable.Helpers
{
    /// <summary>
    /// Helps to create <see cref="IUndoableCommand"/> that adjusts z index.
    /// </summary>
    internal static class ZIndexAdjustmentHelper
    {
        /// <summary>
        /// Creates an <see cref="IUndoableCommand"/> to adjust the ZIndex of the target item.
        /// </summary>
        /// <param name="target">The target item.</param>
        /// <param name="forward">True to bring forward, false to send backward.</param>
        /// <param name="minZIndex">The minimum ZIndex limit.</param>
        /// <param name="maxZIndex">The maximum ZIndex limit.</param>
        /// <returns><see cref="IUndoableCommand"/> instance.</returns>
        internal static IUndoableCommand CreateZIndexAdjustmentCommand(FigureModel target, bool forward, int minZIndex = 1, int maxZIndex = 100)
        {
            var adjust = forward ? 1 : -1;

            return CommonUndoableHelper.CreateUndoableCommand(
                () => AdjustZIndexAndRefresh(target, adjust, minZIndex, maxZIndex),
                () => AdjustZIndexAndRefresh(target, -adjust, minZIndex, maxZIndex)
            );
        }

        private static void AdjustZIndexAndRefresh(FigureModel target, double adjustment, int minZIndex, int maxZIndex)
        {
            double newZIndex = target.ZIndex + adjustment;

            if (newZIndex >= minZIndex && newZIndex <= maxZIndex)
            {
                target.ZIndex = newZIndex;
            }
        }
    }
}
