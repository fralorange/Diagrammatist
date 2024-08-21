namespace DiagramApp.Contracts.Figures
{
    /// <summary>
    /// Figure dto base class.
    /// </summary>
    public abstract class FigureDto
    {
        /// <summary>
        /// Figure name.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Figure position by X-axis.
        /// </summary>
        public double PosX { get; set; }
        /// <summary>
        /// Figure position by Y-axis.
        /// </summary>
        public double PosY { get; set; }
        /// <summary>
        /// Figure rotation.
        /// </summary>
        public double Rotation { get; set; }
        /// <summary>
        /// Figure Z index.
        /// </summary>
        public double ZIndex { get; set; }
    }
}
