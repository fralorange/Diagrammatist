using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Messaging;
using DiagramApp.Client.ViewModels;
using DiagramApp.Domain.DiagramSettings;

namespace DiagramApp.Client.Views;

public partial class NewDiagramPopupView : Popup
{
    public NewDiagramPopupView(NewDiagramPopupViewModel viewmodel)
    {
        InitializeComponent();

        BindingContext = viewmodel;

        WeakReferenceMessenger.Default.Register<DiagramSettings>(this, async (r, settings) =>
        {
            string errorMsg = "";
            if (widthValidator.IsNotValid)
            {
                errorMsg += "\nШирина";
            }
            if (heightValidator.IsNotValid)
            {
                errorMsg += "\nВысота";
            }
            if (!string.IsNullOrEmpty(errorMsg))
            {
                await Shell.Current.DisplayAlert("Ошибка", $"Требуется заполнить следующие поля:{errorMsg}.", "OK");
                return;
            }    

            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            await CloseAsync(settings, cts.Token);
        });
    }

    private async void OnCancelClicked(object sender, EventArgs e) => await CloseAsync();
}