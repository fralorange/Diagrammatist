﻿using CommunityToolkit.Mvvm.ComponentModel;
using Diagrammatist.Presentation.WPF.Core.Foundation.Base.ObservableObject;
using System.Windows.Media;

namespace Diagrammatist.Presentation.WPF.Core.Models.Figures
{
    /// <summary>
    /// A base model for figure objects.
    /// </summary>
    public abstract partial class FigureModel : ExtendedObservableObject
    {
        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="FigureId"]/*'/>
        public Guid Id { get; set; }

        private string _name;

        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="FigureName"]/*'/>
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="FigurePosX"]/*'/>
        [ObservableProperty]
        private double _posX;

        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="FigurePosY"]/*'/>
        [ObservableProperty]
        private double _posY;

        protected double _rotation;

        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="FigureRotation"]/*'/>
        public virtual double Rotation
        {
            get => _rotation;
            set => SetProperty(ref _rotation, value);
        }

        private double _zIndex;

        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="FigureZIndex"]/*'/>
        public double ZIndex
        {
            get => _zIndex;
            set => SetProperty(ref _zIndex, value);
        }

        private Color _backgroundColor;

        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="FigureBackgroundColor"]/*'/>
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
            Id = Guid.NewGuid();
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

        /// <summary>
        /// Copies properties from this instance to the target instance.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="copyMetadata"></param>
        public virtual void CopyPropertiesTo(FigureModel target)
        {
            target.Rotation = Rotation;
            target.ZIndex = ZIndex;
            target.BackgroundColor = BackgroundColor;
        }
    }
}
