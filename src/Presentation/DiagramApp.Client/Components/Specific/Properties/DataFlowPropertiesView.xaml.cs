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

    private void OnLineJoinButtonClicked(object sender, EventArgs e)
    {
        if (sender is Button { Text: { } btnText } && BindingContext is MainViewModel { CurrentCanvas.SelectedFigure: ObservablePolylineFigure figure })
        {
            figure.LineJoin = btnText switch
            {
                "Miter" => PenLineJoin.Miter,
                "Bevel" => PenLineJoin.Bevel,
                "Round" => PenLineJoin.Round,
                _ => default,
            };
        }
    }
}