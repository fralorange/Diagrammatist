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
                    CheckHasChanges();
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
                    CheckHasChanges();
                }
            }
        }

        private bool? _dialogResult;

        public bool? DialogResult
        {
            get => _dialogResult;
            private set => SetProperty(ref _dialogResult, value);
        }

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ApplyCommand))]
        private bool _hasChanges;

#pragma warning disable CS8618 
        public SettingsDialogViewModel()
#pragma warning restore CS8618 
        {
            AvailableLanguages = _supportedCultures.ToArray();
            AvailableThemes = _supportedThemes.ToArray();

            _initialLanguage = CultureInfo.CurrentUICulture;
            _initialTheme = Properties.Settings.Default.Theme;

            SelectedLanguage = CultureInfo.CurrentUICulture;
            SelectedTheme = Properties.Settings.Default.Theme;
        }

        private bool CanApply() => HasChanges;

        private void CheckHasChanges() => HasChanges =
            !_selectedLanguage.Equals(_initialLanguage) ||
            _selectedTheme != _initialTheme;

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

        /// <summary>
        /// Confirms changes.
        /// </summary>
        [RelayCommand]
        private void Ok()
        {
            ValidateAllProperties();

            if (HasErrors)
            {
                return;
            }

            Apply();

            Properties.Settings.Default.Culture = SelectedLanguage.Name;
            Properties.Settings.Default.Theme = SelectedTheme;
            Properties.Settings.Default.Save();

            DialogResult = true;
        }

        /// <summary>
        /// Cancels changes.
        /// </summary>
        [RelayCommand]
        private void Cancel()
        {
            SelectedLanguage = _initialLanguage;
            SelectedTheme = _initialTheme;

            DialogResult = true;
        }

        /// <summary>
        /// Applies changes.
        /// </summary>
        [RelayCommand(CanExecute = nameof(CanApply))]
        private void Apply()
        {
            ApplyLanguage(SelectedLanguage);
            ApplyTheme(SelectedTheme);

            _initialLanguage = SelectedLanguage;
            _initialTheme = SelectedTheme;
            HasChanges = false;
        }
    }
}
