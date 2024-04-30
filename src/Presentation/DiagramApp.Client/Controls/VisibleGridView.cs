namespace DiagramApp.Client.Controls
{
    public class VisibleGridView : AbsoluteLayout
    {
        public static readonly BindableProperty GridSpacingProperty =
            BindableProperty.Create(
                nameof(GridSpacing),
                typeof(double),
                typeof(VisibleGridView),
                10d);

        public double GridSpacing
        {
            get => (double)GetValue(GridSpacingProperty);
            set => SetValue(GridSpacingProperty, value);
        }

        public VisibleGridView()
        {
            BackgroundColor = Colors.Transparent;
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            Children.Clear();

            for (double x = 0; x < width; x+= GridSpacing)
            {
                var line = new BoxView { Color = Colors.Gray, WidthRequest = 1, HeightRequest = height };
                line.SetValue(LayoutBoundsProperty, new Rect(x, 0, 1, height));
                Children.Add(line);
            }

            for (double y = 0; y < height; y+= GridSpacing)
            {
                var line = new BoxView { Color = Colors.Gray, WidthRequest = width, HeightRequest = 1 };
                line.SetValue(LayoutBoundsProperty, new Rect(0, y, width, 1));
                Children.Add(line);
            }
        }
    }
}
