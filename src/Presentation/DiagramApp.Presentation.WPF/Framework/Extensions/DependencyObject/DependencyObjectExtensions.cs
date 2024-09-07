using System.Windows.Media;
using DependencyObjectEnt = System.Windows.DependencyObject;

namespace DiagramApp.Presentation.WPF.Framework.Extensions.DependencyObject
{
    /// <summary>
    /// Extension methods for the <see cref="DependencyObjectEnt" /> type.
    /// </summary>
    public static class DependencyObjectExtensions
    {
        /// <summary>
        /// Gets the first child of the specified visual that is of type <typeparamref name="T" />
        /// in the visual tree recursively.
        /// </summary>
        /// <param name="visual">The visual to get the visual children for.</param>
        /// <returns>
        /// The first child of the specified visual that is of tyoe <typeparamref name="T" /> of the
        /// specified visual in the visual tree recursively or <c>null</c> if none was found.
        /// </returns>
        public static T? GetVisualDescendant<T>(this DependencyObjectEnt visual) where T : DependencyObjectEnt
        {
            return (T?)visual.GetVisualDescendants().FirstOrDefault(d => d is T);
        }

        /// <summary>
        /// Gets all children of the specified visual in the visual tree recursively.
        /// </summary>
        /// <param name="visual">The visual to get the visual children for.</param>
        /// <returns>All children of the specified visual in the visual tree recursively.</returns>
        public static IEnumerable<DependencyObjectEnt> GetVisualDescendants(this DependencyObjectEnt visual)
        {
            if (visual == null)
            {
                yield break;
            }

            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(visual); i++)
            {
                var child = VisualTreeHelper.GetChild(visual, i);
                yield return child;
                foreach (var subChild in GetVisualDescendants(child))
                {
                    yield return subChild;
                }
            }
        }

        /// <summary>
        /// Gets the first ancestor of the specified visual that is of type <typeparamref name="T" />
        /// in the visual tree recursively.
        /// </summary>
        /// <param name="visual">The visual to get the visual ancestors for.</param>
        /// <returns>
        /// The first ancestor of the specified visual that is of type <typeparamref name="T" /> of the
        /// specified visual in the visual tree recursively or <c>null</c> if none was found.
        /// </returns>
        public static T? GetVisualAncestor<T>(this DependencyObjectEnt visual) where T : DependencyObjectEnt
        {
            while (visual != null)
            {
                if (visual.GetType() == typeof(T))
                {
                    return visual as T;
                }
                visual = VisualTreeHelper.GetParent(visual);
            }
            return null;
        }
    }
}
