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
}