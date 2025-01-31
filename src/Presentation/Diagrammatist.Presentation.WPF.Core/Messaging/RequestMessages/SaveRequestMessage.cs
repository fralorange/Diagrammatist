using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Diagrammatist.Presentation.WPF.Core.Messaging.RequestMessages
{
    /// <summary>
    /// A message class that derives from <see cref="RequestMessage{T}"/> of <see cref="bool"/>.
    /// </summary>
    /// <remarks>
    /// This class represents message that orders to save current file and send results as bool.
    /// </remarks>
    public class SaveRequestMessage : RequestMessage<bool>
    {
    }
}
