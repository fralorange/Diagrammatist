using CommunityToolkit.Mvvm.ComponentModel;
using DiagramApp.Domain.Canvas.Figures;

namespace DiagramApp.Client.ViewModels.Wrappers
{
    public partial class ObservableTextFigure : ObservableFigure
    {
        private readonly TextFigure _textFigure;

        public ObservableTextFigure(TextFigure textFigure) : base(textFigure) => _textFigure = textFigure;

        [ObservableProperty]
        private double _fontSize = 14;

        [ObservableProperty]
        private bool _hasOutline = true;

        [ObservableProperty]
        private bool _hasBackground = true;

        public string Text
        {
            get => _textFigure.Text;
            set => SetProperty(_textFigure.Text, value, _textFigure, (tF, t) => tF.Text = t);
        }
    }
}
