using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Diagrammatist.Presentation.WPF.Core.Helpers;
using Diagrammatist.Presentation.WPF.Core.Models.Canvas;
using MvvmDialogs;
using System.ComponentModel.DataAnnotations;
using System.Windows.Media;

namespace Diagrammatist.Presentation.WPF.ViewModels.Dialogs
{
    /// <summary>
    /// A view model class for creating diagram dialog window.
    /// </summary>
    public partial class AddCanvasDialogViewModel : ObservableValidator, IModalDialogViewModel
    {
        private SettingsModel _settings;

        public SettingsModel? Settings => (DialogResult == true) ? _settings : null;

        private bool? _dialogResult;

        public bool? DialogResult
        {
            get => _dialogResult;
            private set => SetProperty(ref _dialogResult, value);
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

        public AddCanvasDialogViewModel()
        {
            _settings = SettingsInitializationHelper.InitializeDefaultSettings();
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
