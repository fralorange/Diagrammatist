using CommunityToolkit.Mvvm.Messaging.Messages;
using DiagramApp.Contracts.Settings;

namespace DiagramApp.Presentation.WPF.Messages
{
    /// <summary>
    /// A message class that derives from <see cref="ValueChangedMessage{T}"/>. This class represents a new canvas settings message.
    /// </summary>
    public class NewCanvasSettingsMessage : ValueChangedMessage<DiagramSettingsDto>
    {
        public NewCanvasSettingsMessage(DiagramSettingsDto value) : base(value)
        {
        }
    }
}
