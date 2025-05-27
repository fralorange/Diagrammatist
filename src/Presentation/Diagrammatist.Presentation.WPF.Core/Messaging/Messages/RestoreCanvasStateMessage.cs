using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Diagrammatist.Presentation.WPF.Core.Messaging.Messages
{
    /// <summary>
    /// A message that is used to restore the canvas state, including zoom level and offsets.
    /// </summary>
    public class RestoreCanvasStateMessage : ValueChangedMessage<(float zoom, double hOffset, double vOffset)>
    {
        /// <inheritdoc/>
        public RestoreCanvasStateMessage((float zoom, double hOffset, double vOffset) value) : base(value)
        {
        }
    }
}
