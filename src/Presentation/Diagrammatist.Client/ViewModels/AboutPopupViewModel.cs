using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LocalizationResourceManager.Maui;

namespace DiagramApp.Client.ViewModels
{
    public partial class AboutPopupViewModel : ObservableObject
    {
        private readonly LocalizedString _buildLocalizedString;

        public string AppNameWithBuild { get; } = $"{AppInfo.Name} {AppInfo.BuildString}";
        public string Version { get; } = AppInfo.VersionString;
        public string VersionWithBuildString { get; }

        public AboutPopupViewModel(ILocalizationResourceManager localizationResourceManager)
        {
            _buildLocalizedString = new(() => localizationResourceManager["Build"]);
            VersionWithBuildString = string.Format(_buildLocalizedString.Localized, Version);
        }

        [RelayCommand]
        private async Task TapAsync(string url) => await Launcher.OpenAsync(url);
    }
}
