namespace Diagrammatist.Presentation.WPF.Core.Services.Sound
{
    /// <summary>
    /// An interface that incapsulates the operations for playing sounds.
    /// </summary>
    public interface ISoundService
    {
        /// <summary>
        /// Plays a sound when an error occurs.
        /// </summary>
        void PlayWarningSound();
    }
}
