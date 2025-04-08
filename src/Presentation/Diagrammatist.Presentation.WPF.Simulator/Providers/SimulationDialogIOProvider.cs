using Diagrammatist.Presentation.WPF.Core.Shared.Dialogs.ViewModels;
using Diagrammatist.Presentation.WPF.Simulator.Interfaces;
using MvvmDialogs;
using System.ComponentModel;

namespace Diagrammatist.Presentation.WPF.Simulator.Providers
{
    /// <summary>
    /// A class that implements <see cref="ISimulationIO"/>. Provides with input/output operations.
    /// </summary>
    public class SimulationDialogIOProvider : ISimulationIO
    {
        private readonly IDialogService _dialogService;
        private readonly INotifyPropertyChanged _owner;

        /// <summary>
        /// Initializes simulation dialog IO provider.
        /// </summary>
        /// <param name="dialogService"></param>
        /// <param name="owner"></param>
        public SimulationDialogIOProvider(IDialogService dialogService, INotifyPropertyChanged owner)
        {
            _dialogService = dialogService;
            _owner = owner;
        }

        /// <inheritdoc/>
        public Dictionary<string, string>? GetInput(List<string> variableNames)
        {
            var viewModel = new InputDialogViewModel(variableNames);

            _dialogService.ShowDialog(_owner, viewModel);
            if (viewModel.DialogResult == true)
            {
                return viewModel.UserInputs.ToDictionary(kv => kv.Key, kv => kv.Value);
            }

            return null;
        }

        /// <inheritdoc/>
        public void ShowOutput(string message)
        {
            var viewModel = new OutputDialogViewModel(message);

            _dialogService.ShowDialog(_owner, viewModel);
        }
    }
}
