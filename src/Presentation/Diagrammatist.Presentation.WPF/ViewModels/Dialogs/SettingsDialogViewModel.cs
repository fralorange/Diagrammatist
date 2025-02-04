using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Diagrammatist.Presentation.WPF.Core.Foundation.Extensions;
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
        private string _initialTheme;

        private readonly List<CultureInfo> _supportedCultures = new()
        {
            new CultureInfo("en-US"),
            new CultureInfo("ru-RU"),
        };

        private readonly List<string> _supportedThemes = new()
        {
            "System",
            "Light",
            "Dark"
        };

        [ObservableProperty]
        private CultureInfo[] _availableLanguages;

        [ObservableProperty]
        private string[] _availableThemes;

        private CultureInfo _selectedLanguage;

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
                if (SetProperty(ref _selectedLanguage, value) && value is not null)
                {
                    ApplyLanguage(value);
                }
            }
        }

        private string _selectedTheme;

        /// <summary>
        /// Gets or sets selected theme.
        /// </summary>
        /// <remarks>
        /// This property used to define current app theme.
        /// </remarks>
        public string SelectedTheme
        {
            get => _selectedTheme;
            set
            {
                if (SetProperty(ref _selectedTheme, value) && value is not null)
                {
                    ApplyTheme(value);
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
            AvailableThemes = _supportedThemes.ToArray();

            SelectedLanguage = CultureInfo.CurrentUICulture;
            SelectedTheme = Properties.Settings.Default.Theme;

            _initialLanguage = CultureInfo.CurrentUICulture;
            _initialTheme = Properties.Settings.Default.Theme;
        }

        private void ApplyLanguage(CultureInfo culture)
        {
            Thread.CurrentThread.CurrentUICulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
            LocalizeDictionary.Instance.SetCultureCommand.Execute(culture.ToString());
        }

        private void ApplyTheme(string theme)
        {
            App.Current.ChangeTheme(theme);
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
            Properties.Settings.Default.Theme = SelectedTheme;
            Properties.Settings.Default.Save();

            DialogResult = true;
        }

        [RelayCommand]
        private void Cancel()
        {
            SelectedLanguage = _initialLanguage;
            SelectedTheme = _initialTheme;

            DialogResult = true;
        }
    }
}
