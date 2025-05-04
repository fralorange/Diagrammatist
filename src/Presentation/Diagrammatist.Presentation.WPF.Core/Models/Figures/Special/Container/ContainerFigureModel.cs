using System.Windows.Media;

namespace Diagrammatist.Presentation.WPF.Core.Models.Figures.Special.Container
{
    /// <summary>
    /// A container figure model class. Derived class from <see cref="ShapeFigureModel"/>.
    /// </summary>
    /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="FigureContainer"]/*'/>
    public class ContainerFigureModel : ShapeFigureModel
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

        /// <inheritdoc/>
#pragma warning disable CS8618
        public ContainerFigureModel() { }

        /// <summary>
        /// Clones all properties and initializes new container figure model.
        /// </summary>
        /// <param name="source"></param>
        public ContainerFigureModel(ContainerFigureModel source) : base(source)
        {
            Text = source.Text;
            TextColor = source.TextColor;
            FontSize = source.FontSize;
        }
#pragma warning restore CS8618

        /// <inheritdoc/>
        public override FigureModel Clone()
        {
            return new ContainerFigureModel(this);
        }

        /// <inheritdoc/>
        public override void CopyPropertiesTo(FigureModel target)
        {
            base.CopyPropertiesTo(target);

            if (target is ContainerFigureModel containerTarget)
            {
                containerTarget.Text = Text;
                containerTarget.TextColor = TextColor;
                containerTarget.FontSize = FontSize;
            }
        }
    }
}
