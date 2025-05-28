using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Diagrammatist.Presentation.WPF.Core.Foundation.Extensions;
using Diagrammatist.Presentation.WPF.Core.Helpers;
using Diagrammatist.Presentation.WPF.Core.Managers.Command;
using Diagrammatist.Presentation.WPF.Core.Messaging.RequestMessages;
using Diagrammatist.Presentation.WPF.Core.Services.Alert;
using Diagrammatist.Presentation.WPF.Core.Services.Settings;
using Diagrammatist.Presentation.WPF.ViewModels.Components.Constants.Flags;
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
        private readonly IUserSettingsService _userSettingsService;
        private readonly IAlertService _alertService;
        private readonly ITrackableCommandManager _commandManager;

        private CultureInfo _initialLanguage;
        private string _initialTheme;
        private bool _initialSnapToGrid;
        private bool _initialAltGridSnap;
        private bool _initialDoNotShowWarningForDiagramType;

        private readonly List<CultureInfo> _supportedCultures =
        [
            new CultureInfo("en-US"),
            new CultureInfo("ru-RU"),
        ];

        private readonly List<string> _supportedThemes =
        [
            "System",
            "Light",
            "Dark"
        ];

        /// <summary>
        /// Gets or sets available languages.
        /// </summary>
        [ObservableProperty]
        private CultureInfo[] _availableLanguages;

        /// <summary>
        /// Gets or sets available themes.
        /// </summary>
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

        private bool _selectedSnapToGrid;

        /// <summary>
        /// Gets or sets selected snap to grid option.
        /// </summary>
        public bool SelectedSnapToGrid
        {
            get => _selectedSnapToGrid;
            set
            {
                if (SetProperty(ref _selectedSnapToGrid, value))
                {
                    CheckHasChanges();
                }
            }
        }

        private bool _selectedAltGridSnap;

        /// <summary>
        /// Gets or sets selected alter grid snap option.
        /// </summary>
        public bool SelectedAltGridSnap
        {
            get => _selectedAltGridSnap;
            set
            {
                if (SetProperty(ref _selectedAltGridSnap, value))
                {
                    CheckHasChanges();
                }
            }
        }

        // this naming and property management looks horrible, maybe fix in future?
        private bool _selectedDoNotShowWarningForDiagramType;

        /// <summary>
        /// Gets or sets selected do not show warning for diagram type option.
        /// </summary>
        public bool SelectedDoNotShowWarningForDiagramType
        {
            get => _selectedDoNotShowWarningForDiagramType;
            set
            {
                if (SetProperty(ref _selectedDoNotShowWarningForDiagramType, value))
                {
                    CheckHasChanges();
                }
            }
        }

        private bool? _dialogResult;

        /// <inheritdoc/>
        public bool? DialogResult
        {
            get => _dialogResult;
            private set => SetProperty(ref _dialogResult, value);
        }

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ApplyCommand))]
        private bool _hasChanges;

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsDialogViewModel"/> class.
        /// </summary>
        /// <param name="userSettingsService"></param>
#pragma warning disable CS8618
        public SettingsDialogViewModel(IUserSettingsService userSettingsService, IAlertService alertService, ITrackableCommandManager commandManager)
#pragma warning restore CS8618 
        {
            _userSettingsService = userSettingsService;
            _alertService = alertService;
            _commandManager = commandManager;

            AvailableLanguages = [.. _supportedCultures];
            AvailableThemes = [.. _supportedThemes];

            _initialLanguage = _userSettingsService.Get("Culture", CultureInfo.CurrentUICulture)!;
            _initialTheme = _userSettingsService.Get("Theme", "Light")!;
            _initialSnapToGrid = _userSettingsService.Get("SnapToGrid", true);
            _initialAltGridSnap = _userSettingsService.Get("AltGridSnap", true);
            _initialDoNotShowWarningForDiagramType = _userSettingsService.Get("DoNotShowChangeDiagramTypeWarning", false);

            SelectedLanguage = CultureInfo.CurrentUICulture;
            SelectedTheme = _initialTheme;
            SelectedSnapToGrid = _initialSnapToGrid;
            SelectedAltGridSnap = _initialAltGridSnap;
            SelectedDoNotShowWarningForDiagramType = _initialDoNotShowWarningForDiagramType;
        }

        private bool CanApply() => HasChanges;

        private void CheckHasChanges() => HasChanges =
            !_selectedLanguage.Equals(_initialLanguage)
            || _selectedTheme != _initialTheme
            || _selectedSnapToGrid != _initialSnapToGrid
            || _selectedAltGridSnap != _initialAltGridSnap
            || _selectedDoNotShowWarningForDiagramType != _initialDoNotShowWarningForDiagramType;

        private void ApplyLanguage(CultureInfo culture)
        {
            Thread.CurrentThread.CurrentUICulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
            LocalizeDictionary.Instance.SetCultureCommand.Execute(culture.ToString());
        }

        private void ApplyTheme(string theme)
        {
            App.Current.ChangeTheme(theme);

            if (_initialTheme != theme)
            {
                AdaptTheme();
            }
        }

        private void ApplySnapToGrid(bool snapToGrid)
        {
            WeakReferenceMessenger.Default.Send(new Tuple<string, bool>(ActionFlags.IsGridSnapEnabled, snapToGrid));
        }

        private void ApplyAltGridSnap(bool altGridSnap)
        {
            WeakReferenceMessenger.Default.Send(new Tuple<string, bool>(ActionFlags.IsAltGridSnapEnabled, altGridSnap));
        }

        private void AdaptTheme()
        {
            if (WeakReferenceMessenger.Default.Send(new CurrentCanvasRequestMessage()).Response is not { } canvas)
                return;

            ThemeAdaptHelper.AdaptTheme(canvas, _alertService, _commandManager);
        }

        /// <summary>
        /// Validates all properties and confirms changes.
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

            DialogResult = true;
        }

        /// <summary>
        /// Cancels changes and closes the dialog.
        /// </summary>
        [RelayCommand]
        private void Cancel()
        {
            SelectedLanguage = _initialLanguage;
            SelectedTheme = _initialTheme;

            DialogResult = true;
        }

        /// <summary>
        /// Applies the changes.
        /// </summary>
        [RelayCommand(CanExecute = nameof(CanApply))]
        private void Apply()
        {
            ApplyLanguage(SelectedLanguage);
            ApplyTheme(SelectedTheme);
            ApplySnapToGrid(SelectedSnapToGrid);
            ApplyAltGridSnap(SelectedAltGridSnap);

            _initialLanguage = SelectedLanguage;
            _initialTheme = SelectedTheme;
            _initialSnapToGrid = SelectedSnapToGrid;
            _initialAltGridSnap = SelectedAltGridSnap;
            _initialDoNotShowWarningForDiagramType = SelectedDoNotShowWarningForDiagramType;

            _userSettingsService.Set("Culture", SelectedLanguage.ToString());
            _userSettingsService.Set("Theme", SelectedTheme);
            _userSettingsService.Set("SnapToGrid", SelectedSnapToGrid);
            _userSettingsService.Set("AltGridSnap", SelectedAltGridSnap);
            _userSettingsService.Set("DoNotShowChangeDiagramTypeWarning", SelectedDoNotShowWarningForDiagramType);
            _userSettingsService.Save();

            HasChanges = false;
        }
    }
}
