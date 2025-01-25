using CommunityToolkit.Mvvm.ComponentModel;

namespace Diagrammatist.Presentation.WPF.Core.Models.ColorScheme
{
    /// <summary>
    /// A class that derives from <see cref="ObservableObject"/>. This class provides color scheme properties.
    /// </summary>
    public partial class ColorSchemeEntry : ObservableObject
    {
        /// <summary>
        /// Gets or sets hex string value.
        /// </summary>
        [ObservableProperty]
#pragma warning disable CS8618
        private string _hex;
        /// <summary>
        /// Gets or sets color scheme entry name.
        /// </summary>
        public string Name { get; set; }
#pragma warning restore CS8618 
    }
}
