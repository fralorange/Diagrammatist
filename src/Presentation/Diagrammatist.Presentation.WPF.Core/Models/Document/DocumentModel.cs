using Diagrammatist.Presentation.WPF.Core.Models.Canvas;

namespace Diagrammatist.Presentation.WPF.Core.Models.Document
{
    /// <summary>
    /// A class that represents model of document.
    /// </summary>
    public class DocumentModel
    {
        /// <summary>
        /// Gets or sets canvas model.
        /// </summary>
        public required CanvasModel Canvas { get; set; }
        
    }
}
