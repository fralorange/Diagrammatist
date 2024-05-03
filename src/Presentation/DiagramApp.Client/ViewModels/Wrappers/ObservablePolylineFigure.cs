using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using DiagramApp.Domain.Canvas.Figures;
using Microsoft.Maui.Controls.Shapes;
using System.Collections.ObjectModel;

namespace DiagramApp.Client.ViewModels.Wrappers
{
    public partial class ObservablePolylineFigure : ObservableFigure
    {
        [ObservableProperty]
        private ObservableCollection<System.Drawing.Point> _points;

        public ObservablePolylineFigure(PolylineFigure polylineFigure) : base(polylineFigure)
        {
            _points = polylineFigure.Points.ToObservableCollection();
        }

        [ObservableProperty]
        private double _thickness = 2;

        [ObservableProperty]
        private bool _dashed;

        [ObservableProperty]
        private PenLineJoin _lineJoin;
    }
}
