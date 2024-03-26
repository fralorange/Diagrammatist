using CommunityToolkit.Mvvm.ComponentModel;
using DiagramApp.Domain.Canvas;

namespace DiagramApp.Client.ViewModels.Wrappers
{
    public class ObservableOffset : ObservableObject
    {
        private readonly Offset _offset;

        public ObservableOffset(Offset offset)
        {
            _offset = offset;
        }

        public double X
        {
            get => _offset.X;
            set => SetProperty(_offset.X, value, _offset, (o, x) => o.X = x);
        }

        public double Y
        {
            get => _offset.Y;
            set => SetProperty(_offset.Y, value, _offset, (o, y) => o.Y = y);
        }
    }
}
