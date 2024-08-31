using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace DiagramApp.Presentation.WPF.Controls
{
    /// <summary>
    /// A class that derives from <see cref="Canvas"/> and represents extended canvas control.
    /// </summary>
    /// <remarks>
    /// Features:<br/>-Visible grid;<br/>-Element panning.
    /// </remarks>
    public class ExtendedCanvas : Canvas
    {
        private List<Line> _gridLines = [];

        public static readonly DependencyProperty IsGridVisibleProperty =
            DependencyProperty.Register(nameof(IsGridVisible), typeof(bool), typeof(ExtendedCanvas), new PropertyMetadata { DefaultValue = true });

        public static readonly DependencyProperty GridStepProperty =
            DependencyProperty.Register(nameof(GridStep), typeof(int), typeof(ExtendedCanvas), new PropertyMetadata { DefaultValue = 10 });

        public static readonly DependencyProperty GridLineColorProperty =
            DependencyProperty.Register(nameof(GridLineColor), typeof(Color), typeof(ExtendedCanvas));

        /// <summary>
        /// Gets or sets the grid visible parameter.
        /// </summary>
        public bool IsGridVisible
        {
            get { return (bool)GetValue(IsGridVisibleProperty); }
            set { SetValue(IsGridVisibleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the grid step.
        /// </summary>
        public int GridStep
        {
            get { return (int)GetValue(GridStepProperty); }
            set { SetValue(GridStepProperty, value); }
        }

        /// <summary>
        /// Gets or sets grid line color.
        /// </summary>
        public Color GridLineColor
        {
            get { return (Color)GetValue(GridLineColorProperty); }
            set { SetValue(GridLineColorProperty, value); }
        }

        public ExtendedCanvas()
        {
            //for (int x = -4000; x <= 4000; x += 100)
            //{
            //    Line verticalLine = new Line
            //    {
            //        Stroke = new SolidColorBrush(Colors.AliceBlue),
            //        X1 = x,
            //        Y1 = -4000,
            //        X2 = x,
            //        Y2 = 4000
            //    };

            //    if (x % 1000 == 0)
            //    {
            //        verticalLine.StrokeThickness = 6;
            //    }
            //    else
            //    {
            //        verticalLine.StrokeThickness = 2;
            //    }

            //    Children.Add(verticalLine);
            //    _gridLines.Add(verticalLine);
            //}

            //for (int y = -4000; y <= 4000; y += 100)
            //{
            //    Line horizontalLine = new Line
            //    {
            //        Stroke = new SolidColorBrush(Colors.AliceBlue),
            //        X1 = -4000,
            //        Y1 = y,
            //        X2 = 4000,
            //        Y2 = y
            //    };

            //    if (y % 1000 == 0)
            //    {
            //        horizontalLine.StrokeThickness = 6;
            //    }
            //    else
            //    {
            //        horizontalLine.StrokeThickness = 2;
            //    }

            //    Children.Add(horizontalLine);
            //    _gridLines.Add(horizontalLine);
            //}
        }
    }
}
