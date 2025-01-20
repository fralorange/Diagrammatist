using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Diagrammatist.Presentation.WPF.Core.Messaging.Messages
{
    /// <summary>
    /// A message class that derives from <see cref="ValueChangedMessage{T}"/>. This class represents an updated canvas file path message.
    /// </summary>
    public class UpdatedCanvasFilePathMessage : ValueChangedMessage<string>
    {
        public UpdatedCanvasFilePathMessage(string value) : base(value)
        {
        }
    }
}
