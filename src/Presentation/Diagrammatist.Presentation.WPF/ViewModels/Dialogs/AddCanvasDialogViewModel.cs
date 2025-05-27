using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Diagrammatist.Presentation.WPF.Core.Foundation.Extensions;
using Diagrammatist.Presentation.WPF.Core.Helpers;
using Diagrammatist.Presentation.WPF.Core.Models.Canvas;
using Diagrammatist.Presentation.WPF.Core.Shared.Enums;
using MvvmDialogs;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Media;

namespace Diagrammatist.Presentation.WPF.ViewModels.Dialogs
{
    /// <summary>
    /// Enum for background types.
    /// </summary>
    public enum BackgroundType { WhiteBG, BlackBG, CustomBG }

    /// <summary>
    /// A class that derives from <see cref="ObservableValidator"/>.
    /// This class represents view model that manages the dialog for adding a canvas.
    /// </summary>
    public partial class AddCanvasDialogViewModel : ObservableValidator, IModalDialogViewModel
    {
        private SettingsModel _settings;

        public SettingsModel? Settings => (DialogResult == true) ? _settings : null;

        private bool? _dialogResult;

        /// <inheritdoc/>
        public bool? DialogResult
        {
            get => _dialogResult;
            private set => SetProperty(ref _dialogResult, value);
        }

        /// <summary>
        /// Gets selected presets.
        /// </summary>
        public ObservableCollection<DiagramsModel> AvailablePresets { get; }
        /// <summary>
        /// Gets or sets selected preset.
        /// </summary>
        public DiagramsModel SelectedPreset
        {
            get => DiagramType;
            set
            {
                DiagramType = value;
            }
        }

        /// <inheritdoc cref="DiagramSettingsDto.FileName"/>
        [Required]
        [MinLength(1)]
        [MaxLength(128)]
        public string FileName
        {
            get => _settings.FileName;
            set => SetProperty(_settings.FileName, value, _settings, (s, f) => s.FileName = f, true);
        }

        /// <summary>
        /// Gets the selected measurement unit options.
        /// </summary>
        public ObservableCollection<MeasurementUnit> UnitOptions { get; }

        private MeasurementUnit _selectedUnit;

        /// <summary>
        /// Gets or sets the selected measurement unit.
        /// </summary>
        public MeasurementUnit SelectedUnit
        {
            get => _selectedUnit;
            set
            {
                if (SetProperty(ref _selectedUnit, value))
                {
                    OnPropertyChanged(nameof(Width));
                    OnPropertyChanged(nameof(Height));
                }
            }
        }

        private double _widthPx;
        private double _heightPx;

        /// <inheritdoc cref="DiagramSettingsDto.Width"/>
        [Required]
        [CustomValidation(typeof(AddCanvasDialogViewModel), nameof(ValidateWidth))]
        public double Width
        {
            get => Math.Round(UnitConvertHelper.FromPixels(_widthPx, SelectedUnit), 3);
            set
            {
                var newPx = UnitConvertHelper.ToPixels(value, SelectedUnit);
                if (SetProperty(ref _widthPx, newPx))
                {
                    ValidateProperty(_widthPx, nameof(Width));
                }
            }
        }

        /// <inheritdoc cref="DiagramSettingsDto.Height"/>
        [Required]
        [CustomValidation(typeof(AddCanvasDialogViewModel), nameof(ValidateHeight))]
        public double Height
        {
            get => Math.Round(UnitConvertHelper.FromPixels(_heightPx, SelectedUnit), 3);
            set
            {
                var newPx = UnitConvertHelper.ToPixels(value, SelectedUnit);
                if (SetProperty(ref _heightPx, newPx))
                {
                    ValidateProperty(_heightPx, nameof(Height));
                }
            }
        }

        /// <summary>
        /// Gets the selected measurement unit.
        /// </summary>
        public ObservableCollection<BackgroundType> BackgroundOptions { get; }

        private bool _suppressColorChange;
        private BackgroundType _selectedBackgroundType;

        /// <summary>
        /// Gets or sets the selected background type.
        /// </summary>
        public BackgroundType SelectedBackgroundType
        {
            get => _selectedBackgroundType;
            set
            {
                if (SetProperty(ref _selectedBackgroundType, value))
                {
                    _suppressColorChange = true;

                    Background = value switch
                    {
                        BackgroundType.WhiteBG => (Color)ColorConverter.ConvertFromString("#FFF5F5F5"),
                        BackgroundType.BlackBG => (Color)ColorConverter.ConvertFromString("#FF1C1C1C"),
                        _ => Background,
                    };
                    _suppressColorChange = false;
                }
            }
        }

        /// <inheritdoc cref="DiagramSettingsDto.Background"/>
        [Required]
        public Color Background
        {
            get => _settings.Background;
            set => SetProperty(_settings.Background, value, _settings, (s, b) => s.Background = b, true);
        }

        /// <inheritdoc cref="DiagramSettingsDto.Type"/>
        [Required]
        public DiagramsModel DiagramType
        {
            get => _settings.Type;
            set => SetProperty(_settings.Type, value, _settings, (s, t) => s.Type = t, true);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AddCanvasDialogViewModel"/> class.
        /// </summary>
        public AddCanvasDialogViewModel()
        {
            _settings = SettingsInitializationHelper.InitializeDefaultSettings();

            _widthPx = _settings.Width;
            _heightPx = _settings.Height;

            AvailablePresets = new ObservableCollection<DiagramsModel>(Enum.GetValues(typeof(DiagramsModel)).Cast<DiagramsModel>());

            UnitOptions = new ObservableCollection<MeasurementUnit>(Enum.GetValues(typeof(MeasurementUnit)).Cast<MeasurementUnit>());
            SelectedUnit = MeasurementUnit.Pixels;

            BackgroundOptions = new ObservableCollection<BackgroundType>(Enum.GetValues(typeof(BackgroundType)).Cast<BackgroundType>());
            SelectedBackgroundType = (App.Current.GetCurrentTheme() == "Light") ? BackgroundType.WhiteBG : BackgroundType.BlackBG;
            Background = ThemeColorHelper.GetBackgroundColor();
        }

        /// <summary>
        /// Changes the background color of the canvas.
        /// </summary>
        [RelayCommand]
        private void ColorChange()
        {
            if (_suppressColorChange)
                return;

            SelectedBackgroundType = BackgroundType.CustomBG;
        }

        /// <summary>
        /// Validates all properties of the view model and confirms changes.
        /// </summary>
        [RelayCommand]
        private void Ok()
        {
            ValidateAllProperties();

            if (HasErrors)
            {
                return;
            }

            _settings.Width = Math.Round(_widthPx);
            _settings.Height = Math.Round(_heightPx);

            DialogResult = true;
        }

        /// <summary>
        /// Validates the width properties.
        /// </summary>
        /// <param name="size"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static ValidationResult? ValidateWidth(double size, ValidationContext context)
        {
            var instance = (AddCanvasDialogViewModel)context.ObjectInstance;
            var widthPx = instance._widthPx;

            if (widthPx < 512 || widthPx > 4000)
            {
                return new ValidationResult("Width must be between 512 and 4000.", [nameof(Width)]);
            }

            return ValidationResult.Success;
        }

        /// <summary>
        /// Validates the height properties.
        /// </summary>
        /// <param name="size"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static ValidationResult? ValidateHeight(double size, ValidationContext context)
        {
            var instance = (AddCanvasDialogViewModel)context.ObjectInstance;
            var heightPx = instance._heightPx;

            if (heightPx < 512 || heightPx > 4000)
            {
                return new ValidationResult("Height must be between 512 and 4000.", [nameof(Height)]);
            }

            return ValidationResult.Success;
        }
    }
}
