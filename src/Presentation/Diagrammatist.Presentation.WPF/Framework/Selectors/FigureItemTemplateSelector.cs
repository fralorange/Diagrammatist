using DiagramApp.Contracts.Figures;
using System.Windows;
using System.Windows.Controls;

namespace DiagramApp.Presentation.WPF.Framework.Selectors
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
                ShapeFigureDto => ShapeFigureTemplate,
                LineFigureDto => LineFigureTemplate,
                TextFigureDto => TextFigureTemplate,
                _ => null
            };
        }
    }
}
