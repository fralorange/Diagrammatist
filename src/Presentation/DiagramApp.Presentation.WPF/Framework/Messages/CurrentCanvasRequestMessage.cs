using CommunityToolkit.Mvvm.Messaging.Messages;
using DiagramApp.Contracts.Canvas;

namespace DiagramApp.Presentation.WPF.Framework.Messages
{
    /// <summary>
    /// A message class that derives from <see cref="RequestMessage{T}"/> of nullable <see cref="CanvasDto"/>.
    /// </summary>
    /// <remarks>
    /// This class represents message that requests <see cref="CanvasDto"/> from one to another instance.
    /// </remarks>
    public class CurrentCanvasRequestMessage : RequestMessage<CanvasDto?>
    {
    }
}
