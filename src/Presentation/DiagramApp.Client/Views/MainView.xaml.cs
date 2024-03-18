using CommunityToolkit.Maui.Core;
using DiagramApp.Client.ViewModels;

namespace DiagramApp.Client
{
    public partial class MainView : ContentPage
    {
        private readonly IPopupService _popupService;

        public MainView(IPopupService popupService)
        {
            InitializeComponent();

            _popupService = popupService;
        }

        private async void New_Clicked(object sender, EventArgs e)
        {
            var result = await _popupService.ShowPopupAsync<NewDiagramPopupViewmodel>(CancellationToken.None);
        }
    }
}
