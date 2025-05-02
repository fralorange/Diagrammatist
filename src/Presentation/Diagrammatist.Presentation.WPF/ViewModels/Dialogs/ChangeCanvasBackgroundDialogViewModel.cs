using ColorPicker.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MvvmDialogs;
using System.Windows.Media;

namespace Diagrammatist.Presentation.WPF.ViewModels.Dialogs
{
    /// <summary>
    /// A class that represents viewmodel of change canvas background dialog window.
    /// </summary>
    public partial class ChangeCanvasBackgroundDialogViewModel : ObservableObject, IModalDialogViewModel
    {
        private bool? _dialogResult;

        /// <inheritdoc/>
        public bool? DialogResult
        {
            get => _dialogResult;
            private set => SetProperty(ref _dialogResult, value);
        }

        private readonly ColorState _colorState = new();

        /// <summary>
        /// Gets or sets the color state of the canvas background.
        /// </summary>
        public ColorState ColorState => _colorState;

        /// <summary>
        /// Gets or sets the color of the canvas background.
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// Initializes 'change canvas background' viewmodel.
        /// </summary>
        /// <param name="color"></param>
        public ChangeCanvasBackgroundDialogViewModel(Color color)
        {
            ColorState.SetARGB(color.A, color.R, color.G, color.B);
        }

        /// <summary>
        /// Confirms changes.
        /// </summary>
        [RelayCommand]
        private void Ok()
        {
            DialogResult = true;
        }
    }
}
