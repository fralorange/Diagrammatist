using CommunityToolkit.Mvvm.ComponentModel;
using DiagramApp.Domain.Canvas;
using DiagramApp.Domain.DiagramSettings;

namespace DiagramApp.Client.ViewModels.Wrappers
{
    public class ObservableCanvas : ObservableObject
    {
        private readonly Canvas _canvas;

        public ObservableCanvas(Canvas canvas)
        {
            _canvas = canvas;
            Zoom = _canvas.Zoom;
        }

        public double Zoom
        {
            get => _canvas.Zoom;
            set => SetProperty(_canvas.Zoom, value, _canvas, (c, z) => c.Zoom = z);
        }

        public int PositionX
        {
            get => _canvas.PositionX;
            set => SetProperty(_canvas.PositionX, value, _canvas, (c, px) => c.PositionX = px);
        }

        public int PositionY
        {
            get => _canvas.PositionY;
            set => SetProperty(_canvas.PositionY, value, _canvas, (c, py) => c.PositionY = py);
        }

        public DiagramSettings Settings { get => _canvas.Settings; }

        public void ZoomIn(double zoomFactor, int? mouseX = null, int? mouseY = null)
        {
            _canvas.ZoomIn(zoomFactor, mouseX, mouseY);
            OnPropertyChanged(nameof(Zoom));
        }

        public void ZoomOut(double zoomFactor, int? mouseX = null, int? mouseY = null)
        {
            _canvas.ZoomOut(zoomFactor, mouseX, mouseY);
            OnPropertyChanged(nameof(Zoom));
        }
    }
}
