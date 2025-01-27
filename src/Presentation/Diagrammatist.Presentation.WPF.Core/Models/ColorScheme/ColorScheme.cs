using System.Collections.ObjectModel;

namespace Diagrammatist.Presentation.WPF.Core.Models.ColorScheme
{
    /// <summary>
    /// A class that describes color scheme data.
    /// </summary>
    public class ColorScheme
    {
        /// <summary>
        /// Gets or initializes <see cref="ObservableCollection{T}"/> of <see cref="ColorSchemeEntry"/> objects.
        /// </summary>
        /// <remarks>
        /// This property used to store color scheme entries.
        /// </remarks>
        public ObservableCollection<ColorSchemeEntry> ColorSchemeEntries { get; init; }

        public ColorScheme(IEnumerable<ColorSchemeEntry> entries)
        {
            ColorSchemeEntries = new ObservableCollection<ColorSchemeEntry>(entries);
        }
    }
}
