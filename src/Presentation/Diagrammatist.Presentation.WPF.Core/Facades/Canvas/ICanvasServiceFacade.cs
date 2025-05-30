﻿using Diagrammatist.Presentation.WPF.Core.Services.Canvas.Interaction;
using Diagrammatist.Presentation.WPF.Core.Services.Canvas.Manipulation;
using Diagrammatist.Presentation.WPF.Core.Services.Figure.Manipulation;

namespace Diagrammatist.Presentation.WPF.Core.Facades.Canvas
{
    /// <summary>
    /// A class that acts as aggregator in canvas layer.
    /// </summary>
    public interface ICanvasServiceFacade
    {
        /// <summary>
        /// Gets figure manipulation service.
        /// </summary>
        IFigureManipulationService FigureManipulation { get; }
        /// <summary>
        /// Gets canvas interaction service.
        /// </summary>
        ICanvasInteractionService Interaction { get; }
        /// <summary>
        /// Gets canvas manipulation service.
        /// </summary>
        ICanvasManipulationService Manipulation { get; }
    }
}
