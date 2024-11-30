using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using DiagramApp.Domain.Figures;
using Microsoft.Maui.Controls.Shapes;
using System.Collections.ObjectModel;

namespace DiagramApp.Client.ViewModels.Wrappers
{
    public partial class ObservablePolylineFigure : ObservableFigure
    {
        [ObservableProperty]
        private ObservableCollection<System.Drawing.Point> _points;

        [ObservableProperty]
        private double _thickness = 2;

        [ObservableProperty]
        private bool _dashed;

        [ObservableProperty]
        private bool _arrow;

        [ObservableProperty]
        private PenLineJoin _lineJoin;

        public ObservablePolylineFigure(LineFigure polylineFigure) : base(polylineFigure)
        {
            _points = polylineFigure.Points.ToObservableCollection();
            _points.CollectionChanged += PointsCollectionChanged;
        }

        private void PointsCollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e) 
            => OnPropertyChanged(nameof(Points));
    }
}
