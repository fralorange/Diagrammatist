using System.Windows.Media;

namespace Diagrammatist.Presentation.WPF.Core.Models.Figures
{
    /// <summary>
    /// A text figure model. Derived class from <see cref="FigureModel"/>.
    /// </summary>
    public partial class TextFigureModel : FigureModel
    {
        private string _text;

        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="FigureTextContent"]/*'/>
        public string Text
        {
            get => _text;
            set => SetProperty(ref _text, value);
        }

        private Color _textColor;

        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="FigureTextColor"]/*'/>
        public Color TextColor
        {
            get => _textColor;
            set => SetProperty(ref _textColor, value);
        }

        private double _fontSize;

        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="FigureTextFontSize"]/*'/>
        public double FontSize
        {
            get => _fontSize;
            set => SetProperty(ref _fontSize, value);
        }


        private bool _hasOutline;

        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="FigureTextHasOutline"]/*'/>
        public bool HasOutline
        {
            get => _hasOutline;
            set => SetProperty(ref _hasOutline, value);
        }

        private bool _hasBackground;

        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="FigureTextHasBackground"]/*'/>
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

        /// <inheritdoc/>
        public override void CopyPropertiesTo(FigureModel target)
        {
            base.CopyPropertiesTo(target);

            if (target is TextFigureModel textTarget)
            {
                textTarget.Text = Text;
                textTarget.TextColor = TextColor;
                textTarget.FontSize = FontSize;
                textTarget.HasOutline = HasOutline;
                textTarget.HasBackground = HasBackground;
            }
        }
    }
}
