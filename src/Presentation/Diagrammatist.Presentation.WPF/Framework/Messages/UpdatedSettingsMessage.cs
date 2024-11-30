using CommunityToolkit.Mvvm.Messaging.Messages;
using DiagramApp.Contracts.Settings;

namespace DiagramApp.Presentation.WPF.Framework.Messages
{
    /// <summary>
    /// A message class that derives from <see cref="ValueChangedMessage{T}"/> of <see cref="DiagramSettingsDto"/>.
    /// </summary>
    /// <remarks>
    /// This class represents a message that sends <see cref="DiagramSettingsDto"/> from one to another instance.
    /// </remarks>
    public class UpdatedSettingsMessage : ValueChangedMessage<DiagramSettingsDto>
    {
        public UpdatedSettingsMessage(DiagramSettingsDto value) : base(value)
        {
        }
    }
}
