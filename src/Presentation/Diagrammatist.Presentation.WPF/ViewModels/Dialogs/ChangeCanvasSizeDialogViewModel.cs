using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MvvmDialogs;
using System.ComponentModel.DataAnnotations;
using System.Windows;

namespace Diagrammatist.Presentation.WPF.ViewModels.Dialogs
{
    /// <summary>
    /// A class that represents viewmodel of change canvas size dialog window.
    /// </summary>
    public partial class ChangeCanvasSizeDialogViewModel : ObservableValidator, IModalDialogViewModel
    {
        private Size _size;

        /// <summary>
        /// Gets result size.
        /// </summary>
        public Size? Size => (DialogResult == true) ? _size : null;

        private bool? _dialogResult;

        /// <inheritdoc/>
        public bool? DialogResult
        {
            get => _dialogResult;
            private set => SetProperty(ref _dialogResult, value);
        }

        /// <summary>
        /// Gets or sets current width.
        /// </summary>
        [Required]
        [Range(400, 2000)]
        [ObservableProperty]
        private double _width;

        /// <summary>
        /// Gets or sets current height.
        /// </summary>
        [Required]
        [Range(300, 2000)]
        [ObservableProperty]
        private double _height;

        public ChangeCanvasSizeDialogViewModel(double width, double height)
        {
            Width = width;
            Height = height;
        }

        private void ApplySize()
        {
            _size = new(Width, Height);
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

            ApplySize();

            DialogResult = true;
        }
    }
}
