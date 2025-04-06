using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Diagrammatist.Presentation.WPF.Core.Messaging.RequestMessages;
using Diagrammatist.Presentation.WPF.Core.Models.Connection;
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

        /// <summary>
        /// Gets or sets current node in simulation.
        /// </summary>
        [ObservableProperty]
        private SimulationNodeBase? _currentNode;

        /// <summary>
        /// Gets or sets selected node in simulation.
        /// </summary>
        [ObservableProperty]
        private SimulationNodeBase? _selectedNode;

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
        public ObservableCollection<SimulationNodeBase> Nodes { get; }

        /// <summary>
        /// Gets connections.
        /// </summary>
        public ObservableCollection<ConnectionModel> Connections { get; }

        /// <inheritdoc/>
        public bool? DialogResult => true;

        /// <summary>
        /// Initializes simulator view model.
        /// </summary>
        /// <param name="dialogService"></param>
        /// <exception cref="ArgumentException"></exception>
        public SimulatorWindowViewModel(IDialogService dialogService)
        {
            // Validation.
            var currentCanvas = Messenger.Send(new CurrentCanvasRequestMessage()).Response;

            ArgumentNullException.ThrowIfNull(currentCanvas, nameof(currentCanvas));

            // Factory.
            var factory = SimulationFactoryProvider.GetFactory(currentCanvas.Settings.Type);

            var createdNodes = factory.CreateNodes(currentCanvas.Figures);
            Nodes = new ObservableCollection<SimulationNodeBase>(createdNodes);
            Connections = currentCanvas.Connections;

            // Simulation parameters.
            SimulationSize = new Size(currentCanvas.Settings.Width, currentCanvas.Settings.Height);

            var simIO = new SimulationDialogIOProvider(dialogService, this);

            _simulationEngine = factory.CreateEngine(Nodes, Connections, simIO);
            _simulationEngine.CurrentNodeChanged += (sender, node) 
                => CurrentNode = node;
            _simulationEngine.Initialize();
        }

        /// <summary>
        /// Starts simulation in simulation window.
        /// </summary>
        [RelayCommand]
        private void StartSimulation()
        {
            _simulationEngine.Start(SimulationInterval);
        }

        /// <summary>
        /// Stops simulation in simulation window.
        /// </summary>
        /// <remarks>
        /// *Acts as pause.
        /// </remarks>
        [RelayCommand]
        private void StopSimulation()
        {
            _simulationEngine.Stop();
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
            //_simulationEngine.StepBackward();
        }

        /// <summary>
        /// Resets simulation to the start in simulation window.
        /// </summary>
        [RelayCommand]
        private void ResetSimulation()
        {
            _simulationEngine.Reset();
        }
    }
}
