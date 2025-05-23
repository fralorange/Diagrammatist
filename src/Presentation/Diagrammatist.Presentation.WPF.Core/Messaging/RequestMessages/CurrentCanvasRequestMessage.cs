﻿using CommunityToolkit.Mvvm.Messaging.Messages;
using Diagrammatist.Presentation.WPF.Core.Models.Canvas;

namespace Diagrammatist.Presentation.WPF.Core.Messaging.RequestMessages
{
    /// <summary>
    /// A message class that derives from <see cref="RequestMessage{T}"/> of nullable <see cref="CanvasDto"/>.
    /// </summary>
    /// <remarks>
    /// This class represents message that requests <see cref="CanvasModel"/> from one to another instance.
    /// </remarks>
    public class CurrentCanvasRequestMessage : RequestMessage<CanvasModel?>
    {
    }
}
