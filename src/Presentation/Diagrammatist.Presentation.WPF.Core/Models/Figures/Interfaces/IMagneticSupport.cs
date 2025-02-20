using Diagrammatist.Presentation.WPF.Core.Models.Figures.Magnetic;

namespace Diagrammatist.Presentation.WPF.Core.Models.Figures.Interfaces
{
    /// <summary>
    /// An interfaces that provides magnetic support behavior to object.
    /// </summary>
    public interface IMagneticSupport
    {
        /// <summary>
        /// Gets magnetic points.
        /// </summary>
        List<MagneticPointModel> MagneticPoints { get; }

        /// <summary>
        /// Updates magnetic points.
        /// </summary>
        void UpdateMagneticPoints();
    }
}
