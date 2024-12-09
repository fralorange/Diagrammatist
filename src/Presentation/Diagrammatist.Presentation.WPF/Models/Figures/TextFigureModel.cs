using CommunityToolkit.Mvvm.ComponentModel;
using System.Drawing;

namespace Diagrammatist.Presentation.WPF.Models.Figures
{
    /// <summary>
    /// A text figure model. Derived class from <see cref="FigureModel"/>.
    /// </summary>
    public partial class TextFigureModel : FigureModel
    {
        private string _text;

        /// <summary>
        /// Gets or sets figure Text.
        /// </summary>
        /// <remarks>
        /// This property used to display text in figure UI.
        /// </remarks>
        public string Text
        {
            get => _text;
            set => SetProperty(ref _text, value);
        }

        private Color _textColor;

        /// <summary>
        /// Gets or sets figure text color.
        /// </summary>
        /// <remarks>
        /// This property used to store figure text color.
        /// </remarks>
        public Color TextColor
        {
            get => _textColor;
            set => SetProperty(ref _textColor, value);
        }

        private double _fontSize;

        /// <summary>
        /// Gets or sets font size.
        /// </summary>
        /// <remarks>
        /// This property used to configure font size.
        /// </remarks>
        public double FontSize
        {
            get => _fontSize;
            set => SetProperty(ref _fontSize, value);
        }


        private bool _hasOutline;

        /// <summary>
        /// Gets or sets figure outline condition.
        /// </summary>
        /// <remarks>
        /// This property indicates whether figure has outline.
        /// </remarks>
        public bool HasOutline
        {
            get => _hasOutline;
            set => SetProperty(ref _hasOutline, value);
        }

        private bool _hasBackground;

        /// <summary>
        /// Gets or sets figure background condition,
        /// </summary>
        /// <remarks>
        /// This property indicates whether figure has background.
        /// </remarks>
        public bool HasBackground
        {
            get => _hasBackground;
            set => SetProperty(ref _hasBackground, value);
        }

        /// <summary>
        /// Default initializer.
        /// </summary>
#pragma warning disable CS8618
        public TextFigureModel() { }
#pragma warning restore CS8618

        /// <summary>
        /// Clones all properties and initializes new instance.
        /// </summary>
        /// <param name="source">Source model.</param>
#pragma warning disable CS8618 
        public TextFigureModel(TextFigureModel source) : base(source)
#pragma warning restore CS8618 
        {
            Text = source.Text;
            TextColor = source.TextColor;
            FontSize = source.FontSize;
            HasOutline = source.HasOutline;
            HasBackground = source.HasBackground;
        }

        /// <inheritdoc/>
        public override FigureModel Clone()
        {
            return new TextFigureModel(this);
        }
    }
}
