using CommunityToolkit.Mvvm.ComponentModel;

namespace Diagrammatist.Presentation.WPF.Core.Shared.Dialogs.ViewModels.KeyValueEntry
{
    /// <summary>
    /// A class-decorator that allows to create observable key-value entries.
    /// </summary>
    public partial class KeyValueEntry : ObservableObject
    {
        /// <summary>
        /// Gets string key.
        /// </summary>
        public string Key { get; }

        /// <summary>
        /// Gets or sets string value.
        /// </summary>
        [ObservableProperty]
        private string _value;

        /// <summary>
        /// Initializes key value entry.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public KeyValueEntry(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }

}
