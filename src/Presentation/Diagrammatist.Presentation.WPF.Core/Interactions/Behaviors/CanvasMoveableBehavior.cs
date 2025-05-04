using System.Windows;

namespace Diagrammatist.Presentation.WPF.Core.Interactions.Behaviors
{
    /// <summary>
    /// A class that provides attached properties to enable or disable the movement of elements on a canvas.
    /// </summary>
    public class CanvasMoveableBehavior
    {
        /// <summary>
        /// A dependency property that indicates whether an element can be moved on a canvas.
        /// </summary>
        public static readonly DependencyProperty IsMovableProperty =
            DependencyProperty.RegisterAttached(
                "IsMovable",
                typeof(bool),
                typeof(CanvasMoveableBehavior),
                new PropertyMetadata(true));

        /// <summary>
        /// Gets a value indicating whether the element can be moved on a canvas.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool GetIsMovable(DependencyObject obj)
            => (bool)obj.GetValue(IsMovableProperty);

        /// <summary>
        /// Sets a value indicating whether the element can be moved on a canvas.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        public static void SetIsMovable(DependencyObject obj, bool value)
            => obj.SetValue(IsMovableProperty, value);
    }
}
