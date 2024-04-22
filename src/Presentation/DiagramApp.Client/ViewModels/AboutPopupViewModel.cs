using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace DiagramApp.Client.ViewModels
{
    public partial class AboutPopupViewModel : ObservableObject
    {
        public string AppNameWithBuild { get; } = $"{AppInfo.Name} {AppInfo.BuildString}";
        public string Version { get; } = AppInfo.VersionString;

        [RelayCommand]
        private async Task TapAsync(string url) => await Launcher.OpenAsync(url);
    }
}
