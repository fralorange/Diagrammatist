namespace Diagrammatist.Presentation.WPF.Core.Shared.Interfaces
{
    /// <summary>
    /// An interface for settings storage.
    /// </summary>
    public interface ISettingsStorage
    {
        /// <summary>
        /// Determines whether the settings storage contains the specified key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool Contains(string key);
        /// <summary>
        /// Gets the settings value by key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        object? Get(string key);
        /// <summary>
        /// Sets the settings value by key.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void Set(string key, object value);
        /// <summary>
        /// Saves the storage with new changes.
        /// </summary>
        void Save();
    }
}
