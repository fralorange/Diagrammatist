using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Diagrammatist.Presentation.WPF.Core.Models.ColorScheme;
using Diagrammatist.Presentation.WPF.Core.Foundation.Extensions;
using MvvmDialogs;
using System.Globalization;
using WPFLocalizeExtension.Engine;
using System.Windows.Media;

namespace Diagrammatist.Presentation.WPF.ViewModels.Dialogs
{
    /// <summary>
    /// A view model class for app settings management.
    /// </summary>
    public partial class SettingsDialogViewModel : ObservableValidator, IModalDialogViewModel
    {
        // if project grows then use snapshot pattern.
        private CultureInfo _initialLanguage;
        private ColorScheme _initialColorScheme;

        private readonly List<CultureInfo> _supportedCultures = new()
        {
            new CultureInfo("en-US"),
            new CultureInfo("ru-RU"),
        };

        [ObservableProperty]
        private CultureInfo[] _availableLanguages;

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

        private ColorScheme _selectedColorScheme;

        public ColorScheme SelectedColorScheme
        {
            get => _selectedColorScheme;
            set
            {
                if (SetProperty(ref _selectedColorScheme, value) && value is not null)
                {
                    ApplyColorScheme(value);
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
            SelectedColorScheme = new(App.Current.GetCurrentColorScheme()!);

            _initialLanguage = CultureInfo.CurrentUICulture;
            _initialColorScheme = new(App.Current.GetCurrentColorScheme()!);
        }

        private void ApplyLanguage(CultureInfo culture)
        {
            Thread.CurrentThread.CurrentUICulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
            LocalizeDictionary.Instance.SetCultureCommand.Execute(culture.ToString());
        }

        private void ApplyColorScheme(ColorScheme scheme)
        {
            foreach (var entry in scheme.ColorSchemeEntries)
            {
                UpdateResourceFromEntry(entry);

                entry.PropertyChanged += (s, e) =>
                {
                    if (e.PropertyName == nameof(ColorSchemeEntry.Hex))
                    {
                        UpdateResourceFromEntry(entry);
                    }
                };
            }
        }

        private void UpdateResourceFromEntry(ColorSchemeEntry entry)
        {
            App.Current.Resources[entry.Name] = ColorConverter.ConvertFromString(entry.Hex);
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
            SelectedColorScheme = _initialColorScheme;

            DialogResult = true;
        }
    }
}
