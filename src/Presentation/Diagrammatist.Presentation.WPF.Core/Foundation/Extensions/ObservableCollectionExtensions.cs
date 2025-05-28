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

        /// <summary>
        /// Casts <see cref="IEnumerable{T}"/> to <see cref="ObservableCollection{T}"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <returns></returns>
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> enumerable)
        {
            return new ObservableCollection<T>(enumerable);
        }

        /// <summary>
        /// Replaces the contents of an <see cref="ObservableCollection{T}"/> with a new set of items.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="newItems"></param>
        public static void ReplaceWith<T>(this ObservableCollection<T> collection, IEnumerable<T> newItems)
        {
            ArgumentNullException.ThrowIfNull(collection);
            ArgumentNullException.ThrowIfNull(newItems);
            collection.Clear();
            foreach (var item in newItems)
            {
                collection.Add(item);
            }
        }

        /// <summary>
        /// Removes all items from an <see cref="ObservableCollection{T}"/> that match a specified predicate.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="predicate"></param>
        public static void RemoveAll<T>(this ObservableCollection<T> collection, Func<T, bool> predicate)
        {
            ArgumentNullException.ThrowIfNull(collection);
            ArgumentNullException.ThrowIfNull(predicate);
            var itemsToRemove = collection.Where(predicate).ToList();
            foreach (var item in itemsToRemove)
            {
                collection.Remove(item);
            }
        }
    }
}
