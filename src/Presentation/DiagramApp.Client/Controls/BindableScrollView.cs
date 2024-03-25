using DiagramApp.Client.ViewModels.Wrappers;

namespace DiagramApp.Client.Controls
{
    public class BindableScrollView : ScrollView
    {
        public bool IsInitializedScroll { private get; set; }

        public static readonly BindableProperty ScrollProperty =
            BindableProperty.Create(
                nameof(Scroll),
                typeof(ObservableOffset),
                typeof(BindableScrollView),
                default(ObservableOffset),
                propertyChanged: OnScrollPropertyChanged);

        public ObservableOffset Scroll
        {
            get => (ObservableOffset)GetValue(ScrollProperty);
            set => SetValue(ScrollProperty, value);
        }

        protected override void OnChildAdded(Element child)
        {
            base.OnChildAdded(child);

            if (child is Layout layout)
            {
                var boxView = layout.Children.OfType<BoxView>().FirstOrDefault();
                if (boxView is not null)
                {
                    boxView.SizeChanged += OnContentSizeChanged;
                }
            }
        }

        private async void OnContentSizeChanged(object? sender, EventArgs e)
        {
            if (IsInitializedScroll && Scroll is not null)
            {
                IsInitializedScroll = false;
                await ScrollToAsync(Scroll.X, Scroll.Y, false);
            }
        }

        private static void OnScrollPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var scrollView = (BindableScrollView)bindable;

            if (newValue is ObservableOffset offset && offset is not null)
            {
                scrollView.IsInitializedScroll = true;
            }
        }
    }
}
