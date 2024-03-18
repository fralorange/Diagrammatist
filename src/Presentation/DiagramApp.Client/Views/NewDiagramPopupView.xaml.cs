using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Messaging;
using DiagramApp.Client.ViewModels;
using DiagramApp.Domain.DiagramSettings;

namespace DiagramApp.Client.Views;

public partial class NewDiagramPopupView : Popup
{
    public NewDiagramPopupView(NewDiagramPopupViewmodel viewmodel)
    {
        InitializeComponent();

        BindingContext = viewmodel;

        WeakReferenceMessenger.Default.Register<DiagramSettings>(this, async (r, settings) =>
        {
            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            await CloseAsync(settings, cts.Token);
        });
    }

    private async void Cancel_Clicked(object sender, EventArgs e)
    {
        await CloseAsync();
    }
}