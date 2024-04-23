using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Messaging;
using DiagramApp.Client.ViewModels;
using DiagramApp.Domain.DiagramSettings;
using LocalizationResourceManager.Maui;

namespace DiagramApp.Client.Views;

public partial class NewDiagramPopupView : Popup
{
    public NewDiagramPopupView(NewDiagramPopupViewModel viewmodel, ILocalizationResourceManager localizationResourceManager)
    {
        InitializeComponent();

        BindingContext = viewmodel;

        WeakReferenceMessenger.Default.Register<DiagramSettings>(this, async (r, settings) =>
        {
            string errorMsg = "";
            if (WidthValidator.IsNotValid)
            {
                errorMsg += $"\n{localizationResourceManager["Width"]}";
            }
            if (HeightValidator.IsNotValid)
            {
                errorMsg += $"\n{localizationResourceManager["Height"]}";
            }
            if (TextValidator.IsNotValid)
            {
                errorMsg += $"\n{localizationResourceManager["FileName"]}";
            }
            if (!string.IsNullOrEmpty(errorMsg))
            {
                // put it in service maybe?
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