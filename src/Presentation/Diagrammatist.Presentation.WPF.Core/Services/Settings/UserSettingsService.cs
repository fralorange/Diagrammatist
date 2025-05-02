using Diagrammatist.Presentation.WPF.Core.Shared.Interfaces;
using System.Configuration;

namespace Diagrammatist.Presentation.WPF.Core.Services.Settings
{
    /// <summary>
    /// A class that implements <see cref="IUserSettingsService"/> interface.
    /// This class is used to manage user settings.
    /// </summary>
    public class UserSettingsService : IUserSettingsService
    {
        private readonly ISettingsStorage _storage;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserSettingsService"/> class.
        /// </summary>
        public UserSettingsService(ISettingsStorage storage)
        {
            _storage = storage;
        }

        /// <inheritdoc/>
        public T? Get<T>(string key, T? defaultValue = default)
        {
            if (_storage.Contains(key))
            {
                var raw = _storage.Get(key);
                if (raw is T t) return t;
                try
                {
                    return (T?)Convert.ChangeType(raw, typeof(T));
                }
                catch { }
            }
            return defaultValue;
        }

        /// <inheritdoc/>
        public void Set<T>(string key, T value)
            => _storage.Set(key, value!);

        /// <inheritdoc/>
        public void Save()
            => _storage.Save();
    }
}
