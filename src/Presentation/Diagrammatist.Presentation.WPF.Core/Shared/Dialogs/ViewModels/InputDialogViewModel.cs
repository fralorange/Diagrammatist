using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MvvmDialogs;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Diagrammatist.Presentation.WPF.Core.Shared.Dialogs.ViewModels
{
    /// <summary>
    /// A class that derives from <see cref="ObservableValidator"/> and implements <see cref="IModalDialogViewModel"/>.
    /// Allows user to input values from keyboard.
    /// </summary>
    public partial class InputDialogViewModel : ObservableValidator, IModalDialogViewModel
    {
        /// <summary>
        /// Gets variable names.
        /// </summary>
        public List<string> VariableNames { get; private set; }

        private bool? _dialogResult;

        /// <inheritdoc/>
        public bool? DialogResult
        {
            get => _dialogResult;
            private set => SetProperty(ref _dialogResult, value);
        }

        private Dictionary<string, string> _userInputs;

        [CustomValidation(typeof(InputDialogViewModel), nameof(ValidateUserInputs))]
        public ObservableCollection<KeyValueEntry.KeyValueEntry> UserInputs { get; }

        /// <summary>
        /// Initializes <see cref="InputDialogViewModel"/>.
        /// </summary>
        /// <param name="variables"></param>
#pragma warning disable CS8618
        public InputDialogViewModel(IEnumerable<string> variables)
#pragma warning restore CS8618 
        {
            VariableNames = variables.ToList();
            UserInputs = new ObservableCollection<KeyValueEntry.KeyValueEntry>(
                variables.Select(v => new KeyValueEntry.KeyValueEntry(v, string.Empty))
            );
        }

        /// <summary>
        /// Closes dialog window if has no errors.
        /// </summary>
        [RelayCommand]
        private void Ok()
        {
            ValidateAllProperties();

            if (HasErrors)
            {
                return;
            }

            DialogResult = true;
        }

        /// <summary>
        /// Validates user inputs.
        /// </summary>
        /// <param name="inputs"></param>
        /// <returns></returns>
        public static ValidationResult? ValidateUserInputs(ObservableCollection<KeyValueEntry.KeyValueEntry> inputs)
        {
            foreach (var entry in inputs)
            {
                if (string.IsNullOrEmpty(entry.Value))
                {
                    return new ValidationResult("The inputs was not validated.");
                }
            }
            return ValidationResult.Success;
        }
    }
}
