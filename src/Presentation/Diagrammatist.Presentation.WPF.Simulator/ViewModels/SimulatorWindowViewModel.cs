using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Diagrammatist.Presentation.WPF.Core.Messaging.RequestMessages;
using Diagrammatist.Presentation.WPF.Core.Models.Canvas;
using Diagrammatist.Presentation.WPF.Simulator.Models.Engine;
using Diagrammatist.Presentation.WPF.Simulator.Models.Engine.Flowchart;
using MvvmDialogs;

namespace Diagrammatist.Presentation.WPF.Simulator.ViewModels
{
    /// <summary>
    /// A class that derived from <see cref="ObservableObject"/>. This class defines simulator view model.
    /// </summary>
    public partial class SimulatorWindowViewModel : ObservableRecipient, IModalDialogViewModel
    {
        private readonly ISimulationEngine _simulationEngine;

        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Behaviors/Member[@name="CurrentCanvas"]/*'/>
        [ObservableProperty]
        private CanvasModel _currentCanvas;

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

            _simulationEngine = currentCanvas.Settings.Type switch
            {
                DiagramsModel.Flowchart => new FlowchartSimulationEngine(),
                _ => throw new ArgumentException("Unsupported type"),
            };

            CurrentCanvas = currentCanvas;
        }

        /// <summary>
        /// Starts simulation in simulation window.
        /// </summary>
        [RelayCommand]
        private void StartSimulation()
        {
            _simulationEngine.Start();
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
