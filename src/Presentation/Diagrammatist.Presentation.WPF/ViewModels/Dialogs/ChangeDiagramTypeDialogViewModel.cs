using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Diagrammatist.Presentation.WPF.Core.Models.Canvas;
using Diagrammatist.Presentation.WPF.Core.Services.Alert;
using MvvmDialogs;

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
        /// Gets or sets color.
        /// </summary>
        [ObservableProperty]
        private DiagramsModel _diagramType;

        /// <summary>
        /// Initializes 'change diagram type' viewmodel.
        /// </summary>
        /// <param name="alertService"></param>
        /// <param name="diagramType"></param>
        public ChangeDiagramTypeDialogViewModel(IAlertService alertService, DiagramsModel diagramType)
        {
            _alertService = alertService;
            DiagramType = diagramType;
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
