using DiagramApp.Domain.Canvas.Figures;

namespace DiagramApp.Client.ViewModels.Wrappers
{
    public class ObservablePolylineFigure : ObservableFigure
    {
        private readonly PolylineFigure _polylineFigure;

        public ObservablePolylineFigure(PolylineFigure polylineFigure) : base(polylineFigure) => _polylineFigure = polylineFigure;

        public List<System.Drawing.Point> Points
        {
            get => _polylineFigure.Points;
            set => SetProperty(_polylineFigure.Points, value, _polylineFigure, (pF, ps) => pF.Points = ps);
        }
    }
}
