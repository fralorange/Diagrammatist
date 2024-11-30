using CommunityToolkit.Mvvm.Messaging.Messages;
using Diagrammatist.Contracts.Settings;

namespace Diagrammatist.Presentation.WPF.Framework.Messages
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
