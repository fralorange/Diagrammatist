using CommunityToolkit.Mvvm.ComponentModel;
using System.Drawing;

namespace Diagrammatist.Presentation.WPF.Models.Figures
{
    /// <summary>
    /// A base model for figure objects.
    /// </summary>
    public abstract partial class FigureModel : ObservableObject
    {
        /// <summary>
        /// Gets or sets figure name.
        /// </summary>
        /// <remarks>
        /// This property used to store figure name.
        /// </remarks>
        [ObservableProperty]
        private string _name;

        /// <summary>
        /// Gets or sets figure position by X-axis.
        /// </summary>
        /// <remarks>
        /// This property used to determine figure position by x-axis.
        /// </remarks>
        [ObservableProperty]
        private double _posX;

        /// <summary>
        /// Gets or sets figure position by Y-axis.
        /// </summary>
        /// <remarks>
        /// This property used to determine figure position by y-axis.
        /// </remarks>
        [ObservableProperty]
        private double _posY;

        /// <summary>
        /// Gets or sets figure rotation.
        /// </summary>
        /// <remarks>
        /// This property used to configure figure rotation.
        /// </remarks>
        [ObservableProperty]
        private double _rotation;

        /// <summary>
        /// Gets or sets figure Z index.
        /// </summary>
        /// <remarks>
        /// This property used to configure overlap order between figures.
        /// </remarks>
        [ObservableProperty]
        private double _zIndex;

        /// <summary>
        /// Gets or sets figure background color.
        /// </summary>
        /// <remarks>
        /// This property used to store figure background color.
        /// </remarks>
        [ObservableProperty]
        private Color _backgroundColor;

        /// <summary>
        /// Default initializer.
        /// </summary>
#pragma warning disable CS8618 
        public FigureModel() { }
#pragma warning restore CS8618 

        /// <summary>
        /// Clones common properties of an abstract class.
        /// </summary>
        /// <param name="source">Source model.</param>
        protected FigureModel(FigureModel source)
        {
            Name = source.Name;
            PosX = source.PosX;
            PosY = source.PosY;
            Rotation = source.Rotation;
            ZIndex = source.ZIndex;
            BackgroundColor = source.BackgroundColor;
        }

        /// <summary>
        /// Deep clones an existing entity and returns a new entity.
        /// </summary>
        /// <returns>A deep clone of the <see cref="FigureModel"/>.</returns>
        public abstract FigureModel Clone();
    }
}
