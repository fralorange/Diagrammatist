using DiagramApp.Domain.Canvas.Figures;

namespace DiagramApp.Client.ViewModels.Wrappers
{
    public class ObservablePathFigure : ObservableFigure
    {
        private readonly PathFigure _pathFigure;

        public ObservablePathFigure(PathFigure pathFigure) : base(pathFigure) => _pathFigure = pathFigure;
        
        public string PathData
        {
            get => _pathFigure.PathData;
            set => SetProperty(_pathFigure.PathData, value, _pathFigure, (pF, pD) => pF.PathData = pD);
        }
    }
}
