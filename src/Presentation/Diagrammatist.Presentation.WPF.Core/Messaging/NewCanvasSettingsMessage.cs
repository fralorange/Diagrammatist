using CommunityToolkit.Mvvm.Messaging.Messages;
using Diagrammatist.Presentation.WPF.Core.Models.Canvas;

namespace Diagrammatist.Presentation.WPF.Core.Messaging
{
    /// <summary>
    /// A message class that derives from <see cref="ValueChangedMessage{T}"/>. This class represents a new canvas settings message.
    /// </summary>
    public class NewCanvasSettingsMessage : ValueChangedMessage<SettingsModel>
    {
        public NewCanvasSettingsMessage(SettingsModel value) : base(value)
        {
        }
    }
}
