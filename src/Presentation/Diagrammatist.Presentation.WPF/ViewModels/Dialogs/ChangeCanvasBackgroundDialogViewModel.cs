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

        /// <summary>
        /// Gets or sets color.
        /// </summary>
        [ObservableProperty]
        private Color _color;

        /// <summary>
        /// Initializes 'change canvas background' viewmodel.
        /// </summary>
        /// <param name="color"></param>
        public ChangeCanvasBackgroundDialogViewModel(Color color)
        {
            Color = color;
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
