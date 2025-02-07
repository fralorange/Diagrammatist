using CommunityToolkit.Mvvm.Messaging.Messages;
using System.Windows;

namespace Diagrammatist.Presentation.WPF.Core.Messaging.Messages
{
    /// <summary>
    /// A message class that derives from <see cref="ValueChangedMessage{T}"/>. This class represents line drawing operation result message.
    /// </summary>
    public class LineDrawResultMessage : ValueChangedMessage<(List<Point> points, Point point)?>
    {
        public LineDrawResultMessage((List<Point> points, Point point)? value) : base(value)
        {
        }
    }
}
