using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Diagrammatist.Presentation.WPF.Core.Messages
{
    /// <summary>
    /// A message class that derives from <see cref="ValueChangedMessage{T}"/>. This class represents a canvas file path message.
    /// </summary>
    internal class CanvasFilePathMessage : ValueChangedMessage<string>
    {
        public CanvasFilePathMessage(string value) : base(value)
        {
        }
    }
}
