using CommunityToolkit.Mvvm.ComponentModel;
using DiagramApp.Domain.Canvas.Figures;

namespace DiagramApp.Client.ViewModels.Wrappers
{
    public partial class ObservablePathFigure : ObservableFigure
    {
        private readonly PathFigure _pathFigure;

        public ObservablePathFigure(PathFigure pathFigure) : base(pathFigure) => _pathFigure = pathFigure;

        [ObservableProperty]
        private double _width = 50;

        [ObservableProperty]
        private double _height = 50;

        [ObservableProperty]
        private bool _aspect;

        public string PathData
        {
            get => _pathFigure.PathData;
            set => SetProperty(_pathFigure.PathData, value, _pathFigure, (pF, pD) => pF.PathData = pD);
        }
    }
}
