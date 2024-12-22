namespace Diagrammatist.Presentation.WPF.Managers.Clipboard
{
    /// <summary>
    /// A base interface for managing clipboard operations of different objects
    /// </summary>
    /// <typeparam name="TObject">Specified object for clipboard operations.</typeparam>
    public interface IClipboardManager<TObject> where TObject : class
    {
        /// <summary>
        /// Copies object to clipboard.
        /// </summary>
        void CopyToClipboard(TObject obj);
        /// <summary>
        /// Pastes object from clipboard.
        /// </summary>
        TObject? PasteFromClipboard();
    }
}
