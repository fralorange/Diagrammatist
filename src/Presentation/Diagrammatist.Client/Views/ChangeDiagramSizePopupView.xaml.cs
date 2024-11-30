using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Messaging;
using DiagramApp.Client.ViewModels;
using DiagramApp.Domain.Settings;
using LocalizationResourceManager.Maui;

namespace DiagramApp.Client.Views;

public partial class ChangeDiagramSizePopupView : Popup
{
	public ChangeDiagramSizePopupView(ChangeDiagramSizePopupViewModel viewModel, ILocalizationResourceManager localizationResourceManager)
	{
		InitializeComponent();

		BindingContext = viewModel;

        WeakReferenceMessenger.Default.Register<DiagramSettings>(this, async (r, settings) =>
        {
            string errorMsg = "";
            if (widthValidator.IsNotValid)
            {
                errorMsg += $"\n{localizationResourceManager["Width"]}";
            }
            if (heightValidator.IsNotValid)
            {
                errorMsg += $"\n{localizationResourceManager["Height"]}";
            }
            if (!string.IsNullOrEmpty(errorMsg))
            {
                await Shell.Current.DisplayAlert($"{localizationResourceManager["Error"]}",
                                                 $"{string.Format(localizationResourceManager["ErrorAttributesString"], errorMsg)}",
                                                 $"{localizationResourceManager["OK"]}");
                return;
            }

            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            await CloseAsync(settings, cts.Token);
        });
    }

    private async void OnCancelClicked(object sender, EventArgs e) => await CloseAsync();
}