using Diagrammatist.Domain.Figures.Constants;
using System.Drawing;

namespace Diagrammatist.Domain.Figures
{
    /// <summary>
    /// A base class for figure objects.
    /// </summary>
    public abstract class Figure
    {
        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="FigureName"]/*'/>
        public string Name { get; set; } = FigureTextConstants.DefaultName;
        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="FigurePosX"]/*'/>
        public double PosX { get; set; } = FigureManipulationConstants.DefaultPosX;
        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="FigurePosY"]/*'/>
        public double PosY { get; set; } = FigureManipulationConstants.DefaultPosY;
        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="FigureRotation"]/*'/>
        public double Rotation { get; set; } = FigureManipulationConstants.DefaultRotation;
        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="FigureZIndex"]/*'/>
        public double ZIndex { get; set; } = FigureManipulationConstants.DefaultZIndex;
        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="FigureBackgroundColor"]/*'/>
        public Color BackgroundColor { get; set; }
    }
}
