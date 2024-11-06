using CommunityToolkit.Mvvm.Messaging.Messages;
using DiagramApp.Contracts.Canvas;

namespace DiagramApp.Presentation.WPF.Framework.Messages
{
    public class CurrentCanvasRequestMessage : RequestMessage<CanvasDto?>
    {
    }
}
