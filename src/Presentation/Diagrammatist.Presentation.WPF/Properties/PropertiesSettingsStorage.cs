using Diagrammatist.Presentation.WPF.Core.Shared.Interfaces;
using System.Configuration;

namespace Diagrammatist.Presentation.WPF.Properties
{
    /// <summary>
    /// A settings storage implementation that uses the .NET properties system.
    /// </summary>
    public class PropertiesSettingsStorage : ISettingsStorage
    {
        private readonly ApplicationSettingsBase _inner = Settings.Default;

        /// <inheritdoc/>
        public bool Contains(string key)
            => _inner.Properties.Cast<SettingsProperty>()
                  .Any(p => p.Name == key);

        /// <inheritdoc/>
        public object? Get(string key)
            => _inner[key];

        /// <inheritdoc/>
        public void Set(string key, object value)
            => _inner[key] = value;

        /// <inheritdoc/>
        public void Save()
            => _inner.Save();
    }
}
