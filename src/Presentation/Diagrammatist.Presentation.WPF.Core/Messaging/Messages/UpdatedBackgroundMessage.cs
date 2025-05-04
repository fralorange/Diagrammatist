using CommunityToolkit.Mvvm.Messaging.Messages;
using System.Windows.Media;

namespace Diagrammatist.Presentation.WPF.Core.Messaging.Messages
{
    /// <summary>
    /// A message class that derives from <see cref="ValueChangedMessage{T}"/> of <see cref="Color"/>.
    /// </summary>
    public class UpdatedBackgroundMessage : ValueChangedMessage<Color>
    {
        /// <inheritdoc/>
        public UpdatedBackgroundMessage(Color value) : base(value)
        {
        }
    }
}
