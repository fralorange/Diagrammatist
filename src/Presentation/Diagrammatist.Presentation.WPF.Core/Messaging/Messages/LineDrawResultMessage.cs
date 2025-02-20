using CommunityToolkit.Mvvm.Messaging.Messages;
using Diagrammatist.Presentation.WPF.Core.Models.Figures.Magnetic;
using System.Windows;

namespace Diagrammatist.Presentation.WPF.Core.Messaging.Messages
{
    /// <summary>
    /// A message class that derives from <see cref="ValueChangedMessage{T}"/>. This class represents line drawing operation result message.
    /// </summary>
    public class LineDrawResultMessage : ValueChangedMessage<(List<Point> points, MagneticPointModel? start, MagneticPointModel? end)?>
    {
        public LineDrawResultMessage((List<Point> points, MagneticPointModel? start, MagneticPointModel? end)? value) : base(value)
        {
        }
    }
}
