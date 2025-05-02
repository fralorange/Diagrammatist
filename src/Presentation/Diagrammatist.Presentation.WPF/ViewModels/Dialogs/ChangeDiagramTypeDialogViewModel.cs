using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Diagrammatist.Presentation.WPF.Core.Models.Canvas;
using Diagrammatist.Presentation.WPF.Core.Services.Alert;
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
        public ChangeDiagramTypeDialogViewModel(IAlertService alertService, DiagramsModel diagramType)
        {
            _alertService = alertService;

            SelectedType = diagramType;

            AvailableTypes = new ObservableCollection<DiagramsModel>(Enum.GetValues(typeof(DiagramsModel)).Cast<DiagramsModel>());
        }

        /// <summary>
        /// Confirms changes.
        /// </summary>
        [RelayCommand]
        private void Ok()
        {
            // TO-DO: implement here _alertService RequestConfirmation with checkbox 'Don't show me again'.
            // Do it when custom alert modals will be implemented.

            DialogResult = true;
        }
    }
}
