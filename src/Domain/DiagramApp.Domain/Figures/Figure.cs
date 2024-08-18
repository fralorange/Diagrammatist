using DiagramApp.Domain.Figures.Constants;

namespace DiagramApp.Domain.Figures
{
    /// <summary>
    /// Figure base class.
    /// </summary>
    public abstract class Figure
    {
        /// <summary>
        /// Figure name.
        /// </summary>
        public string Name { get; set; } = FigureTextConstants.DefaultName;
        /// <summary>
        /// Figure position by X-axis.
        /// </summary>
        public double PosX { get; set; } = FigureManipulationConstants.DefaultPosX;
        /// <summary>
        /// Figure position by Y-axis.
        /// </summary>
        public double PosY { get; set; } = FigureManipulationConstants.DefaultPosY;
        /// <summary>
        /// Figure rotation.
        /// </summary>
        public double Rotation { get; set; } = FigureManipulationConstants.DefaultRotation;
        /// <summary>
        /// Figure Z index.
        /// </summary>
        public double ZIndex { get; set; } = FigureManipulationConstants.DefaultZIndex;
    }
}
