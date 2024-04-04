using CommunityToolkit.Maui.Views;
using DiagramApp.Client.ViewModels;

namespace DiagramApp.Client.Views;

public partial class AboutPopupView : Popup
{
    public string AppNameWithVersion { get; } = $"{AppInfo.Name} {AppInfo.Version}";
    public string Build { get; } = AppInfo.BuildString;

    public AboutPopupView(AboutPopupViewModel viewModel)
	{
		InitializeComponent();

        BindingContext = viewModel;
	}
}