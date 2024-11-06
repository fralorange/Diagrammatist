using CommunityToolkit.Mvvm.Messaging.Messages;
using DiagramApp.Contracts.Settings;

namespace DiagramApp.Presentation.WPF.Framework.Messages
{
    public class UpdatedSettingsMessage : ValueChangedMessage<DiagramSettingsDto>
    {
        public UpdatedSettingsMessage(DiagramSettingsDto value) : base(value)
        {
        }
    }
}
