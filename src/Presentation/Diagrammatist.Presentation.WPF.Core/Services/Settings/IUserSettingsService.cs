namespace Diagrammatist.Presentation.WPF.Core.Services.Settings
{
    /// <summary>
    /// An interface for user settings service.
    /// </summary>
    public interface IUserSettingsService
    {
        /// <summary>
        /// Gets the user settings value by key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        T? Get<T>(string key, T? defaultValue = default);

        /// <summary>
        /// Sets the user settings value by key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void Set<T>(string key, T value);

        /// <summary>
        /// Saves all changes in user settings.
        /// </summary>
        void Save();
    }
}
