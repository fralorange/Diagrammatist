using System.Windows;

namespace Diagrammatist.Presentation.WPF.Core.Helpers
{
    /// <summary>
    /// A class that helps with wpf controls.
    /// </summary>
    public static class WpfHelper
    {
        /// <summary>
        /// Gets element bounds relative to window.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="w"></param>
        /// <returns></returns>
        public static Rect GetElementBoundsRelativeToWindow(FrameworkElement element, Window w)
        {
            return element.TransformToAncestor(w).TransformBounds(new Rect(element.RenderSize));
        }
    }
}
