using DiagramApp.Application.AppServices.Helpers;
using DiagramApp.Client.ViewModels;
using DiagramApp.Client.ViewModels.Wrappers;
using Microsoft.Maui.Controls.Shapes;

namespace DiagramApp.Client.Components.Specific.Properties;

public partial class DataFlowPropertiesView : Grid
{
    public DataFlowPropertiesView()
    {
        InitializeComponent();
    }

    private void OnThicknessEntryCompleted(object sender, EventArgs e)
    {
        if (LineThicknessValidator.IsNotValid)
            return;
        PropertiesView.HandleEntryCompleted(BindingContext, sender, "Thickness", value => Convert.ToDouble(value));
    }

    private void OnDashedCheckboxChanged(object sender, CheckedChangedEventArgs e)
        => PropertiesView.HandleCheckboxCompleted(BindingContext, "Dashed", e);

    private void OnArrowCheckboxChanged(object sender, CheckedChangedEventArgs e)
        => PropertiesView.HandleCheckboxCompleted(BindingContext, "Arrow", e);

    private void OnLineJoinButtonClicked(object sender, string arg)
    {
        if (sender is ImageButton && BindingContext is MainViewModel { CurrentCanvas.SelectedFigure: ObservablePolylineFigure figure })
        {
            figure.LineJoin = arg switch
            {
                "Miter" => PenLineJoin.Miter,
                "Bevel" => PenLineJoin.Bevel,
                "Round" => PenLineJoin.Round,
                _ => default,
            };
        }
    }

    private void OnMiterClicked(object sender, EventArgs e) => OnLineJoinButtonClicked(sender, "Miter");

    private void OnBevelClicked(object sender, EventArgs e) => OnLineJoinButtonClicked(sender, "Bevel");

    private void OnRoundClicked(object sender, EventArgs e) => OnLineJoinButtonClicked(sender, "Round");

    private void OnEntryXCompleted(object sender, EventArgs e)
    {
        if (BindingContext is MainViewModel { CurrentCanvas: { SelectedFigure: ObservablePolylineFigure figure } canvas } && sender is Entry entry && entry.BindingContext is System.Drawing.Point point)
        {
            if (entry.Text.Any(char.IsLetter))
                return;
            var index = figure.Points.IndexOf(point);
            var action = new Action(() =>
            {
                figure.Points[index] = new System.Drawing.Point(int.Parse(entry.Text), point.Y);
            });
            var undoAction = new Action(() =>
            {
                figure.Points[index] = point;
            });
            UndoableCommandHelper.ExecuteAction(canvas, action, undoAction);
        }
    }

    private void OnEntryYCompleted(object sender, EventArgs e)
    {
        if (BindingContext is MainViewModel { CurrentCanvas: { SelectedFigure: ObservablePolylineFigure figure } canvas } && sender is Entry entry && entry.BindingContext is System.Drawing.Point point)
        {
            if (entry.Text.Any(char.IsLetter))
                return;
            var index = figure.Points.IndexOf(point);
            var action = new Action(() =>
            {
                figure.Points[index] = new System.Drawing.Point(point.X, int.Parse(entry.Text));
            });
            var undoAction = new Action(() =>
            {
                figure.Points[index] = point;
            });
            UndoableCommandHelper.ExecuteAction(canvas, action, undoAction);
        }
    }
}