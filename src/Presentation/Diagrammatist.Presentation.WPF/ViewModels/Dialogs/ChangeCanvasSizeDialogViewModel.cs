using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Diagrammatist.Presentation.WPF.Core.Helpers;
using Diagrammatist.Presentation.WPF.Core.Shared.Enums;
using MvvmDialogs;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Windows;

namespace Diagrammatist.Presentation.WPF.ViewModels.Dialogs
{
    /// <summary>
    /// A class that represents viewmodel of change canvas size dialog window.
    /// </summary>
    public partial class ChangeCanvasSizeDialogViewModel : ObservableValidator, IModalDialogViewModel
    {
        private readonly double _initialWidthPx;

        /// <summary>
        /// Gets the initial width in pixels.
        /// </summary>
        public double InitialWidthPx => _initialWidthPx;

        private readonly double _initialHeightPx;

        /// <summary>
        /// Gets the initial height in pixels.
        /// </summary>
        public double InitialHeightPx => _initialHeightPx;

        private Size _size;
        private bool? _dialogResult;

        /// <summary>
        /// Gets the selected measurement unit options.
        /// </summary>
        public ObservableCollection<ChangeUnit> UnitOptions { get; }

        private ChangeUnit _selectedUnit;

        /// <summary>
        /// Gets or sets the selected measurement unit.
        /// </summary>
        public ChangeUnit SelectedUnit
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

        /// <summary>
        /// Gets the width of the canvas in pixels.
        /// </summary>
        public double WidthPx => Math.Round(_widthPx, 3);

        private double _heightPx;

        /// <summary>
        /// Gets the height of the canvas in pixels.
        /// </summary>
        public double HeightPx => Math.Round(_heightPx, 3);

        /// <summary>
        /// Gets or sets the width of the canvas.
        /// </summary>
        [Required]
        [CustomValidation(typeof(ChangeCanvasSizeDialogViewModel), nameof(ValidateWidth))]
        public double Width
        {
            get => Math.Round(ChangeConvertUnit.FromPixels(_widthPx, SelectedUnit, _initialWidthPx), 3);
            set
            {
                var newPx = ChangeConvertUnit.ToPixels(value, SelectedUnit, _initialWidthPx);
                if (SetProperty(ref _widthPx, newPx))
                {
                    ValidateProperty(_widthPx, nameof(Width));
                    OnPropertyChanged(nameof(WidthPx));
                }
            }
        }

        /// <summary>
        /// Gets or sets the height of the canvas.
        /// </summary>
        [Required]
        [CustomValidation(typeof(ChangeCanvasSizeDialogViewModel), nameof(ValidateHeight))]
        public double Height
        {
            get => Math.Round(ChangeConvertUnit.FromPixels(_heightPx, SelectedUnit, _initialWidthPx), 3);
            set
            {
                var newPx = ChangeConvertUnit.ToPixels(value, SelectedUnit, _initialWidthPx);
                if (SetProperty(ref _heightPx, newPx))
                {
                    ValidateProperty(_heightPx, nameof(Height));
                    OnPropertyChanged(nameof(HeightPx));
                }
            }
        }

        /// <summary>
        /// Gets the resulting canvas size.
        /// </summary>
        public Size? Size => (DialogResult == true) ? _size : null;

        /// <inheritdoc/>
        public bool? DialogResult
        {
            get => _dialogResult;
            private set => SetProperty(ref _dialogResult, value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeCanvasSizeDialogViewModel"/> class.
        /// </summary>
        /// <param name="currentWidthPx"></param>
        /// <param name="currentHeightPx"></param>
        public ChangeCanvasSizeDialogViewModel(double currentWidthPx, double currentHeightPx)
        {
            _initialWidthPx = currentWidthPx;
            _initialHeightPx = currentHeightPx;

            UnitOptions = new ObservableCollection<ChangeUnit>(Enum.GetValues(typeof(ChangeUnit)).Cast<ChangeUnit>());
            SelectedUnit = ChangeUnit.Pixels;

            Width = ChangeConvertUnit.FromPixels(_initialWidthPx, SelectedUnit, _initialWidthPx);
            Height = ChangeConvertUnit.FromPixels(_initialHeightPx, SelectedUnit, _initialHeightPx);
        }

        /// <summary>
        /// Validates all properties of the view model and confirm changes.
        /// </summary>
        [RelayCommand]
        private void Ok()
        {
            ValidateAllProperties();

            if (HasErrors) return;

            ApplySize();
            DialogResult = true;
        }

        private void ApplySize()
        {
            double newW = ChangeConvertUnit.ToPixels(Width, SelectedUnit, _initialWidthPx);
            double newH = ChangeConvertUnit.ToPixels(Height, SelectedUnit, _initialHeightPx);

            _size = new Size(newW, newH);
        }

        /// <summary>
        /// Validates the width properties.
        /// </summary>
        /// <param name="size"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static ValidationResult? ValidateWidth(double size, ValidationContext context)
        {
            var instance = (ChangeCanvasSizeDialogViewModel)context.ObjectInstance;
            var widthPx = instance._widthPx;

            if (widthPx < 300 || widthPx > 4000)
            {
                return new ValidationResult("Width must be between 300 and 4000.", [nameof(Width)]);
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
            var instance = (ChangeCanvasSizeDialogViewModel)context.ObjectInstance;
            var heightPx = instance._heightPx;

            if (heightPx < 300 || heightPx > 4000)
            {
                return new ValidationResult("Height must be between 300 and 4000.", [nameof(Height)]);
            }

            return ValidationResult.Success;
        }
    }
}
