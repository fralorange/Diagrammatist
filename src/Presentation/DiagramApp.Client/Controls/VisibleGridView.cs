namespace DiagramApp.Client.Controls
{
    public class VisibleGridView : AbsoluteLayout
    {
        public static readonly BindableProperty GridSpacingProperty =
            BindableProperty.Create(
                nameof(GridSpacing),
                typeof(double),
                typeof(VisibleGridView),
                10d,
                propertyChanged: OnGridSpacingPropertyChanged);

        public double GridSpacing
        {
            get => (double)GetValue(GridSpacingProperty);
            set => SetValue(GridSpacingProperty, value);
        }

        public VisibleGridView()
        {
            BackgroundColor = Colors.Transparent;
        }

        private static void OnGridSpacingPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var gridView = (VisibleGridView)bindable;
            gridView.OnSizeAllocated(gridView.Width, gridView.Height);
        }
        // maybe change it to vectors or imgs, since it loads app heavily
        protected override async void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            await Task.Run(() =>
            {
                var lines = new List<BoxView>();

                for (double x = 0; x < width; x += GridSpacing)
                {
                    var line = new BoxView { Color = Colors.Gray, WidthRequest = 1, HeightRequest = height };
                    line.SetValue(LayoutBoundsProperty, new Rect(x, 0, 1, height));
                    lines.Add(line);
                }

                for (double y = 0; y < height; y += GridSpacing)
                {
                    var line = new BoxView { Color = Colors.Gray, WidthRequest = width, HeightRequest = 1 };
                    line.SetValue(LayoutBoundsProperty, new Rect(0, y, width, 1));
                    lines.Add(line);
                }

                Dispatcher.Dispatch(() =>
                {
                    Children.Clear();
                    foreach (var line in lines)
                    {
                        Children.Add(line);
                    }
                });
            });
        }
    }
}
