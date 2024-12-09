using CommunityToolkit.Mvvm.ComponentModel;
using Diagrammatist.Presentation.WPF.Framework.BaseClasses.ObservableObject;
using System.Drawing;

namespace Diagrammatist.Presentation.WPF.Models.Figures
{
    /// <summary>
    /// A base model for figure objects.
    /// </summary>
    public abstract partial class FigureModel : ExtendedObservableObject
    {
        private string _name;

        /// <summary>
        /// Gets or sets figure name.
        /// </summary>
        /// <remarks>
        /// This property used to store figure name.
        /// </remarks>
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

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

        private double _rotation;

        /// <summary>
        /// Gets or sets figure rotation.
        /// </summary>
        /// <remarks>
        /// This property used to configure figure rotation.
        /// </remarks>
        public double Rotation
        {
            get => _rotation;
            set => SetProperty(ref _rotation, value);
        }


        private double _zIndex;

        /// <summary>
        /// Gets or sets figure Z index.
        /// </summary>
        /// <remarks>
        /// This property used to configure overlap order between figures.
        /// </remarks>
        public double ZIndex
        {
            get => _zIndex;
            set => SetProperty(ref _zIndex, value);
        }

        private Color _backgroundColor;

        /// <summary>
        /// Gets or sets figure background color.
        /// </summary>
        /// <remarks>
        /// This property used to store figure background color.
        /// </remarks>
        public Color BackgroundColor
        {
            get => _backgroundColor;
            set => SetProperty(ref _backgroundColor, value);
        }

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
#pragma warning disable CS8618 
        protected FigureModel(FigureModel source)
#pragma warning restore CS8618 
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
