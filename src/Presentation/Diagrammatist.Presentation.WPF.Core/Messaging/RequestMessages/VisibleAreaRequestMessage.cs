using CommunityToolkit.Mvvm.Messaging.Messages;
using System.Windows;

namespace Diagrammatist.Presentation.WPF.Core.Messaging.RequestMessages
{
    /// <summary>
    /// A messsage class that derives from <see cref="RequestMessage{T}"/> and is used to request the visible area of a canvas.
    /// </summary>
    public class VisibleAreaRequestMessage : RequestMessage<Rect>
    {
    }
}
