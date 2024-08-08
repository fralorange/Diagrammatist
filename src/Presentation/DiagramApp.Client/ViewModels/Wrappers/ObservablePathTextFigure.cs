using CommunityToolkit.Mvvm.ComponentModel;
using DiagramApp.Domain.Figures;

namespace DiagramApp.Client.ViewModels.Wrappers
{
    public partial class ObservablePathTextFigure : ObservablePathFigure
    {
        public ObservablePathTextFigure(PathFigure pathFigure) : base(pathFigure) { }

        [ObservableProperty]
        private string _text = string.Empty;

        [ObservableProperty]
        private double _fontSize = 14;
    }
}
