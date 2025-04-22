using Diagrammatist.Presentation.WPF.Core.Models.Document;
using Diagrammatist.Presentation.WPF.Simulator.Models.Context;

namespace Diagrammatist.Presentation.WPF.Simulator.Providers
{
    /// <summary>
    /// A class that provides required simulation context.
    /// </summary>
    public static class SimulationContextProvider
    {
        private const string Key = "Simulation";

        /// <summary>
        /// Saves simulation context to document.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="context"></param>
        public static void SaveToDocument(DocumentModel doc, SimulationContext context)
        {
            doc.SetPayload(Key, context);
        }

        /// <summary>
        /// Loads simulation context from document.
        /// </summary>
        /// <param name="doc"></param>
        /// <returns><c><see cref="SimulationContext"/></c> if it exists, otherwise <c><see cref="null"/></c>.</returns>
        public static SimulationContext? LoadFromDocument(DocumentModel doc)
        {
            return doc.GetPayloadData<SimulationContext>(Key);
        }
    }
}
