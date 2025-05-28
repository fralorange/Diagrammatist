using CommunityToolkit.Mvvm.Messaging.Messages;
using Diagrammatist.Presentation.WPF.Core.Models.Figures;

namespace Diagrammatist.Presentation.WPF.Core.Messaging.Messages
{
    /// <summary>
    /// A message class that derives from <see cref="ValueChangedMessage{T}"/>.
    /// This message is used to notify subscribers that a specific figure should be scrolled into view.
    /// </summary>
    public class ScrollToFigureMessage : ValueChangedMessage<FigureModel>
    {
        public ScrollToFigureMessage(FigureModel value) : base(value)
        {
        }
    }
}
