using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MvvmDialogs;
using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace Diagrammatist.Presentation.WPF.ViewModels.Dialogs
{
    public partial class ChangeCanvasSizeDialogViewModel : ObservableValidator, IModalDialogViewModel
    {
        private Size _size;

        public Size? Size => (DialogResult == true) ? _size : null;

        private bool? _dialogResult;

        public bool? DialogResult
        {
            get => _dialogResult;
            private set => SetProperty(ref _dialogResult, value);
        }

        [Required]
        [Range(400, 2000)]
        [ObservableProperty]
        private int _width;

        [Required]
        [Range(300, 2000)]
        [ObservableProperty]
        private int _height;

        public ChangeCanvasSizeDialogViewModel(int width, int height)
        {
            Width = width;
            Height = height;
        }

        private void ApplySize()
        {
            _size = new(Width, Height);
        }

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
