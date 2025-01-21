using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MvvmDialogs;
using System.Globalization;
using WPFLocalizeExtension.Engine;

namespace Diagrammatist.Presentation.WPF.ViewModels.Dialogs
{
    /// <summary>
    /// A view model class for app settings management.
    /// </summary>
    public partial class SettingsDialogViewModel : ObservableValidator, IModalDialogViewModel
    {
        // if project grows then use snapshot pattern.
        private CultureInfo _initialLanguage;

        private CultureInfo _selectedLanguage;
        private readonly List<CultureInfo> _supportedCultures = new()
        {
            new CultureInfo("en-US"),
            new CultureInfo("ru-RU"),
        };

        [ObservableProperty]
        private CultureInfo[] _availableLanguages;

        /// <summary>
        /// Gets or sets selected language.
        /// </summary>
        /// <remarks>
        /// This property used to define current culture.
        /// </remarks>
        public CultureInfo SelectedLanguage
        {
            get => _selectedLanguage;
            set
            {
                if (SetProperty(ref _selectedLanguage, value) && value != null)
                {
                    ApplyLanguage(value);
                }
            }
        }

        private bool? _dialogResult;

        public bool? DialogResult
        {
            get => _dialogResult;
            private set => SetProperty(ref _dialogResult, value);
        }

#pragma warning disable CS8618 
        public SettingsDialogViewModel()
#pragma warning restore CS8618 
        {
            AvailableLanguages = _supportedCultures.ToArray();
            SelectedLanguage = CultureInfo.CurrentUICulture;

            _initialLanguage = CultureInfo.CurrentUICulture;
        }

        private void ApplyLanguage(CultureInfo culture)
        {
            Thread.CurrentThread.CurrentUICulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
            LocalizeDictionary.Instance.SetCultureCommand.Execute(culture.ToString());
        }

        [RelayCommand]
        private void Ok()
        {
            ValidateAllProperties();

            if (HasErrors)
            {
                return;
            }

            Properties.Settings.Default.Culture = SelectedLanguage.Name;
            Properties.Settings.Default.Save();
            
            DialogResult = true;
        }

        [RelayCommand]
        private void Cancel()
        {
            SelectedLanguage = _initialLanguage;

            DialogResult = true;
        }
    }
}
