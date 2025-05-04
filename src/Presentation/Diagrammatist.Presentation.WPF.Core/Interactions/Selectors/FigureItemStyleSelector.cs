using Diagrammatist.Presentation.WPF.Core.Models.Figures;
using System.Windows;
using System.Windows.Controls;

namespace Diagrammatist.Presentation.WPF.Core.Interactions.Selectors
{
    /// <summary>
    /// A class that derives from <see cref="StyleSelector"/>. Selects style by figure item type.
    /// </summary>
    public class FigureItemStyleSelector : StyleSelector
    {
        /// <summary>
        /// Gets or sets the style for a moveable figure.
        /// </summary>
        public Style? MoveableFigureStyle { get; set; }

        /// <summary>
        /// Gets or sets the style for a immovable figure.
        /// </summary>
        public Style? ImmovableFigureStyle { get; set; }

        /// <inheritdoc/>
        public override Style? SelectStyle(object item, DependencyObject container)
        {
            return item switch
            {
                LineFigureModel => ImmovableFigureStyle,
                FigureModel => MoveableFigureStyle,
                _ => null
            };
        }
    }
}
