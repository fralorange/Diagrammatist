using Diagrammatist.Presentation.WPF.Core.Services.Canvas.Interaction;
using Diagrammatist.Presentation.WPF.Core.Services.Canvas.Manipulation;
using Diagrammatist.Presentation.WPF.Core.Services.Figure.Manipulation;

namespace Diagrammatist.Presentation.WPF.Core.Facades.Canvas
{
    /// <summary>
    /// A class that implements <see cref="ICanvasServiceFacade"/>. 
    /// </summary>
    public class CanvasServiceFacade : ICanvasServiceFacade
    {
        /// <inheritdoc/>
        public IFigureManipulationService FigureManipulation { get; }

        /// <inheritdoc/>
        public ICanvasInteractionService Interaction { get; }

        /// <inheritdoc/>
        public ICanvasManipulationService Manipulation { get; }

        /// <summary>
        /// Initializes canvas service facade.
        /// </summary>
        public CanvasServiceFacade(IFigureManipulationService figureManipulationService,
                                   ICanvasInteractionService canvasInteractionService,
                                   ICanvasManipulationService manipulation)
        {
            FigureManipulation = figureManipulationService;
            Interaction = canvasInteractionService;
            Manipulation = manipulation;
        }
    }
}
