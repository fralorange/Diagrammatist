using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Data;

namespace Diagrammatist.Presentation.WPF.Framework.Extensions.ObservableCollection
{
    /// <summary>
    /// Observable collection extensions.
    /// </summary>
    public static class ObservableCollectionExtensions
    {
        /// <summary>
        /// Links <see cref="ObservableCollection{T}"/> to any <see cref="ICollection{T}"/> implementation.
        /// <para>
        /// Changes on the ObservableCollection are reflected on the bound collection
        /// </para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="observableCollection"></param>
        /// <param name="collection">Target <see cref="ICollection{T}"/> implementation.</param>
        /// <remarks>
        /// This method used to link <see cref="ObservableCollection{T}"/> (presumably in ViewModel) to <see cref="ICollection{T}"/> implementation (presumably in Model).
        /// </remarks>
        public static void LinkTo<T>(this ObservableCollection<T> observableCollection, ICollection<T> collection)
        {
            observableCollection.CollectionChanged += (o, e) =>
            {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        foreach (var item in e.NewItems!)
                        {
                            collection.Add((T)item);
                        }
                        break;
                    case NotifyCollectionChangedAction.Remove:
                        foreach (var item in e.OldItems!)
                        {
                            collection.Remove((T)item);
                        }
                        break;
                    case NotifyCollectionChangedAction.Replace:
                        var t = collection.ElementAt(e.OldStartingIndex);
                        t = (T)e.NewItems![0]!;
                        break;
                    case NotifyCollectionChangedAction.Reset:
                        collection.Clear();
                        foreach (var item in observableCollection)
                        {
                            collection.Add(item);
                        }
                        break;
                }
            };
        }

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
