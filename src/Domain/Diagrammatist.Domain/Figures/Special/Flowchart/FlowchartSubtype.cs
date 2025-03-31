namespace Diagrammatist.Domain.Figures.Special.Flowchart
{
    /// <summary>
    /// An enumeration that defines flowchart subtypes.
    /// </summary>
    public enum FlowchartSubtype
    {
        /// <summary>
        /// A start or end subtype.
        /// </summary>
        StartEnd,
        /// <summary>
        /// A process subtype.
        /// </summary>
        Process,
        /// <summary>
        /// A input or output subtype.
        /// </summary>
        InputOutput,
        /// <summary>
        /// A decision subtype.
        /// </summary>
        Decision,
        /// <summary>
        /// A connector subtype.
        /// </summary>
        Connector,
        /// <summary>
        /// A preparation subtpe.
        /// </summary>
        Preparation,
        /// <summary>
        /// A predefined process suptype.
        /// </summary>
        PredefinedProcess,
        /// <summary>
        /// A document subtybe.
        /// </summary>
        Document,
        /// <summary>
        /// A database subtype.
        /// </summary>
        Database,
    }
}
