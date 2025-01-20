using System.Collections.ObjectModel;
using System.Windows.Data;

namespace Diagrammatist.Presentation.WPF.Core.Foundation.Extensions
{
    /// <summary>
    /// Observable collection extensions.
    /// </summary>
    public static class ObservableCollectionExtensions
    {
        /// <summary>
        /// Refreshes <see cref="ObservableCollection{T}"/> that fires CollectionChanged event.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">Target collection.</param>
        public static void Refresh<T>(this ObservableCollection<T> collection)
        {
            CollectionViewSource.GetDefaultView(collection).Refresh();
        }
    }
}
