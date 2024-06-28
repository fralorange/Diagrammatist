using DiagramApp.Client.Components.Specific.Builders;
using DiagramApp.Client.ViewModels;
using DiagramApp.Client.Views.Base;

namespace DiagramApp.Client.Views;

public class BuildFlowchartPopupView : BuildDiagramPopupView
{
	public BuildFlowchartPopupView(BuildFlowchartPopupViewModel viewModel)
	{
		BindingContext = viewModel;

		var flowchartBuilderView = new FlowchartBuilderView();
		flowchartBuilderView.Margin = new Thickness(5);

		InsertFirstChild(flowchartBuilderView);
	}
}