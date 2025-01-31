using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Diagrammatist.Presentation.WPF.Core.Messaging.Messages
{
    /// <summary>
    /// A class that derives from <see cref="ValueChangedMessage{T}"/> of <see cref="string"/>. This class notifies modules about theme change.
    /// </summary>
    public class ThemeChangedMessage : ValueChangedMessage<string>
    {
        public ThemeChangedMessage(string value) : base(value)
        {
        }
    }
}
