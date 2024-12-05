using CommunityToolkit.Mvvm.Messaging.Messages;
using Diagrammatist.Presentation.WPF.Models.Canvas;

namespace Diagrammatist.Presentation.WPF.Framework.Messages
{
    /// <summary>
    /// A message class that derives from <see cref="ValueChangedMessage{T}"/>. This class represents a new canvas settings message.
    /// </summary>
    internal class NewCanvasSettingsMessage : ValueChangedMessage<SettingsModel>
    {
        public NewCanvasSettingsMessage(SettingsModel value) : base(value)
        {
        }
    }
}
