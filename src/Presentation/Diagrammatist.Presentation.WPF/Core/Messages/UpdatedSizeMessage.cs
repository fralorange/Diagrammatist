using CommunityToolkit.Mvvm.Messaging.Messages;
using Diagrammatist.Presentation.WPF.Models.Canvas;
using System.Drawing;

namespace Diagrammatist.Presentation.WPF.Core.Messages
{
    /// <summary>
    /// A message class that derives from <see cref="ValueChangedMessage{T}"/> of <see cref="SettingsModel"/>.
    /// </summary>
    /// <remarks>
    /// This class represents a message that sends <see cref="Size"/> from one to another instance.
    /// </remarks>
    internal class UpdatedSizeMessage : ValueChangedMessage<Size>
    {
        public UpdatedSizeMessage(Size value) : base(value)
        {
        }
    }
}
