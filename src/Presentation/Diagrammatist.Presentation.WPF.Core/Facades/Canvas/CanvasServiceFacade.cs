using Diagrammatist.Application.AppServices.Canvas.Services;
using Diagrammatist.Presentation.WPF.Core.Services.Canvas.Interaction;
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
        public ICanvasSerializationService Serialization { get; }

        /// <summary>
        /// Initializes canvas service facade.
        /// </summary>
        public CanvasServiceFacade(IFigureManipulationService figureManipulationService,
                                   ICanvasInteractionService canvasInteractionService,
                                   ICanvasSerializationService canvasSerializationService)
        {
            FigureManipulation = figureManipulationService;
            Interaction = canvasInteractionService;
            Serialization = canvasSerializationService;
        }
    }
}
