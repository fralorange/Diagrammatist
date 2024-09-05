namespace DiagramApp.Presentation.WPF.ViewModels.Components.Enums.Modes
{
    /// <summary>
    /// Specifies the mouse mode.
    /// </summary>
    public enum MouseMode
    {
        /// <summary>
        /// Selection mode.
        /// </summary>
        /// <remarks>
        /// Default mouse mode. <br/>
        /// This mode used to select objects that placed on the canvas.
        /// </remarks>
        Select,
        /// <summary>
        /// Panning mode.
        /// </summary>
        /// <remarks>
        /// This mode used to move the canvas in window.
        /// </remarks>
        Pan,
        /// <summary>
        /// Transform mode.
        /// </summary>
        /// <remarks>
        /// This mode used to transform objects that placed on the canvas.
        /// </remarks>
        Transform,
    }
}
