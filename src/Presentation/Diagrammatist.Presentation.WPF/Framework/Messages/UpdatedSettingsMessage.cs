using CommunityToolkit.Mvvm.Messaging.Messages;
using Diagrammatist.Presentation.WPF.Models.Canvas;

namespace Diagrammatist.Presentation.WPF.Framework.Messages
{
    /// <summary>
    /// A message class that derives from <see cref="ValueChangedMessage{T}"/> of <see cref="SettingsModel"/>.
    /// </summary>
    /// <remarks>
    /// This class represents a message that sends <see cref="SettingsModel"/> from one to another instance.
    /// </remarks>
    internal class UpdatedSettingsMessage : ValueChangedMessage<SettingsModel>
    {
        public UpdatedSettingsMessage(SettingsModel value) : base(value)
        {
        }
    }
}
