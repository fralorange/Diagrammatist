using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Diagrammatist.Presentation.WPF.Core.Messaging.RequestMessages;
using Diagrammatist.Presentation.WPF.Core.Models.Canvas;
using Diagrammatist.Presentation.WPF.Core.Models.Connection;
using Diagrammatist.Presentation.WPF.Core.Models.Figures.Special.Flowchart;
using Diagrammatist.Presentation.WPF.Simulator.Models.Engine;
using Diagrammatist.Presentation.WPF.Simulator.Models.Engine.Flowchart;
using Diagrammatist.Presentation.WPF.Simulator.Models.Node;
using Diagrammatist.Presentation.WPF.Simulator.Models.Node.Flowchart;
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
        private SimulationNodeBase? _simulationNode;

        /// <summary>
        /// Gets or sets simulation interval between steps.
        /// </summary>
        /// <remarks>
        /// By default its 5000 ms.
        /// </remarks>
        [ObservableProperty]
        private int _simulationInterval = 5000;

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
        /// <param name="currentCanvas">Current canvas.</param>
        /// <exception cref="ArgumentException"></exception>
        public SimulatorWindowViewModel()
        {
            var currentCanvas = Messenger.Send(new CurrentCanvasRequestMessage()).Response;

            if (currentCanvas is null)
                throw new ArgumentNullException(nameof(currentCanvas));

            Nodes = new ObservableCollection<SimulationNodeBase>(currentCanvas.Figures
                .OfType<FlowchartFigureModel>()
                .Select(f => new FlowchartSimulationNode { Figure = f}));

            Connections = currentCanvas.Connections;

            SimulationSize = new Size(currentCanvas.Settings.Width, currentCanvas.Settings.Height);

            _simulationEngine = currentCanvas.Settings.Type switch
            {
                DiagramsModel.Flowchart => new FlowchartSimulationEngine(currentCanvas),
                _ => throw new ArgumentException("Unsupported type"),
            };
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
    }
}
