using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Messaging;
using DiagramApp.Client.ViewModels;
using DiagramApp.Domain.DiagramSettings;

namespace DiagramApp.Client.Views;

public partial class ChangeDiagramSizePopupView : Popup
{
	public ChangeDiagramSizePopupView(ChangeDiagramSizePopupViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;

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
                await Shell.Current.DisplayAlert("Ошибка", $"Требуется заполнить следующие поля корректно:{errorMsg}.", "OK");
                return;
            }

            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            await CloseAsync(settings, cts.Token);
        });
    }

    private async void OnCancelClicked(object sender, EventArgs e) => await CloseAsync();
}