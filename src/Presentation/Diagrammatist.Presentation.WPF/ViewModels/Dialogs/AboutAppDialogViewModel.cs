using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Diagrammatist.Presentation.WPF.Core.Helpers;
using MvvmDialogs;

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
        public string AppTitle => AppMetadata.Title;

        /// <summary>
        /// Gets app version.
        /// </summary>
        public string AppVersion => AppMetadata.Version;

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
