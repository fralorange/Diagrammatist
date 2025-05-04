using CommunityToolkit.Mvvm.Messaging.Messages;
using Diagrammatist.Presentation.WPF.Core.Models.Canvas;

namespace Diagrammatist.Presentation.WPF.Core.Messaging.Messages
{
    /// <summary>
    /// A message class that derives from <see cref="ValueChangedMessage{T}"/> of <see cref="DiagramsModel"/>.
    /// </summary>
    public class UpdatedTypeMessage : ValueChangedMessage<DiagramsModel>
    {
        /// <inheritdoc/>
        public UpdatedTypeMessage(DiagramsModel value) : base(value)
        {
        }
    }
}
