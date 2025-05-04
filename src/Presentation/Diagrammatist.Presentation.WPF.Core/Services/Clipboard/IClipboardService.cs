namespace Diagrammatist.Presentation.WPF.Core.Services.Clipboard
{
    /// <summary>  
    /// A base interface for managing clipboard operations of different objects  
    /// </summary>  
    /// <typeparam name="TObject">Specified object for clipboard operations.</typeparam>  
    public interface IClipboardService<TObject> where TObject : class
    {
        /// <summary>  
        /// Copies object to clipboard.  
        /// </summary>  
        /// <param name="obj"></param>  
        /// <param name="key"></param>  
        void CopyToClipboard(TObject obj, string key = ""); 

        /// <summary>  
        /// Pastes object from clipboard.  
        /// </summary>  
        /// <param name="key"></param>
        TObject? GetFromClipboard(string key = "");
    }
}
