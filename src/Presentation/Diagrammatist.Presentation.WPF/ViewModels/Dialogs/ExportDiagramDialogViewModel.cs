using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Diagrammatist.Presentation.WPF.Core.Shared.Records;
using Diagrammatist.Presentation.WPF.Core.Shared.Enums;
using MvvmDialogs;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json.Linq;

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
        private ExportScenario selectedExportScenario = ExportScenario.Full;

        public ExportScenario SelectedExportScenario
        {
            get => selectedExportScenario;
            set
            {
                SetProperty(ref selectedExportScenario, value, true);
                if (value is not ExportScenario.Content)
                {
                    ContentMargin = "0";
                }
            }
        }

        /// <summary>
        /// Gets or sets the content margin.
        /// </summary>
        [ObservableProperty]
        [Required]
        [RegularExpression("([0-9]+)")]
        private string contentMargin = "0";

        /// <summary>
        /// Gets the collection of export PPI (Pixels Per Inch) values available for selection.
        /// </summary>
        public ObservableCollection<ExportPPI> ExportPPIValues { get; }

        private ExportPPI selectedPpi = ExportPPI.PPI_96;

        /// <summary>
        /// Gets or sets the selected PPI value for exporting diagrams.
        /// </summary>
        public ExportPPI SelectedPpi
        {
            get => selectedPpi;
            set
            {
                SetProperty(ref selectedPpi, value, true);
                if (value is not ExportPPI.PPI_Custom)
                {
                    CustomPpi = ((int)SelectedPpi).ToString();
                }
            }
        }

        /// <summary>
        /// Gets or sets the custom PPI value for exporting diagrams.
        /// </summary>
        [ObservableProperty]
        [Required]
        [RegularExpression("([1-9][0-9]*)")]
        private string customPpi = "96";

        /// <summary>
        /// Gets or sets the export settings for the diagram export dialog.
        /// </summary>
        [ObservableProperty]
        private ExportSettings? _exportSettings;

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
            ValidateAllProperties();

            if (HasErrors)
            {
                return;
            }

            ExportSettings = new ExportSettings(SelectedExportScenario,
                                                int.TryParse(ContentMargin, out var margin) ? margin : 0,
                                                int.TryParse(CustomPpi, out var ppi) ? ppi : (int)ExportPPI.PPI_72);
            DialogResult = true;
        }
    }
}
