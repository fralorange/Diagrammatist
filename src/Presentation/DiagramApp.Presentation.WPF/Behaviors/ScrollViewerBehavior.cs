using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Controls;

namespace DiagramApp.Presentation.WPF.Behaviors
{
    public class ScrollViewerBehavior : Behavior<ScrollViewer>
    {
        public static readonly DependencyProperty VerticalOffsetProperty = DependencyProperty
            .Register("VerticalOffset", typeof(double), typeof(ScrollViewerBehavior), new PropertyMetadata(0.0, OnVerticalOffsetChanged));

        public static readonly DependencyProperty HorizontalOffsetProperty = DependencyProperty
            .Register("HorizontalOffset", typeof(double), typeof(ScrollViewerBehavior), new PropertyMetadata(0.0, OnHorizontalOffsetChanged));

        public double VerticalOffset
        {
            get { return (double)GetValue(VerticalOffsetProperty); }
            set { SetValue(VerticalOffsetProperty, value); }
        }

        public double HorizontalOffset
        {
            get { return (double)GetValue(HorizontalOffsetProperty); }
            set { SetValue(HorizontalOffsetProperty, value); }
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.ScrollChanged += AssociatedObject_ScrollChanged;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.ScrollChanged -= AssociatedObject_ScrollChanged;
        }

        private void AssociatedObject_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            VerticalOffset = AssociatedObject.VerticalOffset;
            HorizontalOffset = AssociatedObject.HorizontalOffset;
        }

        private static void OnVerticalOffsetChanged(DependencyObject d,  DependencyPropertyChangedEventArgs e)
        {
            var behavior = (ScrollViewerBehavior)d;
            behavior.AssociatedObject.ScrollToVerticalOffset((double)e.NewValue);
        }

        private static void OnHorizontalOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var behavior = (ScrollViewerBehavior)d;
            behavior.AssociatedObject.ScrollToHorizontalOffset((double)e.NewValue);
        }
    }
}
