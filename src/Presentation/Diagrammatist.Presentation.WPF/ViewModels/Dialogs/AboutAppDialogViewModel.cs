using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Diagrammatist.Presentation.WPF.Core.Commands.Helpers.General;
using MvvmDialogs;
using System.Reflection;

namespace Diagrammatist.Presentation.WPF.ViewModels.Dialogs
{
    public partial class AboutAppDialogViewModel : ObservableObject, IModalDialogViewModel
    {
        public string AppTitle => Assembly.GetExecutingAssembly()
            .GetCustomAttribute<AssemblyTitleAttribute>()?.Title ?? string.Empty;

        public string AppVersion =>
            Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion.Split('+')[0] ?? string.Empty;

        public string AppAuthor =>
            App.Current.Resources["Author"] as string ?? string.Empty;

        private bool? _dialogResult;

        public bool? DialogResult
        {
            get => _dialogResult;
            private set => SetProperty(ref _dialogResult, value);
        }

        [RelayCommand]
        private void OpenLink()
        {
            var url = App.Current.Resources["HubUrl"] as string;

            UrlHelper.OpenUrl(url);
        }
    }
}
