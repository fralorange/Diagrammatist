using Diagrammatist.Presentation.WPF.Core.Managers.Command;
using Diagrammatist.Presentation.WPF.Core.Models.Canvas;
using Diagrammatist.Presentation.WPF.Core.Services.Alert;

namespace Diagrammatist.Presentation.WPF.Core.Helpers
{
    /// <summary>
    /// A static class that provides methods to adapt the theme of the canvas.
    /// </summary>
    public static class ThemeAdaptHelper
    {
        /// <summary>
        /// Adapts the theme of the canvas by applying colors to figures and setting the background color.
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="alertService"></param>
        public static void AdaptTheme(CanvasModel canvas, IAlertService alertService, ITrackableCommandManager commandManager)
        {
            var localizedCaption = LocalizationHelper
                .GetLocalizedValue<string>("Diagrammatist.Presentation.WPF", "Dialogs.Settings.SettingsResources", "ThemeDecisionCaption");
            var localizedMessage = LocalizationHelper
                .GetLocalizedValue<string>("Diagrammatist.Presentation.WPF", "Dialogs.Settings.SettingsResources", "ThemeDecisionMessage");

            var result = alertService.RequestYesNoDecision(localizedMessage, localizedCaption);

            if (result is Shared.Enums.ConfirmationResult.Yes)
            {
                (var snapshot, var background) = (FigureColorHelper.Capture(canvas.Figures), canvas.Settings.Background);

                var command = CommonUndoableHelper.CreateUndoableCommand(
                    () =>
                    {
                        FigureColorHelper.ApplyColors(canvas.Figures);
                        canvas.Settings.Background = ThemeColorHelper.GetBackgroundColor();
                    },
                    () =>
                    {
                        FigureColorHelper.Restore(snapshot);
                        canvas.Settings.Background = background;
                    });

                commandManager.Execute(command);
            }
        }
    }
}
