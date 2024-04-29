using CommunityToolkit.Mvvm.ComponentModel;
using DiagramApp.Domain.Canvas.Figures;
using Microsoft.Maui.Controls.Shapes;

namespace DiagramApp.Client.ViewModels.Wrappers
{
    public partial class ObservablePolylineFigure : ObservableFigure
    {
        private readonly PolylineFigure _polylineFigure;

        public ObservablePolylineFigure(PolylineFigure polylineFigure) : base(polylineFigure) => _polylineFigure = polylineFigure;

        [ObservableProperty]
        private double _thickness = 2;

        [ObservableProperty]
        private bool _dashed;

        [ObservableProperty]
        private PenLineJoin _lineJoin;

        public List<System.Drawing.Point> Points
        {
            get => _polylineFigure.Points;
            set => SetProperty(_polylineFigure.Points, value, _polylineFigure, (pF, ps) => pF.Points = ps);
        }
    }
}
