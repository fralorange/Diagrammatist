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

    private async void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender is CollectionView { SelectedItem: { } } collectionView)
        {
            if (BindingContext is MainViewModel { CurrentCanvas: { } } viewModel)
            {
                var current = e.CurrentSelection[0] as ToolboxItem;
                var figure = current!.Figure;

                ObservableFigure? observableFigure = null;
                Action addAction = null!;
                addAction = async () =>
                {
                    switch (figure)
                    {
                        case PathFigure pathFigure:
                            observableFigure = new ObservablePathFigure(new PathFigure { Name = pathFigure.Name, PathData = pathFigure.PathData });
                            break;
                        case PolylineFigure polylineFigure:
                            var blockedPoints = await viewModel.CurrentCanvas.BlockAsync<List<System.Drawing.Point>>();
                            if (blockedPoints is null) break;
                            var points = new List<System.Drawing.Point>(blockedPoints);
                            var (translatedX, translatedY) = points.Count > 1
                                ? (points.Min(pt => pt.X), points.Min(pt => pt.Y))
                                : (default(int), default(int));
                            NormalizePoints(points, (translatedX, translatedY));
                            observableFigure = new ObservablePolylineFigure(new PolylineFigure
                            {
                                Name = polylineFigure.Name,
                                Points = points.Count > 1 ? points : polylineFigure.Points
                            })
                            {
                                TranslationX = points.Count > 1 ? translatedX : default,
                                TranslationY = points.Count > 1 ? translatedY : default
                            };
                            break;
                        case TextFigure textFigure:
                            observableFigure = new ObservableTextFigure(new TextFigure { Name = textFigure.Name, Text = textFigure.Text });
                            break;
                    }

                    if (observableFigure is not null)
                    {
                        viewModel.CurrentCanvas.Figures.Add(observableFigure);
                        viewModel.CurrentCanvas.AddUndoCommand(() =>
                        {
                            viewModel.CurrentCanvas.Figures.Remove(observableFigure);
                            viewModel.CurrentCanvas.AddRedoCommand(addAction);
                        });
                    }
                };

                addAction.Invoke();
                viewModel.CurrentCanvas.ClearRedoCommands();
            }

            Dispatcher.Dispatch(() =>
            {
                collectionView.SelectedItem = null;
            });
        }
    }

    private void NormalizePoints(List<System.Drawing.Point> points, (int X, int Y) normalPoint)
    {
        if (points.Count <= 1) return;

        for (int i = 0; i < points.Count; i++)
        {
            points[i] = new System.Drawing.Point(points[i].X - normalPoint.X, points[i].Y - normalPoint.Y);
        }
    }
}