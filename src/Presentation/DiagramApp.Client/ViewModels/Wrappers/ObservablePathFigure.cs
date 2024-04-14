using CommunityToolkit.Mvvm.ComponentModel;
using DiagramApp.Domain.Canvas.Figures;

namespace DiagramApp.Client.ViewModels.Wrappers
{
    public partial class ObservablePathFigure : ObservableFigure
    {
        private readonly PathFigure _pathFigure;

        public ObservablePathFigure(PathFigure pathFigure) : base(pathFigure) => _pathFigure = pathFigure;

        [ObservableProperty]
        private double _size = 50;

        public string PathData
        {
            get => _pathFigure.PathData;
            set => SetProperty(_pathFigure.PathData, value, _pathFigure, (pF, pD) => pF.PathData = pD);
        }
    }
}
