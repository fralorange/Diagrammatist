using DiagramApp.Client.Components.Specific.Builders;
using DiagramApp.Client.ViewModels;
using DiagramApp.Client.Views.Base;
using Microsoft.Maui.Layouts;

namespace DiagramApp.Client.Views;

public class BuildFlowchartPopupView : BuildDiagramPopupView
{
    public BuildFlowchartPopupView(BuildFlowchartPopupViewModel viewModel)
    {
        BindingContext = viewModel;

        AddFlowchartBuilderView();
        AddComponentsToPreview(viewModel);
    }

    private void AddFlowchartBuilderView()
    {
        var flowchartBuilderView = new FlowchartBuilderView();
        flowchartBuilderView.Margin = new Thickness(5);

        InsertFirstChild(flowchartBuilderView);
    }

    private void AddComponentsToPreview(BuildFlowchartPopupViewModel viewModel)
    {
        var dataTemplate = new DataTemplate(() =>
        {
            var border = new Border()
            {
                StrokeDashArray = new DoubleCollection(new double[] { 6, 3 }),
                StrokeDashOffset = 1
            };
            border.SetBinding(Border.TranslationXProperty, new Binding("XPos"));
            border.SetBinding(Border.TranslationYProperty, new Binding("YPos"));
            border.SetBinding(Border.WidthRequestProperty, new Binding("Width"));
            border.SetBinding(Border.HeightRequestProperty, new Binding("Height"));

            AbsoluteLayout.SetLayoutBounds(border, new Rect(0.5, 0.01, -1, -1));
            AbsoluteLayout.SetLayoutFlags(border, AbsoluteLayoutFlags.PositionProportional);

            var label = new Label
            {
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center
            };
            label.SetBinding(Label.TextProperty, new Binding("Text"));

            border.Content = label;

            return border;
        });

        SetupPreview(dataTemplate, viewModel.Components);
    }
}