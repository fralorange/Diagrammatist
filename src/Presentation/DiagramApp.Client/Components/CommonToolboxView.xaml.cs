using DiagramApp.Client.ViewModels;
using DiagramApp.Client.ViewModels.Wrappers;
using DiagramApp.Domain.Canvas.Figures;
using DiagramApp.Domain.Toolbox;

namespace DiagramApp.Client.Components;

public partial class CommonToolboxView : Grid
{
	public CommonToolboxView()
	{
		InitializeComponent();
	}

    private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender is CollectionView { SelectedItem: { } } collectionView)
        {
            if (BindingContext is MainViewModel { CurrentCanvas: { } } viewModel)
            {
                var current = e.CurrentSelection[0] as ToolboxItem;
                var figure = current!.Figure;

                if (figure is PathFigure pathFigure)
                    viewModel.CurrentCanvas!.Figures.Add(new ObservablePathFigure(new PathFigure { Name = pathFigure.Name, PathData = pathFigure.PathData }));
                else if (figure is PolylineFigure polylineFigure)
                    viewModel.CurrentCanvas!.Figures.Add(new ObservablePolylineFigure(new PolylineFigure { Name = polylineFigure.Name, Points = polylineFigure.Points }));
                else if (figure is TextFigure textFigure)
                    viewModel.CurrentCanvas!.Figures.Add(new ObservableTextFigure(new TextFigure { Name = textFigure.Name, Text = textFigure.Text }));
            }

            Dispatcher.Dispatch(() =>
            {
                collectionView.SelectedItem = null;
            });
        }
    }
}