namespace Diagrammatist.Presentation.WPF.Core.Shared.Enums
{
    /// <summary>
    /// An enumeration that represents the result of a confirmation dialog.
    /// </summary>
    public enum ConfirmationResult
    {
        /// <summary>
        /// The user did not make a choice.
        /// </summary>
        None,
        /// <summary>
        /// The user confirmed the action.
        /// </summary>
        Yes,
        /// <summary>
        /// The user declined the action.
        /// </summary>
        No,
        /// <summary>
        /// The user canceled the action.
        /// </summary>
        Cancel
    }
}
