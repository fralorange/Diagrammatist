﻿using DiagramApp.Contracts.Canvas;
using DiagramApp.Contracts.Settings;

namespace DiagramApp.Application.AppServices.Contexts.Canvas.Services
{
    /// <summary>
    /// Canvas manipulation service.
    /// </summary>
    public interface ICanvasManipulationService
    {
        /// <summary>
        /// Create new canvas.
        /// </summary>
        /// <param name="settings">Settings.</param>
        /// <returns><see cref="CanvasDto"/>. Canvas model.</returns>
        CanvasDto CreateCanvas(DiagramSettingsDto settings);
        /// <summary>
        /// Edits existing canvas.
        /// </summary>
        /// <param name="canvas">Target canvas.</param>
        /// <param name="settings">New settings.</param>
        void UpdateCanvas(CanvasDto canvas, DiagramSettingsDto settings);
    }
}
