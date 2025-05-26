using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Diagrammatist.Presentation.WPF.Core.Shared.Enums;
using MvvmDialogs;
using System.Collections.ObjectModel;

namespace Diagrammatist.Presentation.WPF.ViewModels.Dialogs
{
    /// <summary>
    /// A view model class for exporting diagrams.
    /// </summary>
    public partial class ExportDiagramDialogViewModel : ObservableValidator, IModalDialogViewModel
    {
        /// <summary>
        /// Gets the collection of export scenarios available for selection.
        /// </summary>
        public ObservableCollection<ExportScenario> ExportScenarios { get; }

        /// <summary>
        /// Gets or sets the selected export scenario.
        /// </summary>
        [ObservableProperty]
        private ExportScenario selectedExportScenario = ExportScenario.Full;

        /// <summary>
        /// Gets or sets the content margin.
        /// </summary>
        [ObservableProperty]
        private int contentMargin = 10;

        /// <summary>
        /// Gets the collection of export PPI (Pixels Per Inch) values available for selection.
        /// </summary>
        public ObservableCollection<ExportPPI> ExportPPIValues { get; }

        private ExportPPI selectedPpi = ExportPPI.PPI_72;

        /// <summary>
        /// Gets or sets the selected PPI value for exporting diagrams.
        /// </summary>
        public ExportPPI SelectedPpi
        {
            get => selectedPpi;
            set
            {
                SetProperty(ref selectedPpi, value);
                if (value is not ExportPPI.PPI_Custom)
                {
                    CustomPpi = (int)SelectedPpi;
                }
            }
        }

        /// <summary>
        /// Gets or sets the custom PPI value for exporting diagrams.
        /// </summary>
        [ObservableProperty]
        private int customPpi = 72;

        private bool? _dialogResult;

        /// <inheritdoc/>
        public bool? DialogResult
        {
            get => _dialogResult;
            private set => SetProperty(ref _dialogResult, value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExportDiagramDialogViewModel"/> class.
        /// </summary>
        public ExportDiagramDialogViewModel()
        {
            ExportScenarios = new ObservableCollection<ExportScenario>(Enum.GetValues(typeof(ExportScenario)).Cast<ExportScenario>());
            ExportPPIValues = new ObservableCollection<ExportPPI>(Enum.GetValues(typeof(ExportPPI)).Cast<ExportPPI>());
        }

        /// <summary>
        /// Command to confirm the export settings and close the dialog with a positive result.
        /// </summary>
        [RelayCommand]
        private void Ok()
        {
            DialogResult = true;
        }
    }
}
