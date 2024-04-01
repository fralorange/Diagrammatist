using CommunityToolkit.Mvvm.ComponentModel;
using DiagramApp.Domain.Canvas.Figures;

namespace DiagramApp.Client.ViewModels.Wrappers
{
    public partial class ObservableFigure : ObservableObject
    {
        private readonly Figure _figure;

        public ObservableFigure(Figure figure)
            => _figure = figure;

        [ObservableProperty]
        private bool _isSelected;

        [ObservableProperty]
        private double _translationX;
        
        [ObservableProperty]
        private double _translationY;

        public string Name
        {
            get => _figure.Name;
            set => SetProperty(_figure.Name, value, _figure, (f, n) =>  f.Name = n);
        }

        public string PathData
        {
            get => _figure.PathData;
            set => SetProperty(_figure.PathData, value, _figure, (f, p) => f.PathData = p);
        }
    }
}
