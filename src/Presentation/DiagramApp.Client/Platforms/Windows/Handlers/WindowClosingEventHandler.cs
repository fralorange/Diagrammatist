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

                await ProcessMessage(_localizationResourceManager);
            }
        }

        public static async Task ProcessMessage(ILocalizationResourceManager localizationResourceManager)
        {
            var warning = localizationResourceManager!["Warning"] + "!";
            var message = localizationResourceManager["UnsavedChanges"];
            var confirm = localizationResourceManager["Yes"];
            var cancel = localizationResourceManager["Cancel"];

            bool result = await App.Current!.MainPage!.DisplayAlert(warning, message, confirm, cancel);

            if (result)
            {
                App.Current.Quit();
            }
        }
    }
}
