using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Diagrammatist.Presentation.WPF.Core.Foundation.Extensions;
using Diagrammatist.Presentation.WPF.Core.Messaging.RequestMessages;
using Diagrammatist.Presentation.WPF.Core.Models.Connection;
using Diagrammatist.Presentation.WPF.Simulator.Models.Context;
using Diagrammatist.Presentation.WPF.Simulator.Models.Engine;
using Diagrammatist.Presentation.WPF.Simulator.Models.Node;
using Diagrammatist.Presentation.WPF.Simulator.Providers;
using MvvmDialogs;
using System.Collections.ObjectModel;
using System.Windows;

namespace Diagrammatist.Presentation.WPF.Simulator.ViewModels
{
    /// <summary>
    /// A class that derived from <see cref="ObservableObject"/>. This class defines simulator view model.
    /// </summary>
    public partial class SimulatorWindowViewModel : ObservableRecipient, IModalDialogViewModel
    {
        private readonly ISimulationEngine _simulationEngine;

        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Behaviors/Member[@name="RequestOpen"]/*'/>
        public event Func<string>? RequestOpen;

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
        /// Gets or sets simulation interval between steps.
        /// </summary>
        /// <remarks>
        /// By default its 5000 ms.
        /// </remarks>
        [ObservableProperty]
        private int _simulationInterval = 5000;

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

        private bool? _dialogResult;

        /// <inheritdoc/>
        public bool? DialogResult
        {
            get => _dialogResult;
            private set => SetProperty(ref _dialogResult, value);
        }

        /// <summary>
        /// Gets or sets new context, if user confirmed changes.
        /// </summary>
        public SimulationContext? NewContext { get; private set; }

        /// <summary>
        /// Initializes simulator view model.
        /// </summary>
        /// <param name="dialogService"></param>
        /// <exception cref="ArgumentException"></exception>
        public SimulatorWindowViewModel(IDialogService dialogService, SimulationContext? payload = null)
        {
            // Validation.
            var currentCanvas = Messenger.Send(new CurrentCanvasRequestMessage()).Response;

            ArgumentNullException.ThrowIfNull(currentCanvas, nameof(currentCanvas));

            // Factory.
            var factory = SimulationFactoryProvider.GetFactory(currentCanvas.Settings.Type);

            var createdNodes = factory.CreateNodes(currentCanvas.Figures, payload?.Nodes);
            var createdConnections = factory.CreateConnections(currentCanvas.Connections, payload?.Connections);
            Nodes = createdNodes.ToObservableCollection();
            Connections = createdConnections.ToObservableCollection();

            // Simulation parameters.
            SimulationSize = new Size(currentCanvas.Settings.Width, currentCanvas.Settings.Height);

            var simIO = new SimulationDialogIOProvider(dialogService, this);

            _simulationEngine = factory.CreateEngine(Nodes, Connections, simIO);
            _simulationEngine.CurrentNodeChanged += (sender, node) 
                => CurrentNode = node;
            _simulationEngine.Initialize();
        }

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
    }
}
