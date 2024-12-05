using Diagrammatist.Presentation.WPF.Models.Figures;
using System.Windows;
using System.Windows.Controls;

namespace Diagrammatist.Presentation.WPF.Framework.Selectors
{
    public class FigureItemTemplateSelector : DataTemplateSelector
    {
        /// <summary>
        /// Gets or sets shape figure template.
        /// </summary>
        public DataTemplate? ShapeFigureTemplate { get; set; }
        /// <summary>
        /// Gets or sets line figure template.
        /// </summary>
        public DataTemplate? LineFigureTemplate { get; set; }
        /// <summary>
        /// Gets or sets text figure template.
        /// </summary>
        public DataTemplate? TextFigureTemplate { get; set; }

        public override DataTemplate? SelectTemplate(object item, DependencyObject container)
        {
            return item switch
            {
                ShapeFigureModel => ShapeFigureTemplate,
                LineFigureModel => LineFigureTemplate,
                TextFigureModel => TextFigureTemplate,
                _ => null
            };
        }
    }
}
