using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Diagrammatist.Presentation.WPF.Core.Commands.Helpers.General;
using MvvmDialogs;
using System.Reflection;

namespace Diagrammatist.Presentation.WPF.ViewModels.Dialogs
{
    /// <summary>
    /// A view model class for showing user information about app.
    /// </summary>
    public partial class AboutAppDialogViewModel : ObservableObject, IModalDialogViewModel
    {
        /// <summary>
        /// Gets app title.
        /// </summary>
        public string AppTitle => Assembly.GetExecutingAssembly()
            .GetCustomAttribute<AssemblyTitleAttribute>()?.Title ?? string.Empty;

        /// <summary>
        /// Gets app version.
        /// </summary>
        public string AppVersion =>
            Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion.Split('+')[0] ?? string.Empty;

        /// <summary>
        /// Gets app author.
        /// </summary>
        public string AppAuthor =>
            App.Current.Resources["Author"] as string ?? string.Empty;

        private bool? _dialogResult;

        /// <inheritdoc/>
        public bool? DialogResult
        {
            get => _dialogResult;
            private set => SetProperty(ref _dialogResult, value);
        }

        /// <summary>
        /// Opens GitHub link.
        /// </summary>
        [RelayCommand]
        private void OpenLink()
        {
            var url = App.Current.Resources["HubUrl"] as string;

            UrlHelper.OpenUrl(url);
        }
    }
}
