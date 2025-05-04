using System.Media;

namespace Diagrammatist.Presentation.WPF.Core.Services.Sound
{
    /// <summary>
    /// A class that implements <see cref="ISoundService"/>. Provides base operations for playing sounds.
    /// </summary>
    public class SoundService : ISoundService
    {
        /// <inheritdoc/>
        public void PlayWarningSound() 
            => SystemSounds.Exclamation.Play();
    }
}
