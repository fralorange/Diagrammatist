using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Diagrammatist.Presentation.WPF.Core.Helpers;
using Diagrammatist.Presentation.WPF.Core.Models.Canvas;
using Diagrammatist.Presentation.WPF.Core.Services.Alert;
using Diagrammatist.Presentation.WPF.Core.Services.Settings;
using Diagrammatist.Presentation.WPF.Core.Shared.Enums;
using MvvmDialogs;
using System.Collections.ObjectModel;

namespace Diagrammatist.Presentation.WPF.ViewModels.Dialogs
{
    /// <summary>
    /// A class that represents viewmodel of change diagram type dialog window.
    /// </summary>
    public partial class ChangeDiagramTypeDialogViewModel : ObservableObject, IModalDialogViewModel
    {
        private readonly IAlertService _alertService;
        private readonly IUserSettingsService _userSettingsService;

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
        public ObservableCollection<DiagramsModel> AvailableTypes { get; }

        private DiagramsModel _selectedType;

        /// <summary>
        /// Gets or sets selected preset.
        /// </summary>
        public DiagramsModel SelectedType
        {
            get => _selectedType;
            set
            {
                _selectedType = value;
                OnPropertyChanged(nameof(SelectedTypeDescription));
            }
        }

        /// <summary>
        /// Gets selected type description.
        /// </summary>
        public string SelectedTypeDescription 
            => $"{SelectedType}Desc";

        /// <summary>
        /// Initializes 'change diagram type' viewmodel.
        /// </summary>
        /// <param name="alertService"></param>
        /// <param name="diagramType"></param>
        public ChangeDiagramTypeDialogViewModel(IAlertService alertService,
                                                IUserSettingsService userSettingsService,
                                                DiagramsModel diagramType)
        {
            _alertService = alertService;
            _userSettingsService = userSettingsService;

            SelectedType = diagramType;

            AvailableTypes = new ObservableCollection<DiagramsModel>(Enum.GetValues(typeof(DiagramsModel)).Cast<DiagramsModel>());
        }

        /// <summary>
        /// Confirms changes.
        /// </summary>
        [RelayCommand]
        private void Ok()
        {
            var skipWarning = _userSettingsService.Get<bool>("DoNotShowChangeDiagramTypeWarning");

            if (skipWarning)
            {
                DialogResult = true;
                return;
            }

            var localizedCaption = LocalizationHelper
                .GetLocalizedValue<string>("Dialogs.ChangeDiagramType.ChangeDiagramTypeResources", "WarningCaption");
            var localizedMessage = LocalizationHelper
                .GetLocalizedValue<string>("Dialogs.ChangeDiagramType.ChangeDiagramTypeResources", "WarningMessage");

            var response = _alertService.ShowWarning(localizedMessage, localizedCaption);

            if (response.DoNotShowAgain)
            {
                _userSettingsService.Set("DoNotShowChangeDiagramTypeWarning", true);
            }

            if (response.Result == ConfirmationResult.Yes)
            {
                DialogResult = true;
            }
        }
    }
}
