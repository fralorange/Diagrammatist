namespace Diagrammatist.Domain.Figures.Magnetic
{
    /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="MagneticPoint"]/*'/>
    public class MagneticPoint
    {
        /// <summary>
        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="Position"]/*'/> (By X-axis)
        /// </summary>
        public double PosX { get; set; }
        /// <summary>
        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="Position"]/*'/> (By Y-axis)
        /// </summary>
        public double PosY { get; set; }
        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Sources/Member[@name="Owner"]/*'/>
        public required ShapeFigure Owner { get; set; }
    }
}
