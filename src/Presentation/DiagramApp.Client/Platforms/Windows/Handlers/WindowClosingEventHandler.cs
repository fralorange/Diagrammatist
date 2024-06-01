using DiagramApp.Client.ViewModels;
using LocalizationResourceManager.Maui;
using Microsoft.UI.Windowing;

namespace DiagramApp.Client.Platforms.Windows.Handlers
{
    public class WindowClosingEventHandler
    {
        private readonly ILocalizationResourceManager _localizationResourceManager;

        public WindowClosingEventHandler(ILocalizationResourceManager localizationResourceManager)
        {
            _localizationResourceManager = localizationResourceManager;
        }

        public async void OnWindowClosing(object sender, AppWindowClosingEventArgs e)
        {
            var _mainViewModel = Shell.Current.CurrentPage.BindingContext as MainViewModel;
            if (_mainViewModel is { CurrentCanvas.CanSave: true })
            {
                e.Cancel = true;

                var result = await DisplayMessage(_localizationResourceManager);

                if (result)
                {
                    App.Current!.Quit();
                }
            }
        }

        public static async Task<bool> DisplayMessage(ILocalizationResourceManager localizationResourceManager)
        {
            var warning = localizationResourceManager!["Warning"] + "!";
            var message = localizationResourceManager["UnsavedChanges"];
            var confirm = localizationResourceManager["Yes"];
            var cancel = localizationResourceManager["Cancel"];

            return await App.Current!.MainPage!.DisplayAlert(warning, message, confirm, cancel);
        }
    }
}
