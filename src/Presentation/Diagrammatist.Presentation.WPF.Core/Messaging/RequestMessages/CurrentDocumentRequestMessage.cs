using CommunityToolkit.Mvvm.Messaging.Messages;
using Diagrammatist.Presentation.WPF.Core.Models.Document;

namespace Diagrammatist.Presentation.WPF.Core.Messaging.RequestMessages
{
    /// <summary>
    /// A class that derives from <see cref="RequestMessage{T}"/> of <see cref="DocumentModel"/>.
    /// Requests current <see cref="DocumentModel"/> instance.
    /// </summary>
    public class CurrentDocumentRequestMessage : RequestMessage<DocumentModel?>
    {
    }
}
