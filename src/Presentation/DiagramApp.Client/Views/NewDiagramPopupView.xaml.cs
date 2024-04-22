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
            if (WidthValidator.IsNotValid)
            {
                errorMsg += "\nШирина";
            }
            if (HeightValidator.IsNotValid)
            {
                errorMsg += "\nВысота";
            }
            if (TextValidator.IsNotValid)
            {
                errorMsg += "\nИмя файла";
            }
            if (!string.IsNullOrEmpty(errorMsg))
            {
                // put it in service maybe?
                await Shell.Current.DisplayAlert("Ошибка", $"Требуется заполнить следующие поля корректно:{errorMsg}.", "OK");
                return;
            }    

            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            await CloseAsync(settings, cts.Token);
        });
    }

    private async void OnCancelClicked(object sender, EventArgs e) => await CloseAsync();
}