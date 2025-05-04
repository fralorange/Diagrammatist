using Diagrammatist.Presentation.WPF.Core.Controls;
using Diagrammatist.Presentation.WPF.Core.Models.Figures;
using System.Windows;
using System.Windows.Documents;

namespace Diagrammatist.Presentation.WPF.Core.Interactions.Behaviors
{
    /// <summary>
    /// A class that represents <see cref="MagneticPointsAdorner"/> behavior.
    /// </summary>
    public class MagneticPointsAdornerBehavior
    {
        /// <summary>
        /// An magnetic points attached property.
        /// </summary>
        public static readonly DependencyProperty AttachMagneticPointsProperty =
            DependencyProperty.RegisterAttached(
                "AttachMagneticPoints",
                typeof(bool),
                typeof(MagneticPointsAdornerBehavior),
                new PropertyMetadata(false, OnAttachMagneticPointsChanged));

        /// <summary>
        /// An magnetic points dependency property.
        /// </summary>
        public static readonly DependencyProperty IsVisibleProperty =
            DependencyProperty.RegisterAttached(
                "IsVisible",
                typeof(bool),
                typeof(MagneticPointsAdornerBehavior),
                new PropertyMetadata(true, OnIsVisibleChanged));

        private static readonly Dictionary<UIElement, MagneticPointsAdorner> _adorners = new();

        /// <summary>
        /// Sets attached magnetic points property.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public static void SetAttachMagneticPoints(UIElement element, bool value)
        {
            element.SetValue(AttachMagneticPointsProperty, value);
        }

        /// <summary>
        /// Gets attached magnetic points property.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static bool GetAttachMagneticPoints(UIElement element)
        {
            return (bool)element.GetValue(AttachMagneticPointsProperty);
        }

        /// <summary>
        /// Sets visible parameter.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public static void SetIsVisible(UIElement element, bool value)
        {
            element.SetValue(IsVisibleProperty, value);
        }

        /// <summary>
        /// Gets visible parameter.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static bool GetIsVisible(UIElement element)
        {
            return (bool)element.GetValue(IsVisibleProperty);
        }

        private static void OnAttachMagneticPointsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FrameworkElement elem && (bool)e.NewValue)
            {
                elem.Loaded += (s, args) =>
                {
                    var adornerLayer = AdornerLayer.GetAdornerLayer(elem);
                    if (adornerLayer is not null && elem.DataContext is ShapeFigureModel model)
                    {
                        if (_adorners.TryGetValue(elem, out var existingAdorner))
                        {
                            existingAdorner.UpdatePoints(model.MagneticPoints.Select(mp => mp.Position).ToList());
                            existingAdorner.IsVisible = GetIsVisible(elem); 
                        }
                        else
                        {
                            var adorner = new MagneticPointsAdorner(elem, model.MagneticPoints.Select(mp => mp.Position).ToList())
                            {
                                IsVisible = GetIsVisible(elem)
                            };
                            adornerLayer.Add(adorner);
                            _adorners[elem] = adorner;
                        }
                    }
                };
            }
        }


        private static void OnIsVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is UIElement elem && _adorners.TryGetValue(elem, out var adorner))
            {
                adorner.IsVisible = (bool)e.NewValue;
            }
        }
    }
}
