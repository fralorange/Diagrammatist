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
            Offset= new ObservableOffset(_canvas.Offset);
        }

        public int ImaginaryWidth
        {
            get => _canvas.ImaginaryWidth;
            set => SetProperty(_canvas.ImaginaryWidth, value, _canvas, (c, iw) => c.ImaginaryWidth = iw);
        }

        public int ImaginaryHeight
        {
            get => _canvas.ImaginaryHeight;
            set => SetProperty(_canvas.ImaginaryHeight, value, _canvas, (c, ih) => c.ImaginaryHeight = ih);
        }

        public double Zoom
        {
            get => _canvas.Zoom;
            set => SetProperty(_canvas.Zoom, value, _canvas, (c, z) => c.Zoom = z);
        }

        public ControlsType Controls
        {
            get => _canvas.Controls;
            set => SetProperty(_canvas.Controls, value, _canvas, (c, ctrls) => c.Controls = ctrls);
        }

        public ObservableOffset Offset { get; }

        public DiagramSettings Settings => _canvas.Settings;

        public void ZoomIn(double zoomFactor, int? mouseX = null, int? mouseY = null)
        {
            _canvas.ZoomIn(zoomFactor, mouseX, mouseY);
            ZoomChanged();
        }

        public void ZoomOut(double zoomFactor, int? mouseX = null, int? mouseY = null)
        {
            _canvas.ZoomOut(zoomFactor, mouseX, mouseY);
            ZoomChanged();
        }

        public void ZoomReset()
        {
            _canvas.Zoom = 1;
            ZoomChanged();
        }

        public void MoveCanvas(double deltaX, double deltaY)
        {
            _canvas.MoveCanvas(deltaX, deltaY);
            OnPropertyChanged(nameof(Offset));
        }

        public void ChangeControls(string controlName)
        {
            _canvas.ChangeControls(controlName);
            OnPropertyChanged(nameof(Controls));
        }

        private void ZoomChanged()
        {
            OnPropertyChanged(nameof(Zoom));
            OnPropertyChanged(nameof(Offset));
            OnPropertyChanged(nameof(ImaginaryWidth));
            OnPropertyChanged(nameof(ImaginaryHeight));
        }
    }
}
