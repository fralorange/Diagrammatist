using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Diagrammatist.Application.AppServices.Document.Services;
using Diagrammatist.Presentation.WPF.Core.Foundation.Extensions;
using Diagrammatist.Presentation.WPF.Core.Helpers;
using Diagrammatist.Presentation.WPF.Core.Messaging.RequestMessages;
using Diagrammatist.Presentation.WPF.Core.Models.Connection;
using Diagrammatist.Presentation.WPF.Core.Models.Figures;
using Diagrammatist.Presentation.WPF.Core.Models.Figures.Special.Flowchart;
using Diagrammatist.Presentation.WPF.Core.Services.Alert;
using Diagrammatist.Presentation.WPF.Simulator.Models.Context;
using Diagrammatist.Presentation.WPF.Simulator.Models.Engine;
using Diagrammatist.Presentation.WPF.Simulator.Models.Engine.Args;
using Diagrammatist.Presentation.WPF.Simulator.Models.Node;
using Diagrammatist.Presentation.WPF.Simulator.Providers;
using MvvmDialogs;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace Diagrammatist.Presentation.WPF.Simulator.ViewModels
{
    /// <summary>
    /// A class that derived from <see cref="ObservableObject"/>. This class defines simulator view model.
    /// </summary>
    public partial class SimulatorWindowViewModel : ObservableRecipient, IModalDialogViewModel
    {
        private readonly ISimulationEngine _simulationEngine;
        private readonly IAlertService _alertService;

        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Behaviors/Member[@name="RequestOpen"]/*'/>
        public event Func<string>? RequestOpen;

        /// <summary>
        /// Occurs when user wants to apply changes.
        /// </summary>
        /// <remarks>
        /// This event marks current file as 'has changes'.
        /// </remarks>
        public event Action? RequestApply;

        /// <summary>
        /// Gets or sets current node in simulation.
        /// </summary>
        [ObservableProperty]
        private SimulationNode? _currentNode;

        /// <summary>
        /// Gets or sets selected node in simulation.
        /// </summary>
        [ObservableProperty]
        private SimulationNode? _selectedNode;

        /// <summary>
        /// Gets or sets simulation window size.
        /// </summary>
        [ObservableProperty]
        private Size _simulationSize;

        /// <summary>
        /// Gets simulation nodes.
        /// </summary>
        public ObservableCollection<SimulationNode> Nodes { get; }

        /// <summary>
        /// Gets connections.
        /// </summary>
        public ObservableCollection<ConnectionModel> Connections { get; }

        /// <summary>
        /// Gets annotations.
        /// </summary>
        public ObservableCollection<TextFigureModel> Annotations { get; }

        private bool? _dialogResult;

        /// <inheritdoc/>
        public bool? DialogResult
        {
            get => _dialogResult;
            private set => SetProperty(ref _dialogResult, value);
        }

        /// <summary>
        /// Gets or sets the change state of the current simulation.
        /// </summary>
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ApplyCommand))]
        private bool _hasChanges;

        /// <summary>
        /// Gets or sets new context, if user confirmed changes.
        /// </summary>
        public SimulationContext? NewContext { get; private set; }

        /// <summary>
        /// Initializes simulator view model.
        /// </summary>
        /// <param name="dialogService"></param>
        /// <param name="alertService"></param>
        /// <param name="documentSerializationService"></param>
        /// <exception cref="ArgumentException"></exception>
        public SimulatorWindowViewModel(IDialogService dialogService,
                                        IAlertService alertService,
                                        IDocumentSerializationService documentSerializationService,
                                        SimulationContext? payload = null,
                                        Action? onTerminate = null)
        {
            _alertService = alertService;

            // Validation.
            var currentCanvas = Messenger.Send(new CurrentCanvasRequestMessage()).Response;

            ArgumentNullException.ThrowIfNull(currentCanvas, nameof(currentCanvas));

            // Factory.
            var factory = SimulationFactoryProvider.GetFactory(currentCanvas.Settings.Type);

            var createdNodes = factory.CreateNodes(currentCanvas.Figures, payload?.Nodes);
            Nodes = createdNodes.ToObservableCollection();
            Connections = currentCanvas.Connections;
            Annotations = currentCanvas.Figures.OfType<TextFigureModel>().ToObservableCollection();

            // Simulation parameters.
            SimulationSize = new Size(currentCanvas.Settings.Width, currentCanvas.Settings.Height);

            var simIO = new SimulationDialogIOProvider(dialogService, this);
            var simContextProvider = new SimulationContextProvider(documentSerializationService);

            _simulationEngine = factory.CreateEngine(Nodes, Connections, simIO, simContextProvider);
            _simulationEngine.CurrentNodeChanged += (sender, node)
                => CurrentNode = node;
            _simulationEngine.ErrorOccurred += (sender, e) =>
            {
                SimulationEngine_ErrorOccured(sender, e);
                onTerminate?.Invoke();
            };
            _simulationEngine.Initialize();
        }

        private bool CanApply() => HasChanges;

        /// <summary>
        /// Takes one step forward in simulation window.
        /// </summary>
        [RelayCommand]
        private void StepForward()
        {
            _simulationEngine.StepForward();
        }

        /// <summary>
        /// Takes one step backwards in simulation window.
        /// </summary>
        [RelayCommand]
        private void StepBackward()
        {
            _simulationEngine.StepBackward();
        }

        /// <summary>
        /// Resets simulation to the start in simulation window.
        /// </summary>
        [RelayCommand]
        private void ResetSimulation()
        {
            _simulationEngine.Reset();
        }

        /// <summary>
        /// Loads file to selected node.
        /// </summary>
        [RelayCommand]
        private void LoadFile()
        {
            if (RequestOpen is null || SelectedNode is null) return;

            var filePath = RequestOpen();

            if (string.IsNullOrEmpty(filePath)) return;

            SelectedNode.ExternalFilePath = filePath;
        }

        /// <summary>
        /// Confirms changes.
        /// </summary>
        [RelayCommand]
        private void OK()
        {
            NewContext = new() { Nodes = Nodes, Connections = Connections };
            DialogResult = true;
        }

        /// <summary>
        /// Applies changes.
        /// </summary>
        [RelayCommand(CanExecute = nameof(CanApply))]
        private void Apply()
        {
            NewContext = new() { Nodes = Nodes, Connections = Connections };
            HasChanges = false;
            RequestApply?.Invoke();
        }

        private void SelectedNode_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SimulationNode.LuaScript) ||
                e.PropertyName == nameof(SimulationNode.ExternalFilePath))
            {
                HasChanges = true;
            }
        }

        private void SimulationEngine_ErrorOccured(object? sender, SimulationErrorEventArgs e)
        {
            var subtype = (e.Node?.Figure as FlowchartFigureModel)?.Subtype.ToString() ?? string.Empty;

            var localizedMessage = LocalizationHelper
                .GetLocalizedValue<string>("SimulatorResources", $"{e.Message}{subtype}Message");

            var localizedCaption = LocalizationHelper
                .GetLocalizedValue<string>("SimulatorResources", $"{e.Message}Caption");

            _alertService.ShowError(localizedMessage, localizedCaption);
        }

        /// <summary>
        /// Handles selected node changing.
        /// </summary>
        /// <remarks>
        /// Occurs when selected node changes.
        /// </remarks>
        /// <param name="value"></param>
        partial void OnSelectedNodeChanged(SimulationNode? oldValue, SimulationNode? newValue)
        {
            if (oldValue is not null)
            {
                oldValue.PropertyChanged -= SelectedNode_PropertyChanged;
            }

            if (newValue is not null)
            {
                newValue.PropertyChanged += SelectedNode_PropertyChanged;
            }
        }
    }
}
