using CommunityToolkit.Mvvm.ComponentModel;
using DiagramApp.Domain.Figures;

namespace DiagramApp.Client.ViewModels.Wrappers
{
    public partial class ObservablePathFigure : ObservableFigure
    {
        private readonly ShapeFigure _pathFigure;

        public ObservablePathFigure(ShapeFigure pathFigure) : base(pathFigure) => _pathFigure = pathFigure;

        [ObservableProperty]
        private double _width = 50;

        [ObservableProperty]
        private double _height = 50;

        [ObservableProperty]
        private bool _aspect;

        public string PathData
        {
            get => _pathFigure.Data;
            set => SetProperty(_pathFigure.Data, value, _pathFigure, (pF, pD) => pF.Data = pD);
        }
    }
}
