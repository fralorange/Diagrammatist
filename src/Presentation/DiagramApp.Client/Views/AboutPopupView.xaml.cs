using CommunityToolkit.Maui.Views;
using DiagramApp.Client.ViewModels;

namespace DiagramApp.Client.Views;

public partial class AboutPopupView : Popup
{
    public AboutPopupView(AboutPopupViewModel viewModel)
	{
		InitializeComponent();

        BindingContext = viewModel;
	}
}