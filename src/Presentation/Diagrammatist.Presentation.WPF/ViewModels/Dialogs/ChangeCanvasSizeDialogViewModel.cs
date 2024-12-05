using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Diagrammatist.Presentation.WPF.Models.Canvas;
using MvvmDialogs;
using System.ComponentModel.DataAnnotations;

namespace Diagrammatist.Presentation.WPF.ViewModels.Dialogs
{
    public partial class ChangeCanvasSizeDialogViewModel : ObservableValidator, IModalDialogViewModel
    {
        private SettingsModel _settings;

        public SettingsModel? Settings => (DialogResult == true) ? _settings : null;

        private bool? _dialogResult;

        public bool? DialogResult
        {
            get => _dialogResult;
            private set => SetProperty(ref _dialogResult, value);
        }

        /// <inheritdoc cref="DiagramSettingsDto.Width"/>
        [Required]
        [Range(400, 2000)]
        public int Width
        {
            get => _settings.Width;
            set => SetProperty(_settings.Width, value, _settings, (s, w) => s.Width = w, true);
        }

        /// <inheritdoc cref="DiagramSettingsDto.Height"/>
        [Required]
        [Range(300, 2000)]
        public int Height
        {
            get => _settings.Height;
            set => SetProperty(_settings.Height, value, _settings, (s, h) => s.Height = h, true);
        }

        public ChangeCanvasSizeDialogViewModel(SettingsModel settings)
        {
            _settings = settings;
        }

        [RelayCommand]
        private void Ok()
        {
            ValidateAllProperties();

            if (HasErrors)
            {
                return;
            }

            DialogResult = true;
        }
    }
}
